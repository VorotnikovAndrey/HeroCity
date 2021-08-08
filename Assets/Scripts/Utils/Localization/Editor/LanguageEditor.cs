//Mike Hergaarden - M2H.nl

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Localization
{
    public class LanguageEditor : EditorWindow
    {
        private const string DOC_URL = "https://docs.google.com/spreadsheets/d/e/2PACX-1vQkybasduDynylKJdBPN17tsUmqud6ulnz4tC1Adkod3wtG71kiy_B_mN6wRWMPIshyFGeGrLvccf8N/pubhtml";
        private const string PROXY_URL = "http://www.M2H.nl/unity/editorPageProxy.php";

        private LocalizationSettings settings = null;
        private bool getFixedArabic;

        [MenuItem("Tools/Localization")]
        private static void OpenWindow()
        {
            EditorWindow.GetWindow(typeof(LanguageEditor));
        }

        private string gDocsURL = DOC_URL;
        private int unresolvedErrors = 0;
        private int foundSheets = 0;
        private bool useSystemLang = true;
        private LanguageCode langCode = LanguageCode.EN;
        private bool loadedSettings = false;
        private Vector2 scrollView;

        private void LoadSettings()
        {
            if (loadedSettings || settings == null)
                return;

            loadedSettings = true;
            gDocsURL = DOC_URL;

            if (File.Exists(LocalizationSettings.SETTINGS_ASSET_PATH))
            {
                settings = AssetDatabase.LoadAssetAtPath<LocalizationSettings>(LocalizationSettings.SETTINGS_ASSET_PATH);
            }
            else
            {
                Debug.LogErrorFormat("[LoadSettings] settings file not exist, path: {0}", LocalizationSettings.SETTINGS_ASSET_PATH);
            }

            useSystemLang = settings.useSystemLanguagePerDefault;
            langCode = settings.defaultLangCode;
        }

        private void OnGUI()
        {
            if (EditorApplication.isPlaying)
            {
                GUILayout.Label("Editor is in play mode.");
                return;
            }
            loadedSettings = false;
            LoadSettings();

            scrollView = GUILayout.BeginScrollView(scrollView);

            GUILayout.Label("Settings", EditorStyles.boldLabel);
            useSystemLang = EditorGUILayout.Toggle("Try system language", useSystemLang);
            getFixedArabic = EditorGUILayout.Toggle("Get fixed arabic", getFixedArabic);
            langCode = (LanguageCode)EditorGUILayout.EnumPopup("Default language", langCode);
            gDocsURL = EditorGUILayout.TextField("gDocs Link", gDocsURL);

            if (GUI.changed)
            {
                SaveSettingsFile();
            }

            if (GUILayout.Button("Update translations"))
            {
                gDocsURL = gDocsURL.Trim();

                if (!gDocsURL.Contains(".google.com") || !gDocsURL.EndsWith("/pubhtml"))
                {
                    EditorUtility.DisplayDialog("Error", "You have entered an incorrect spreadsheet URL. Support only google spreadsheets published as webpage", "OK");
                }
                else
                {
                    EditorPrefs.SetString(PlayerSettings.productName + "gDocs", gDocsURL);
                    DownloadLocalization(gDocsURL);
                }
            }

            if (unresolvedErrors > 0)
            {
                Rect rec = GUILayoutUtility.GetLastRect();
                GUI.color = Color.red;
                EditorGUI.DropShadowLabel(new Rect(0, rec.yMin + 15, 200, 20), "Unresolved errors: " + unresolvedErrors);
                GUI.color = Color.white;
            }

            GUILayout.Space(25);
            GUILayout.Label("For full instructions read the localization package documentation.", EditorStyles.miniLabel);
            if (GUILayout.Button("Open documentation"))
            {
                Application.OpenURL("http://www.M2H.nl/files/LocalizationPackage.pdf");
            }
            if (GUILayout.Button("Verify localized assets"))
            {
                VerifyLocAssets();
            }
            if (GUILayout.Button("More Unity resources"))
            {
                Application.OpenURL("http://www.M2H.nl/unity/");
            }

            GUILayout.EndScrollView();
        }

        private static string PregReplace(string input, string[] pattern, string[] replacements)
        {
            if (replacements.Length != pattern.Length)
                throw new ArgumentException("Replacement and Pattern Arrays must be balanced");

            for (var i = 0; i < pattern.Length; i++)
            {
                input = Regex.Replace(input, pattern[i], replacements[i]);
            }

            return input;
        }

        private void VerifyLocAssets()
        {
            string langRootFolder = GetLanguageFolder() + "/Assets/";
            string[] files = Directory.GetFiles(langRootFolder, "*", SearchOption.AllDirectories);

            List<string> langList = new List<string>();
            List<string> uniqueAssets = new List<string>();
            foreach (string file in files)
            {
                if (file.Length > 5 && file.Substring(file.Length - 5, 5) == ".meta")
                    continue;//Ignore meta files

                string file2 = file.Substring(langRootFolder.Length);

                string lang = file2.Substring(0, 2);
                if (!langList.Contains(lang))
                    langList.Add(lang);
                string uniqueAsset = file2.Substring(3);
                if (!uniqueAssets.Contains(uniqueAsset))
                {
                    uniqueAssets.Add(uniqueAsset);
                }
            }

            int missing = 0;
            //Test assets
            foreach (string lang in langList)
            {
                foreach (string asset in uniqueAssets)
                {
                    if (!File.Exists(langRootFolder + lang + "/" + asset))
                    {
                        missing++;
                        Debug.LogError("[" + lang + "] MISSING " + langRootFolder + lang + "/" + asset);
                    }
                }
            }

            if (missing > 0)
            {
                EditorUtility.DisplayDialog("Verifying assets", "Missing asset translations: " + missing + ". See console for details.", "OK");
            }
            else
            {
                EditorUtility.DisplayDialog("Verifying assets", "All seems to be OK!", "OK");
            }
        }

        private void DownloadLocalization(string gDocsPage)
        {
            SaveSettingsFile();

            gDocsPage += ((gDocsPage.Contains("&")) ? "&" : "?") + "timestamp=" + EditorApplication.timeSinceStartup;//Prevent caching

            float progress = 0.1f;

            string output = GetWebpage(gDocsPage);

            var document = new HtmlAgilityPack.HtmlDocument();
            document.LoadHtml(output);

            List<SheetInfo> sheetIDs = GetSpreadSheetIDs(document);
            Debug.Log(sheetIDs.Count.ToString());
            settings.sheetTitles = new string[0];
            unresolvedErrors = 0;
            foundSheets = 0;

            for (int i = sheetIDs.Count - 1; i >= 0; i--)
            {
                int downloadSheet = sheetIDs[i].ID;

                EditorUtility.DisplayProgressBar("Downloading gDoc data", "Page: " + downloadSheet, progress);

                if (!ParseData(document, sheetIDs[i].title, i))
                {
                    //Failed
                    Debug.LogErrorFormat("ParseData Error url:{0}", gDocsPage);
                }
            }

            if (foundSheets == 0)
            {
                EditorUtility.DisplayDialog("Error", "No sheets could be imported. Either they were all empty, or you entered a wrong link. Please copy your LINK again and verify your spreadsheet.", "OK");
            }

            EditorUtility.ClearProgressBar();
            if (unresolvedErrors > 0)
            {
                EditorUtility.DisplayDialog("Errors", "There are " + unresolvedErrors + " open errors in your localization. See the console for more information.", "OK");
            }
        }

        private void LoadHTML(Hashtable loadLanguages, Hashtable loadEntries, HtmlAgilityPack.HtmlDocument document, string sheetTitle, int sheetID)
        {
            int tableNR = 0;
            foreach (HtmlAgilityPack.HtmlNode node in document.DocumentNode.SelectNodes("//table"))
            {
                if (tableNR == sheetID)
                {
                    //Only parse 1 "sheet"
                    ParseHTMLTable(loadLanguages, loadEntries, node);
                }
                tableNR++;
                foundSheets++;
            }

            //Parse sheet titles
            List<string> sheetNames = new List<string>(settings.sheetTitles);

            List<string> newTitles = GetSheetTitles(document);
            foreach (string newTitl in newTitles)
            {
                if (!sheetNames.Contains(newTitl))
                    sheetNames.Add(newTitl);
            }

            settings.sheetTitles = sheetNames.ToArray();
            SaveSettingsFile();
        }

        private void ParseHTMLTable(Hashtable loadLanguages, Hashtable loadEntries, HtmlAgilityPack.HtmlNode node)
        {
            int row = -1;
            int langCount = 0;
            bool firstRow = true;

            foreach (HtmlAgilityPack.HtmlNode trNode in node.SelectNodes(".//tr"))
            {
                if (trNode.SelectNodes(".//td") == null)
                {
                    continue;
                }
                row++;
                int i = -1;
                string key = "";

                foreach (HtmlAgilityPack.HtmlNode tdNode in trNode.SelectNodes(".//td"))
                {
                    i++;

                    if (firstRow && row == 0)
                    {
                        //Language header
                        if (i == 0)
                            continue; //Ignore this top-left empty cell	

                        if (tdNode.InnerText != "")
                        {
                            langCount++;
                            loadLanguages[i] = tdNode.InnerText;
                            if (!loadEntries.ContainsKey(i))
                                loadEntries[i] = new Hashtable();
                        }
                    }
                    else
                    {
                        //Data rows				

                        if (i == 0)
                        {
                            key = tdNode.InnerText;
                            continue;
                        }
                        if (key == "")
                            continue; //Skip entries with empty keys (the other values can be used as labels)

                        if (i > langCount)
                            continue;

                        string content = tdNode.InnerText.Replace("&#39;", "'");
                        if (loadEntries[i] == null)
                            loadEntries[i] = new Hashtable();
                        Hashtable hTable = (Hashtable)loadEntries[i];
                        if (hTable == null)
                        {
                            Debug.LogError($"Loc parse error on key {key}");
                            unresolvedErrors++;
                            continue;
                        }
                        if (hTable.ContainsKey(key))
                        {
                            Debug.LogError("ERROR: Double key [" + key + "]");
                            unresolvedErrors++;
                        }
                        hTable[key] = content;
                    }
                }
                firstRow = false;
            }
        }

        private bool ParseData(HtmlAgilityPack.HtmlDocument document, string sheetTitle, int sheetID)
        {
            string langFolder = GetLanguageFolder();

            Hashtable loadLanguages = new Hashtable();
            Hashtable loadEntries = new Hashtable();

            LoadHTML(loadLanguages, loadEntries, document, sheetTitle, sheetID);

            if (loadEntries.Count < 1)
            {
                unresolvedErrors++;
                Debug.LogError("Sheet " + sheetTitle + " contains no languages!");
                return false;
            }
            //Verify loaded data
            Hashtable sampleData = (Hashtable)loadEntries[1];
            for (int j = 2; j < loadEntries.Count; j++)
            {
                Hashtable otherData = ((Hashtable)loadEntries[j]);

                foreach (DictionaryEntry item in otherData)
                {
                    if (!sampleData.ContainsKey(item.Key))
                    {
                        Debug.LogError("[" + loadLanguages[1] + "] [" + item.Key + "] Key is missing!");
                        unresolvedErrors++;
                    }
                }

                foreach (DictionaryEntry item in sampleData)
                {
                    if (!otherData.ContainsKey(item.Key))
                    {
                        Debug.LogError("Sheet(" + sheetTitle + ") [" + loadLanguages[j] + "] [" + item.Key + "] Key is missing!");
                        unresolvedErrors++;
                    }
                }
            }

            //Save the loaded data
            foreach (DictionaryEntry langs in loadLanguages)
            {
                LanguageAsset asset = CreateInstance<LanguageAsset>();

                string langCode = ((string)langs.Value).TrimEnd(System.Environment.NewLine.ToCharArray());
                int langID = (int)langs.Key;
                Hashtable entries = (Hashtable)loadEntries[langID];
                foreach (DictionaryEntry item in entries)
                {
                    asset.Data.Add(new LanguageAsset.LanguageData() { key = item.Key + "", value = StringExtensions.UnescapeXML((item.Value + "")).Replace("\\n", "\n") });
                }

                foundSheets++;

                if (sheetTitle != settings.predefSheetTitle)
                    AssetDatabase.CreateAsset(asset, string.Format("{0}{1}_{2}.asset", LocalizationSettings.BUNDLE_PATH, langCode, sheetTitle));
                else
                    AssetDatabase.CreateAsset(asset, string.Format("{0}{1}_{2}.asset", LocalizationSettings.PREDEF_PATH, langCode, sheetTitle));
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            return true;
        }

        private string GetWebpage(string url)
        {
            //Switch to standalone as we need to be able to do unrestricted WWW calls: Google does not host a crossdomain.xml
            if (EditorUserBuildSettings.selectedBuildTargetGroup == BuildTargetGroup.WebGL)
            {
                EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.StandaloneWindows);
            }

            WWW wwwReq = new WWW(url);
            while (!wwwReq.isDone)
            {
            }
            if (wwwReq.error == null)
            {
                return wwwReq.text;
            }
            else
            {
                //Unity Editor needs Crossdomain.xml when running in Editor with Webplayer target
                //If you are concerned about privacy/performance: You can also host this proxy yourself, see PDF
                Debug.LogWarning("Error grabbing gDocs data:" + wwwReq.error + " (URL=" + url + ")");
                Debug.LogWarning("Trying again via proxy. Switch to standalone target to prevent this!");

                WWWForm form = new WWWForm();
                form.AddField("page", url);
                WWW wwwReq2 = new WWW(PROXY_URL, form);
                while (!wwwReq2.isDone)
                {
                }
                if (wwwReq2.error == null)
                {
                    return wwwReq2.text;
                }
                else
                {
                    Debug.LogError(wwwReq2.error);
                }
            }
            return "";

        }

        private string GetLanguageFolder()
        {
            string[] subdirEntries = Directory.GetDirectories(Application.dataPath, "Languages", SearchOption.AllDirectories);
            foreach (string subDir in subdirEntries)
            {
                if (subDir.Contains("Resources"))
                {
                    string outp = subDir.Replace(Application.dataPath, "");
                    if (outp[0] == '\\') outp = outp.Substring(1);
                    return outp;
                }
            }

            //Create folder
            string folder = Application.dataPath + "/Localization";
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            folder = folder + "/Resources";
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
                AssetDatabase.ImportAsset(folder, ImportAssetOptions.ForceUpdate);
            }
            folder = folder + "/Languages";
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
                AssetDatabase.ImportAsset(folder, ImportAssetOptions.ForceUpdate);
            }
            AssetDatabase.Refresh();

            return folder;
        }

        private List<string> GetSheetTitles(HtmlAgilityPack.HtmlDocument document)
        {
            List<string> titles = new List<string>();

            if (document.DocumentNode.SelectNodes("//a") == null)
            {
                //Single sheet in this document
                HtmlAgilityPack.HtmlNode node = document.DocumentNode.SelectSingleNode("//span ");

                int cutOffFrom = node.InnerText.IndexOf(":") + 2;
                string title = node.InnerText.Substring(cutOffFrom);

                titles.Add(title);
            }
            else
            {
                //Parse sheet titles
                foreach (HtmlAgilityPack.HtmlNode node in document.DocumentNode.SelectNodes("//a"))
                {
                    var href = node.GetAttributeValue("href", string.Empty);
                    if (string.IsNullOrEmpty(href) || href == "#")
                        titles.Add(node.InnerText);
                }
            }
            return titles;

        }

        private List<SheetInfo> GetSpreadSheetIDs(HtmlAgilityPack.HtmlDocument document)
        {
            List<SheetInfo> res = new List<SheetInfo>();
            List<string> titles = GetSheetTitles(document);

            int i = 0;
            foreach (string title in titles)
            {
                SheetInfo inf = new SheetInfo();
                inf.ID = i;
                inf.title = title;
                res.Add(inf);
                i++;
            }

            if (res.Count == 0)
            {
                Debug.LogWarning("No sheets found, or your spreadsheet has only 1 sheet. We are assuming that the first sheet has ID '0'. (You can fix this by simply adding a second sheet as this will allow ID lookup via HTML output)");
                SheetInfo info = new SheetInfo();
                info.ID = 0;
                info.title = "Sheet1";
                res.Add(info);
            }
            return res;
        }

        private struct SheetInfo
        {
            public int ID;
            public string title;
        }

        private void SaveSettingsFile()
        {
            if (settings == null)
            {
                settings = (LocalizationSettings)ScriptableObject.CreateInstance(typeof(LocalizationSettings));
                string settingsPath = Path.GetDirectoryName(LocalizationSettings.SETTINGS_ASSET_PATH);

                if (!Directory.Exists(settingsPath))
                    Directory.CreateDirectory(settingsPath);

                if (!File.Exists(LocalizationSettings.SETTINGS_ASSET_PATH))
                {
                    AssetDatabase.CreateAsset(settings, LocalizationSettings.SETTINGS_ASSET_PATH);
                }
                else
                {
                    AssetDatabase.SaveAssets();
                }
            }

            settings.defaultLangCode = langCode;
            settings.useSystemLanguagePerDefault = useSystemLang;
            EditorUtility.SetDirty(settings);
        }

        public static bool IsArabic(string strCompare)
        {
            char[] chars = strCompare.ToCharArray();
            foreach (char ch in chars)
                if (ch >= '\u0627' && ch <= '\u0649')
                    return true;
            return false;
        }
    }
}
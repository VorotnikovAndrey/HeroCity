using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Defong;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace Economies.Editor
{
    public abstract class BaseEconomyEditor : UnityEditor.Editor
    {
        public static bool Downloading;

        private const string _webAssetsName = "WebAssets";
        
        public override void OnInspectorGUI()
        {
            serializedObject.UpdateIfRequiredOrScript();

            EditorGUILayout.PropertyField(serializedObject.FindProperty(_webAssetsName), true);
            EditorGUILayout.Space();

            var rects = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight * 2.5f).SplitHorizontal(3);
            
            if (GUI.Button(rects[0], "Go to web page"))
            {
                var container = ((EconomyFile) target).WebAssets;
                Application.OpenURL($"https://docs.google.com/spreadsheets/d/{container.DocumentId}/edit#gid={container.FileAssets.FirstOrDefault()?.SheetId}");
            }
            
            if (GUI.Button(rects[1], "Update locally"))
            {
                UpdateData();
            }
            
            if (GUI.Button(rects[2], "Update from web"))
            {
                Downloading = true;
                UpdateDataFromWeb();
            }

            EditorGUILayout.Space();
            
            serializedObject.ApplyModifiedProperties();
        }

        public void UpdateDataFromWeb()
        {
            var container = ((EconomyFile) target).WebAssets;
            if (container == null || string.IsNullOrEmpty(container.DocumentId) || container.FileAssets.Count == 0)
            {
                EditorUtility.DisplayDialog("Please fill in Web Assets data", "", "OK");
            }
            else
            {
                for (int i = 0; i < container.FileAssets.Count; i++)
                {
                    DownloadGoogleSheet(container.DocumentId, container.FileAssets[i]);
                }

                UpdateData();
            }
        }

        public void UpdateData()
        {
            List<string> files = new List<string>();
            var setup = serializedObject.FindProperty(_webAssetsName + ".FileAssets");
            for (int i = 0; i < setup.arraySize; i++)
            {
                files.Add(AssetDatabase.GetAssetPath(setup.GetArrayElementAtIndex(i).FindPropertyRelative("TextAsset").objectReferenceValue));
            }
            ParseFolder(files.ToArray());
        }
        
        protected abstract void ParseFolder(params string[] files);
        
        private void DownloadGoogleSheet(string docId, ParsingFileAsset fileAsset)
        {
            EditorUtility.DisplayCancelableProgressBar("Download", "Downloading...", 0);

            var iterator = DownloadSheet(docId, fileAsset.SheetId, t => DownloadComplete(t, fileAsset), DisplayDownloadProgressbar);
            while(iterator.MoveNext())
            {}
        }
        
        private IEnumerator DownloadSheet(string docsId, string sheetId, Action<string> done, Func<float, bool> progressbar = null)
        {
            if (progressbar != null && progressbar(0))
            {
                done(null);
                yield break;
            }

            var url = $"https://docs.google.com/spreadsheets/d/{docsId}/export?format=csv&gid={sheetId}";
            var www = UnityWebRequest.Get(url);
            www.SendWebRequest();
            while (!www.isDone)
            {
                var progress = www.downloadProgress;
                if (progressbar != null && progressbar(progress))
                {
                    done(null);
                    yield break;
                }
                yield return null;
            }

            if (progressbar != null && progressbar(1))
            {
                done(null);
                yield break;
            }

            var text = www.downloadHandler.text;

            if (text.StartsWith("<!"))
            {
                Debug.LogError("Google sheet could not be downloaded.\nURL:" + url + "\n" + text);
                done(null);
                yield break;
            }

            done(text);
        }
        
        private static void DownloadComplete(string text, ParsingFileAsset fileAsset)
        {
            if (string.IsNullOrEmpty(text))
            {
                Debug.LogError("Could not download google sheet");
                return;
            }
            
            var path = fileAsset.TextAsset != null ? AssetDatabase.GetAssetPath(fileAsset.TextAsset) : null;

            if (string.IsNullOrEmpty(path))
            {
                path = EditorUtility.SaveFilePanelInProject("Save parsing file", "", "txt", "Please enter a file name to save the csv to", path);
            }
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            File.WriteAllText(path, text);

            AssetDatabase.ImportAsset(path);
            
            fileAsset.TextAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(path);
            EditorUtility.SetDirty(fileAsset.TextAsset);
            AssetDatabase.SaveAssets();

            Downloading = false;
        }

        private static bool DisplayDownloadProgressbar(float progress)
        {
            if(progress < 1)
            {
                return EditorUtility.DisplayCancelableProgressBar("Download parsing files", "Downloading...", progress);
            }
        
            EditorUtility.ClearProgressBar();
            return false;
        }
    }
}
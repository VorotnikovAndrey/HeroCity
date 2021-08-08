using UnityEngine;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Localization
{
    public static class Lang
    {
        private const string PLAYER_PREFS_LAST_LANGUAGE_KEY = "last_lang";
        public const string SHEET_NAME_DIALOGUES = "Dialogues";

        public static event Action OnAbortSwitch;
        public static event Action OnSuccessSwitch;

        private static HashSet<ILocalizable> _localizables = new HashSet<ILocalizable>();

        //public static bool DenyAtlasLoading => false;

        public static LocalizationSettings Settings // => global::Get.LocalizationSettings;
        {
            get
            {
                if (_settings == null)
                    _settings = Resources.Load<LocalizationSettings>(LocalizationSettings
                        .SETTINGS_ASSET_RESOURCES_PATH);

                return _settings;
            }
        }

        private static LocalizationSettings _settings = null;

        public static List<LanguageCode> LanguageFilter = new List<LanguageCode>
        {
            LanguageCode.EN,
            LanguageCode.RU,
            LanguageCode.FR,
            LanguageCode.DE,
            LanguageCode.IT,
            LanguageCode.ES,
            //LanguageCode.ID,
            //LanguageCode.TR,
            //LanguageCode.NO,
            //LanguageCode.ZH_CN,
            //LanguageCode.ZH_TW,
            //LanguageCode.KO,
            //LanguageCode.JA,
            LanguageCode.PT_BR,
            //LanguageCode.AR,
            LanguageCode.PL,
        };

        public static Dictionary<string, Dictionary<string, string>> CurrEntrySheets { get; private set; }
        public static PluralisationForms CurrPlForm { get; private set; }
        private static LanguageCode _currLanguage = LanguageCode.N;

        public static LanguageCode CurrLanguage => _currLanguage;

        static Lang()
        {
            bool useSystemLanguagePerDefault = Settings.useSystemLanguagePerDefault;
            LanguageCode useLang = Settings.defaultLangCode;

            string lastLang = PlayerPrefs.GetString(PLAYER_PREFS_LAST_LANGUAGE_KEY, string.Empty);
            LanguageCode lastLangCode = GetLanguageEnum(lastLang);

#if UNITY_EDITOR
            if (!Application.isPlaying)
                SwitchLanguage(useLang);
#endif

            if (!string.IsNullOrEmpty(lastLang) && IsLanguageAvailable(lastLangCode))
            {
                SwitchLanguage(lastLang);
            }
            else
            {
                //See if we can use the local system language: if so, we overwrite useLang
                if (useSystemLanguagePerDefault)
                {
                    LanguageCode localLang = LocalizationUtils.LanguageNameToCode(Application.systemLanguage);
                    if (localLang == LanguageCode.N)
                    {
                        string langISO = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;

                        if (langISO != "iv") //IV = InvariantCulture
                            localLang = GetLanguageEnum(langISO);
                    }

                    if (IsLanguageAvailable(localLang))
                    {
                        useLang = localLang;
                    }
                    else
                    {
                        //?More attemps detect lang
                    }
                }

                SwitchLanguage(useLang);
            }
        }

        public static bool SwitchLanguage(string langCode)
        {
            return SwitchLanguage(GetLanguageEnum(langCode));
        }

        public static bool SwitchLanguage(LanguageCode code, bool ignoreCurrent = false)
        {
            if (CurrLanguage == code && !ignoreCurrent)
                return false;

            if (IsLanguageAvailable(code))
            {
                DoSwitch(code);
                return true;
            }
            else
            {
#if DEBUG_MODE
            Debug.LogError("Could not switch from " + CurrLanguage + " to " + code);
#endif
                if (CurrLanguage == LanguageCode.N)
                {
                    DoSwitch(LanguageFilter[0]);
#if DEBUG_MODE
                Debug.LogError("Switched to " + CurrLanguage + " language");
#endif
                }

                return false;
            }

        }

        private static LanguageCode GetLanguageEnum(string langCode)
        {
            langCode = langCode.ToUpper();

            foreach (LanguageCode item in Enum.GetValues(typeof(LanguageCode)))
            {
                if (item.ToString() == langCode)
                    return item;
            }

#if DEBUG_MODE
        Debug.LogErrorFormat("[GetLanguageEnum]: Missing language enum: {0}", langCode);
#endif

            return LanguageCode.EN;
        }

        private static bool IsLanguageAvailable(LanguageCode code)
        {
            return LanguageFilter.Contains(code);
        }

        private static void DoSwitch(LanguageCode newLang)
        {
            Debug.LogFormat("Language.DoSwitch: {0}", newLang);

            if (IsNeedSwitchAdditive(newLang))
                DoSwitchAdditive(newLang);
            else
                DoSwitchLanguage(newLang);
        }

        private static void DoSwitchLanguage(LanguageCode newLang)
        {
            PlayerPrefs.SetString(PLAYER_PREFS_LAST_LANGUAGE_KEY, newLang.ToString());

            _currLanguage = newLang;
            CurrEntrySheets = new Dictionary<string, Dictionary<string, string>>();

            foreach (string sheetTitle in Settings.sheetTitles)
            {
                var asset = GetLanguageFileAsset(sheetTitle);

                if (asset != null)
                    CurrEntrySheets[sheetTitle] = asset.Data.ToDictionary(x => x.key, y => y.value);
            }

            CurrPlForm = new PluralisationForms();

            OnLanguageSwitch();
            if (OnSuccessSwitch != null)
                OnSuccessSwitch.Invoke();
#if DEBUG_MODE
        Debug.LogFormat("Language.DoSwitch: {0}", CurrLanguage);
#endif
        }

        public static void OnLanguageSwitch()
        {
            foreach (var localizable in _localizables)
                localizable?.Localize();
            //var components = FindLocalizeComponents();
            //if (components != null)
            //{
            //    for (int i = 0, l = components.Length; i < l; i++)
            //    {
            //        components[i].OnLanguageSwitch();
            //    }
            //}
        }

        public static void AddLocalizable(ILocalizable localizable)
        {
            _localizables.Add(localizable);
            localizable.Localize();
        }
        public static void RemoveLocalizable(ILocalizable localizable)
        {
            _localizables.Remove(localizable);
        }

        private static void DoSwitchAdditive(LanguageCode newLang, bool withoutPopup = false)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying) return;
#endif
            DoSwitchLanguage(newLang);
        }

        private static void CloseServiceDownloadPopup()
        {

        }

        private static bool CanOpenServiceDownloadPopup()
        {
            return false;
        }


        private static void CheckInternetConnection(Action<bool> callback)
        {

        }

        private static void LoadFontResources(LanguageCode langCode)
        {

        }

        private static bool IsNeedSwitchAdditive(LanguageCode newLang)
        {
            //return FontTexturesLoader.NeedToLoadAtlasses(newLang) && !DenyAtlasLoading;
            return false;
        }

        private static void OnCompleteLoadAdditiveLanguage(LanguageCode newLang)
        {
            LoadFontResources(newLang);
            DoSwitchLanguage(newLang);
        }

        //private static void OnServiceDownloadPopupCallback(PopupCallbackParameters res)
        //{
        //    if (res.CallbackType == PopupCallbackType.Denied)
        //    {
        //        AssetBundlesLoader.Instance.AbortAssetBundle(Constants.ADDITIVE_FONTS_BUNDLE_NAME);
        //        OnAbortSwitch?.Invoke();
        //    }
        //}

        private static void BlockLanguageChanges(bool active)
        {
            //TODO: Get and lock popup with language selector
        }
        
        private static LanguageAsset GetLanguageFileAsset(string sheetName)
        {
            
            return Resources.Load<LanguageAsset>(string.Format(LocalizationSettings.SHEET_ASSET_RES_PATH_FORMAT,
                CurrLanguage, sheetName));
        }
        
        public static string Get(string key, ESheet sheet, bool returnNullWhenNotFound = false)
        {
            return Get(key, sheet.ToString(), returnNullWhenNotFound);
        }

        public static string Get(string key, string sheetTitle, bool returnNullWhenNotFound = false)
        {
            if (Has(key, sheetTitle))
            {
                return (CurrEntrySheets[sheetTitle])[key];
            }

            if (Has(key, Settings.predefSheetTitle))
            {
                return CurrEntrySheets[Settings.predefSheetTitle][key];
            }
            else
            {
                return returnNullWhenNotFound ? null : "#!#" + key + "#!#";
            }
        }

        public static bool Has(string key)
        {
            return Has(key, Settings.sheetTitles[0]);
        }

        public static bool Has(string key, string sheetTitle)
        {
            if (CurrEntrySheets == null || !CurrEntrySheets.ContainsKey(sheetTitle)) return false;
            return CurrEntrySheets[sheetTitle].ContainsKey(key);
        }

    }

    public static class StringExtensions
    {
        public static string UnescapeXML(this string s)
        {
            if (string.IsNullOrEmpty(s)) return s;

            string returnString = s;
            returnString = returnString.Replace("&apos;", "'");
            returnString = returnString.Replace("&quot;", "\"");
            returnString = returnString.Replace("&gt;", ">");
            returnString = returnString.Replace("&lt;", "<");
            returnString = returnString.Replace("&amp;", "&");

            return returnString;
        }
    }
}
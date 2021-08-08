using UnityEngine;

namespace Localization
{
    [System.Serializable]
    public class LocalizationSettings : ScriptableObject
    {
        public const string ADDITIVE_FONTS_BUNDLE_NAME = "add_fonts";
        public const string SHEET_ASSET_RES_PATH_FORMAT = "Localization/{0}_{1}";

        public const string SETTINGS_ASSET_BUNDLE_NAME = "settings";
        public const string SETTINGS_ASSET_PATH = "Assets/Localization/Resources/Languages/LocalizationSettings.asset";
        public const string SETTINGS_ASSET_RESOURCES_PATH = "Languages/LocalizationSettings";

        public const string BUNDLE_PATH = "Assets/Resources/Localization/";
        public const string PREDEF_PATH = "Assets/Localization/Resources/Languages/";

        public string[] sheetTitles;
 
        public bool useSystemLanguagePerDefault = true;
        public LanguageCode defaultLangCode = LanguageCode.EN;

        public string predefSheetTitle = "Predef";

        public LanguageCode[] availableLanguages;
    }
}
using System.Text.RegularExpressions;
using UnityEngine;

namespace Localization
{
    public static class LocalizationUtils
    {
        private const string ARABIC_SYMBOLS = "\\p{IsArabic}"
                                              //+ "\u0600-\u06FF"
                                              + "\u0750-\u077F"
                                              + "\u08A0-\u08FF"
                                              + "\uFB50-\uFDFF"
                                              + "\uFE70-\uFEFF";

        private static Regex _isArabicSymbol = new Regex("[" + ARABIC_SYMBOLS + "]");
        private static Regex _arabicReverseRegex = new Regex("[a-zA-Z0-9а-яА-Я&]{2,}");
        
        public static string GetPluralized(string key, string sheetTitle, int numeral)
        {
            return Lang.Get(Lang.CurrPlForm.GetPluralizedKey(key, numeral.ToString()), sheetTitle);
        }

        public static bool IsCurrentLanguageArabic()
        {
            return IsArabicLanguage(Lang.CurrLanguage);
        }

        public static bool IsArabicLanguage(LanguageCode langCode)
        {
            return false; //langCode == LanguageCode.AR;
        }

        public static bool IsArabicLanguage(string langCode)
        {
            return "AR".Equals(langCode);
        }

        //public static string ReverseArabicString(string s)
        //{
        //    char[] arr = s.ToCharArray();
        //    System.Array.Reverse(arr); //TODO: use for loop
        //    return new string(arr);
        //}

        public static string FixArabicString(string s)
        {
            if (IsArabic(s))
            {
                s = UnescapeXMLSpecialCharacters(s);
                s = ReverseArabicString(s);
                //s = ArabicSupport.ArabicFixer.Fix(s, true, true);
                s = EscapeXMLSpecialCharacters(s);
            }
            return s;
        }

        public static string UnescapeXMLSpecialCharacters(string s)
        {
            return s.Replace("&amp;", "&")
                .Replace("&lt;", "<")
                .Replace("&gt;", ">")
                .Replace("&quot;", "\"")
                .Replace("&apos;", "'");
        }

        public static string EscapeXMLSpecialCharacters(string s)
        {
            return s.Replace("&", "&amp;")
                .Replace("<", "&lt;")
                .Replace(">", "&gt;")
                .Replace("\"", "&quot;")
                .Replace("'", "&apos;");
        }

        public static bool IsArabic(string strCompare)
        {
            return _isArabicSymbol.IsMatch(strCompare);
        }

        public static string ReverseArabicString(string s)
        {
            char[] arr = s.ToCharArray();

            var matches = _arabicReverseRegex.Matches(s);
            if (matches.Count > 0)
            {
                for (int x = 0; x < matches.Count; x++)
                {
                    string matchValue = matches[x].Value;
                    for (int y = 0; y < matchValue.Length; y++)
                    {
                        arr[matches[x].Index + y] = matchValue[matchValue.Length - y - 1];
                    }
                }
            }

            //System.Array.Reverse(arr);
            return new string(arr);
        }

        public static LanguageCode LanguageNameToCode(SystemLanguage name)
        {
            if (name == SystemLanguage.Unknown) return LanguageCode.N;
            //else if (name == SystemLanguage.Afrikaans) return LanguageCode.AF;
            //else if (name == SystemLanguage.Arabic) return LanguageCode.AR;
            //else if (name == SystemLanguage.Basque) return LanguageCode.BA;
            //else if (name == SystemLanguage.Belarusian) return LanguageCode.BE;
            //else if (name == SystemLanguage.Bulgarian) return LanguageCode.BG;
            //else if (name == SystemLanguage.Catalan) return LanguageCode.CA;
            //else if (name == SystemLanguage.Chinese) return LanguageCode.ZH;
            //else if (name == SystemLanguage.Czech) return LanguageCode.CS;
            //else if (name == SystemLanguage.Danish) return LanguageCode.DA;
            //else if (name == SystemLanguage.Dutch) return LanguageCode.NL;
            else if (name == SystemLanguage.English) return LanguageCode.EN;
            //else if (name == SystemLanguage.Estonian) return LanguageCode.ET;
            //else if (name == SystemLanguage.Faroese) return LanguageCode.FA;
            //else if (name == SystemLanguage.Finnish) return LanguageCode.FI;
            else if (name == SystemLanguage.French) return LanguageCode.FR;
            else if (name == SystemLanguage.German) return LanguageCode.DE;
            //else if (name == SystemLanguage.Greek) return LanguageCode.EL;
            //else if (name == SystemLanguage.Hebrew) return LanguageCode.HE;
            //else if (name == SystemLanguage.Hungarian) return LanguageCode.HU;
            //else if (name == SystemLanguage.Icelandic) return LanguageCode.IS;
            //else if (name == SystemLanguage.Indonesian) return LanguageCode.ID;
            else if (name == SystemLanguage.Italian) return LanguageCode.IT;
//            else if (name == SystemLanguage.Japanese) return LanguageCode.JA;
//            else if (name == SystemLanguage.Korean) return LanguageCode.KO;
            //else if (name == SystemLanguage.Latvian) return LanguageCode.LA;
            //else if (name == SystemLanguage.Lithuanian) return LanguageCode.LT;
            //else if (name == SystemLanguage.Norwegian) return LanguageCode.NO;
            else if (name == SystemLanguage.Polish) return LanguageCode.PL;
            else if (name == SystemLanguage.Portuguese) return LanguageCode.PT_BR;
            //else if (name == SystemLanguage.Romanian) return LanguageCode.RO;
            else if (name == SystemLanguage.Russian) return LanguageCode.RU;
            //else if (name == SystemLanguage.SerboCroatian) return LanguageCode.SH;
            //else if (name == SystemLanguage.Slovak) return LanguageCode.SK;
            //else if (name == SystemLanguage.Slovenian) return LanguageCode.SL;
           else if (name == SystemLanguage.Spanish) return LanguageCode.ES;
            //else if (name == SystemLanguage.Swedish) return LanguageCode.SW;
            //else if (name == SystemLanguage.Thai) return LanguageCode.TH;
            //else if (name == SystemLanguage.Turkish) return LanguageCode.TR;
            //else if (name == SystemLanguage.Ukrainian) return LanguageCode.UK;
            //else if (name == SystemLanguage.Vietnamese) return LanguageCode.VI;
            //else if (name == SystemLanguage.Hungarian) return LanguageCode.HU;        
            //else if (name == SystemLanguage.ChineseSimplified) return LanguageCode.ZH_CN;
            //else if (name == SystemLanguage.ChineseTraditional) return LanguageCode.ZH_TW;
            return LanguageCode.N;
        }
    }
}
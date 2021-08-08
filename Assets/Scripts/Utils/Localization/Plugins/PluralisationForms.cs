using System.Text.RegularExpressions;

namespace Localization
{
    public class PluralisationForms
    {
        const string LOCALIZATION_SERVICE_KEY_PLURAL_FORMS = "plural_forms";

        private Regex[] _regexs;

        public PluralisationForms()
        {
            var patterns = Lang.Get (LOCALIZATION_SERVICE_KEY_PLURAL_FORMS, "common").Split(';');
            _regexs = new Regex[patterns.Length];
            for(int x = 0 ; x < patterns.Length ; x++)
            {
                _regexs[x] = new Regex(patterns[x]);
            }
        }

        public string GetPluralizedKey(string key, string number)
        {
            for(int x = 0 ; x < _regexs.Length ; x++)
            {
                if (_regexs[x].IsMatch(number))
                {
                    return string.Format("{0}.pf{1}", key, x);
                }
            }
            return string.Format("{0}.pf{1}", key, _regexs.Length);
        }

    }
}
using TMPro;
using UnityEngine;

namespace Localization
{
    public class LocalizedText : MonoBehaviour, ILocalizable
    {
        public TMP_Text Text;
        public string Key;
        public ESheet Sheet = ESheet.Interface;

        public void Localize()
        {
            Text.text = Lang.Get(Key, Sheet.ToString());
        }

        void Awake()
        {
            Lang.AddLocalizable(this);
        }

        void OnDestroy()
        {
            Lang.RemoveLocalizable(this);
        }

        void Reset()
        {
            Text = GetComponent<TMP_Text>();
        }
    }
}
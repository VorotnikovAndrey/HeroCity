using TMPro;
using UnityEngine;
using Utils.ObjectPool;

namespace UI
{
    public class StatsInfoElement : AbstractBaseViewUI
    {
        [SerializeField] private TextMeshProUGUI _title;
        [SerializeField] private TextMeshProUGUI _value;

        public void SetTitle(string value)
        {
            _title.text = value;
        }

        public void SetValue(string value)
        {
            _value.text = value;
        }

        private void SetColor(Color color)
        {
            _title.color = color;
            _value.color = color;
        }
    }
}
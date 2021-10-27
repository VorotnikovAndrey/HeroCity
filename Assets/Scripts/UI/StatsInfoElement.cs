using System;
using TMPro;
using UnityEngine;
using Utils.ObjectPool;

namespace UI
{
    public class StatsInfoElement : AbstractBaseViewUI
    {
        [SerializeField] private TextMeshProUGUI _title;
        [SerializeField] private TextMeshProUGUI _value;

        public void SetKey(string value)
        {
            _title.text = value;
        }

        public void SetValue(string value, bool format = false)
        {
            _value.text = format ? FormatValue(value) : value;
        }

        private string FormatValue(string value)
        {
            float result = Convert.ToSingle(value);

            return result > 0 ? $"+{result}" : result.ToString();
        }
    }
}
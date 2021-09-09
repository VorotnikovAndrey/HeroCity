using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Building
{
    public class BuildingUpgradeBar : MonoBehaviour
    {
        [SerializeField] private GameObject Holder;
        [SerializeField] private TextMeshProUGUI _timerText;
        [SerializeField] private Image _fillImage;

        public void SetValue(string time, float progress01)
        {
            _timerText.text = time;
            _fillImage.fillAmount = Mathf.Clamp01(progress01);
        }

        public void Show()
        {
            Holder.SetActive(true);
        }

        public void Hide()
        {
            Holder.SetActive(false);
        }
    }
}
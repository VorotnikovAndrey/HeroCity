using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Popups.Components
{
    public class UpgradeBar : MonoBehaviour
    {
        [SerializeField] private GameObject Holder;
        [SerializeField] private TextMeshProUGUI _timerText;
        [SerializeField] private Image _fillImage;

        private long _startUnixTime;
        private long _endUnixTime;
        private Tweener _tweener;
        private float _progress;
        private Ease _animationEase = Ease.Linear;

        public float Progress
        {
            get => _progress;
            private set
            {
                _progress = value;
                _fillImage.fillAmount = value;
            }
        }

        private void OnEnable()
        {
            Progress = 0;
            GameManager.Instance.TimeTicker.OnSecondTick += UpdateText;
        }

        private void OnDisable()
        {
            _tweener?.Kill();

            if (GameManager.Instance != null)
            {
                GameManager.Instance.TimeTicker.OnSecondTick -= UpdateText;
            }
        }

        public void Initialize(long startUnixTime, long endUnixTime)
        {
            _startUnixTime = startUnixTime;
            _endUnixTime = endUnixTime;

            Holder.SetActive(true);

            UpdateText();
            UpdateAnimation();
        }

        private void UpdateText()
        {
            if (DateTimeUtils.GetCurrentTime() > _endUnixTime)
            {
                Holder.SetActive(false);
                return;
            }

            _timerText.text = DateTimeUtils.GetTimerText(DateTimeUtils.UnixTimeToDateTime(_endUnixTime - DateTimeUtils.GetCurrentTime()));
        }

        private void UpdateAnimation()
        {
            var timeLeft = _endUnixTime - DateTimeUtils.GetCurrentTime();

            _tweener?.Kill();
            _tweener = DOTween.To(() => Progress, x => Progress = x, 1f, timeLeft).SetEase(_animationEase).OnKill(() =>
            {
                _tweener = null;
            });
        }
    }
}
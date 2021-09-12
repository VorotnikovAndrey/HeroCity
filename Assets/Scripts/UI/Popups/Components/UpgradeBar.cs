using DG.Tweening;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Popups.Components
{
    public class UpgradeBar : MonoBehaviour
    {
        [SerializeField] private GameObject Holder;
        [SerializeField] private TextMeshProUGUI _timerText;
        [SerializeField] private Image _fillImage;
        [SerializeField] private bool _useAnimation;
        [SerializeField] private Ease _animationEase = Ease.Unset;
        [SerializeField] [Range(0, 1)] private float _animationDuration = 1f;

        private long _startUnixTime;
        private long _endUnixTime;
        private Tweener _tweener;
        private float _progress;

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
            GameManager.Instance.TimeTicker.OnSecondTick += UpdateInfo;
        }

        private void OnDisable()
        {
            _tweener?.Kill();

            if (GameManager.Instance != null)
            {
                GameManager.Instance.TimeTicker.OnSecondTick -= UpdateInfo;
            }
        }

        public void Initialize(long startUnixTime, long endUnixTime)
        {
            _startUnixTime = startUnixTime;
            _endUnixTime = endUnixTime;

            Holder.SetActive(true);

            UpdateInfo();
        }

        private void UpdateInfo()
        {
            if (DateTimeUtils.GetCurrentTime() >= _endUnixTime)
            {
                Holder.SetActive(false);
                return;
            }

            var currentUnixTime = DateTimeUtils.GetCurrentTime();

            _timerText.text = DateTimeUtils.GetTimerText(DateTimeUtils.UnixTimeToDateTime(_endUnixTime - currentUnixTime));

            var totalTime = _endUnixTime - _startUnixTime;
            var timeLeft = _endUnixTime - currentUnixTime;
            var progress = 1 - (float)timeLeft / totalTime;

            if (_useAnimation)
            {
                _tweener?.Kill();
                _tweener = DOTween.To(() => Progress, x => Progress = x, progress, _animationDuration).SetEase(_animationEase).OnKill(() =>
                {
                    _tweener = null;
                });
            }
            else
            {
                _fillImage.fillAmount = progress;
            }
        }
    }
}
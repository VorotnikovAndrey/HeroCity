using System.Linq;
using Content;
using Economies;
using PopupSystem;
using UnityEngine;
using UnityEngine.UI;
using Utils.ObjectPool;

namespace Gameplay.Improvements
{
    public class ImprovementButton : AbstractBaseViewUI
    {
        [SerializeField] private Image _iconImage;

        private ImprovementData _improvementData;

        public override void Initialize(object args)
        {
            ImprovementData data = ContentProvider.Economies.ImprovementEconomy.Data.FirstOrDefault(x => x.Id == args.ToString());
            if (data == null)
            {
                return;
            }

            _improvementData = data;

            var sprite = ContentProvider.Graphic.SpriteBank.GetSprite(_improvementData.SpriteBankId);
            if (sprite != null)
            {
                _iconImage.sprite = sprite;
            }
        }

        public void OnButtonPressed()
        {
            GameManager.Instance.PopupManager.ShowPopup(PopupType.Improvement, _improvementData);
        }
    }
}
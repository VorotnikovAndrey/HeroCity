using System.Linq;
using Content;
using Economies;
using ResourceSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UserSystem;
using Utils;
using Utils.ObjectPool;
using Zenject;

namespace UI.Popups.Components
{
    public class ResourceRequiredContainer : AbstractBaseViewUI
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _title;
        [SerializeField] private TextMeshProUGUI _count;
        [Space]
        [SerializeField] private Color _requiredColor;
        [SerializeField] private Color _nonRequiredColor;

        private UserManager _userManager;
        private SpriteBank _spriteBank;

        private void Awake()
        {
            _userManager = ProjectContext.Instance.Container.Resolve<UserManager>();
            _spriteBank = ContentProvider.Graphic.SpriteBank;
        }

        public void SetIcon(Sprite sprite)
        {
            _icon.sprite = sprite;
        }

        public void SetTitle(string title)
        {
            _title.text = title;
        }

        public void SetCount(ResourceType type, int count)
        {
            var currentValue = _userManager.CurrentUser.Resources[type];

            _count.color = currentValue >= count ? _requiredColor : _nonRequiredColor;
            _count.text = $"{currentValue}/{count}";
        }

        public void SetResourcesData(ResourcesData resource)
        {
            SetTitle(resource.Type.ToString());
            SetIcon(_spriteBank.ResourceIcons.FirstOrDefault(x => x.Type == resource.Type)?.Sprite);
            SetCount(resource.Type, resource.Value);
        }
    }
}
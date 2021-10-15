using System.Collections.Generic;
using System.Linq;
using Content;
using Events;
using Gameplay.Characters;
using PopupSystem;
using UI.Popups.Components;
using UnityEngine;
using Utils;
using Utils.PopupSystem;
using Zenject;

namespace UI.Popups
{
    public class CharacterPopup : AbstractPopupBase<PopupType>
    {
        public override PopupType Type => PopupType.Character;

        [SerializeField] private List<CharacterPopupPageElement> _pages;

        private BaseCharacterView _characterView;
        private CharacterRawModelHolder _rawModelHolder;

        private void OnValidate()
        {
            _pages = transform.GetComponentsInChildren<CharacterPopupPageElement>(true).ToList();
        }

        protected override void OnShow(object args = null)
        {
            _characterView = args as BaseCharacterView;

            if (_characterView == null)
            {
                Debug.LogError("View is null".AddColorTag(Color.red));
                return;
            }

            foreach (var page in _pages)
            {
                page.SetModel(_characterView.Model);
            }

            SetPage(0);
            SpawnRawDummy();
        }

        protected override void OnHide()
        {
            foreach (var page in _pages)
            {
                page.DropData();
            }

            EventAggregator.SendEvent(new CharacterViewUnSelectedEvent
            {
                ReturnToPrevPos = true
            });

            _rawModelHolder.SetState(false);

            ReleaseRawDummy();
        }

        public void SetPage(int index)
        {
            foreach (var page in _pages)
            {
                if (page.Index == index)
                {
                    page.Show();
                }
                else
                {
                    page.Hide();
                }
            }
        }

        private void SpawnRawDummy()
        {
            _rawModelHolder = ProjectContext.Instance.Container.Resolve<CharacterRawModelHolder>();
            _rawModelHolder.SetState(true);

            CharacterGraphicPresetPair data = ContentProvider.Graphic.CharacterGraphicPreset.Get(_characterView.Model.GraphicPresetId, _characterView.Model.CharacterType);
            var dummy = Instantiate(data.Object, _rawModelHolder.Holder);

            // TODO: Equip Dummy

            _rawModelHolder.Show(dummy);
        }

        private void ReleaseRawDummy()
        {
            _rawModelHolder.Hide();
        }
    }
}
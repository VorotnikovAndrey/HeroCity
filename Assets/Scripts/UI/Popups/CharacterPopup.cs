using System.Collections.Generic;
using System.Linq;
using CameraSystem;
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
        [SerializeField] private GameObject _switchButtonsHolder;

        private BaseCharacterView _characterView;
        private CharacterRawModelHolder _rawModelHolder;
        private CharactersSystem _charactersSystem;
        private LocationCamera _locationCamera;

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

            _charactersSystem = ProjectContext.Instance.Container.Resolve<CharactersSystem>();
            _locationCamera = ProjectContext.Instance.Container.Resolve<CameraManager>().ActiveCameraView as LocationCamera;
            _rawModelHolder = ProjectContext.Instance.Container.Resolve<CharacterRawModelHolder>();

            foreach (var page in _pages)
            {
                page.SetModel(_characterView.Model);
            }

            SetPage(0);
            SpawnRawDummy();

            _switchButtonsHolder.gameObject.SetActive(_charactersSystem.Components[_characterView.Model.CharacterType].Characters.Count > 1);
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

        public void SwitchCharacter(int shift)
        {
            var array = _charactersSystem.Components[_characterView.Model.CharacterType].Characters.Values.ToList();
            int index = array.IndexOf(_characterView);

            index += shift;
            if (index >= array.Count)
            {
                index = 0;
            }
            else if (index < 0)
            {
                index = array.Count - 1;
            }

            _characterView = array[index];
            _locationCamera.SwitchToFollow(_characterView.Transform);

            foreach (var page in _pages)
            {
                page.SetModel(_characterView.Model);
            }

            ReleaseRawDummy();
            SpawnRawDummy();
        }
    }
}
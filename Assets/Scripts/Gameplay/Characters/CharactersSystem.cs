using System.Collections.Generic;
using CameraSystem;
using Events;
using Gameplay.Characters.Components;
using Gameplay.Equipments;
using PopupSystem;
using UnityEngine;
using UserSystem;
using Utils;
using Utils.Events;
using Utils.PopupSystem;
using Zenject;

namespace Gameplay.Characters
{
    public class CharactersSystem
    {
        public readonly Dictionary<CharacterType, BaseCharacterComponent> Components;

        private UserManager _userManager;
        private PopupManager<PopupType> _popupManager;
        private EventAggregator _eventAggregator;
        private TimeTicker _timeTicker;
        private LocationCamera _locationCamera;

        public CharactersSystem()
        {
            Components = new Dictionary<CharacterType, BaseCharacterComponent>
            {
                {CharacterType.Default, new BaseCharacterComponent()},
                {CharacterType.Hero, new HeroCharacterComponent()},
                {CharacterType.Enemy, new EnemyCharacterComponent()},
                {CharacterType.Citizen, new CitizenCharacterComponent()},
                {CharacterType.Employee, new EmployeeCharacterComponent()},
            };
        }

        public virtual void Initialize()
        {
            ProjectContext.Instance.Container.BindInstances(this);

            _timeTicker = ProjectContext.Instance.Container.Resolve<TimeTicker>();
            _eventAggregator = ProjectContext.Instance.Container.Resolve<EventAggregator>();
            _userManager = ProjectContext.Instance.Container.Resolve<UserManager>();
            _locationCamera = ProjectContext.Instance.Container.Resolve<CameraManager>().ActiveCameraView as LocationCamera;
            _popupManager = ProjectContext.Instance.Container.Resolve<PopupManager<PopupType>>();

            foreach (var component in Components)
            {
                component.Value.Initialize();
            }

            _timeTicker.OnTick += Update;

            _eventAggregator.Add<CreateCharacterEvent>(CreateCharacter);
            _eventAggregator.Add<ReleaseCharacterEvent>(ReleaseCharacter);
            _eventAggregator.Add<CharacterViewSelectedEvent>(OnCharacterViewSelected);
            _eventAggregator.Add<CharacterViewUnSelectedEvent>(OnCharacterViewUnSelected);

            LoadCharacters();
        }

        public virtual void DeInitialize()
        {
            _eventAggregator.Remove<CreateCharacterEvent>(CreateCharacter);
            _eventAggregator.Remove<ReleaseCharacterEvent>(ReleaseCharacter);
            _eventAggregator.Remove<CharacterViewSelectedEvent>(OnCharacterViewSelected);
            _eventAggregator.Remove<CharacterViewUnSelectedEvent>(OnCharacterViewUnSelected);

            _timeTicker.OnTick -= Update;

            foreach (var component in Components)
            {
                component.Value.DeInitialize();
            }

            ProjectContext.Instance.Container.Unbind<CharactersSystem>();
        }

        private void Update()
        {
            foreach (var component in Components)
            {
                component.Value.Update();
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                ApplyDebug();
            }
        }

        private void LoadCharacters()
        {
            foreach (var model in _userManager.CurrentUser.Characters)
            {
                _eventAggregator.SendEvent(new CreateCharacterEvent
                {
                    Model = model
                });
            }
        }

        private void CreateCharacter(CreateCharacterEvent sender)
        {
            if (sender.Model == null)
            {
                Debug.LogError("Model is null".AddColorTag(Color.red));
                return;
            }

            Components[sender.Model.CharacterType].Add(sender.Model);
        }

        private void ReleaseCharacter(ReleaseCharacterEvent sender)
        {
            if (sender.Model == null)
            {
                Debug.LogError("Model is null".AddColorTag(Color.red));
                return;
            }

            Components[sender.Model.CharacterType].Remove(sender.Model);
        }

        private void OnCharacterViewSelected(CharacterViewSelectedEvent sender)
        {
            if (_locationCamera.CameraState != CameraStates.Default)
            {
                return;
            }

            _locationCamera.SwitchToFollow(sender.View.Transform);
            _popupManager.ShowPopup(PopupType.Character, sender.View);
        }

        private void OnCharacterViewUnSelected(CharacterViewUnSelectedEvent sender)
        {
            _locationCamera.SwitchToDefaultState(sender.ReturnToPrevPos);
        }

        private void ApplyDebug()
        {
            var model = CharacterFactory.Get(CharacterType.Hero, (Gender)Random.Range(0, 2), (Rarity)Random.Range(0, 4));

            _eventAggregator.SendEvent(new CreateCharacterEvent
            {
                Model = model
            });

            _userManager.CurrentUser.Characters.Add(model);
            _userManager.Save(true);
        }
    }
}

using System.Collections.Generic;
using CameraSystem;
using Events;
using Gameplay.Characters.Components;
using Gameplay.Characters.Models;
using Gameplay.Movement;
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
        private readonly Dictionary<CharacterType, BaseCharacterComponent> _components;

        private UserManager _userManager;
        private PopupManager<PopupType> _popupManager;
        private EventAggregator _eventAggregator;
        private TimeTicker _timeTicker;
        private LocationCamera _locationCamera;

        public CharactersSystem()
        {
            _components = new Dictionary<CharacterType, BaseCharacterComponent>
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
            _timeTicker = ProjectContext.Instance.Container.Resolve<TimeTicker>();
            _eventAggregator = ProjectContext.Instance.Container.Resolve<EventAggregator>();
            _userManager = ProjectContext.Instance.Container.Resolve<UserManager>();
            _locationCamera = ProjectContext.Instance.Container.Resolve<CameraManager>().ActiveCameraView as LocationCamera;
            _popupManager = ProjectContext.Instance.Container.Resolve<PopupManager<PopupType>>();

            foreach (var component in _components)
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

            foreach (var component in _components)
            {
                component.Value.DeInitialize();
            }
        }

        private void Update()
        {
            foreach (var component in _components)
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

            _components[sender.Model.CharacterType].Add(sender.Model);
        }

        private void ReleaseCharacter(ReleaseCharacterEvent sender)
        {
            if (sender.Model == null)
            {
                Debug.LogError("Model is null".AddColorTag(Color.red));
                return;
            }

            _components[sender.Model.CharacterType].Remove(sender.Model);
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
            var model = new HeroModel
            {
                CharacterType = CharacterType.Hero,
                Movement = new WaypointMovement(),
                Stats = new Stats()
            };

            _eventAggregator.SendEvent(new CreateCharacterEvent
            {
                Model = model
            });

            _userManager.CurrentUser.Characters.Add(model);
            _userManager.Save(true);
        }
    }
}

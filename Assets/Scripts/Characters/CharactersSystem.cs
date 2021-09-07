using System.Collections.Generic;
using System.Linq;
using Asyncoroutine;
using Characters.Components;
using Characters.Models;
using Content;
using Defong.Utils;
using Events;
using Gameplay.Locations.View;
using Gameplay.Movement;
using Source;
using UnityEngine;
using UserSystem;
using Utils.Events;
using Utils.ObjectPool;
using Utils.Pathfinding;
using Zenject;

namespace Characters
{
    public class CharactersSystem
    {
        private EventAggregator _eventAggregator;
        private UserManager _userManager;
        private TimeTicker _timeTicker;
        private LocationView _locationView;

        private Dictionary<CharacterType, BaseCharacterComponent> _components;
        private List<BaseCharacterModel> _characters;

        // TODO:
        private static readonly int Move = Animator.StringToHash("Move");

        public virtual void Initialize()
        {
            _locationView = ProjectContext.Instance.Container.Resolve<LocationView>();
            _userManager = ProjectContext.Instance.Container.Resolve<UserManager>();
            _timeTicker = ProjectContext.Instance.Container.Resolve<TimeTicker>();
            _eventAggregator = ProjectContext.Instance.Container.Resolve<EventAggregator>();

            _components = new Dictionary<CharacterType, BaseCharacterComponent>
            {
                {CharacterType.Default, new BaseCharacterComponent()},
                {CharacterType.Hero, new BaseCharacterComponent()},
                {CharacterType.Citizen, new BaseCharacterComponent()},
                {CharacterType.Employee, new BaseCharacterComponent()},
                {CharacterType.Enemy, new BaseCharacterComponent()}
            };

            _characters = new List<BaseCharacterModel>();

            _eventAggregator.Add<CharacterLeavedFromLocationEvent>(OnCharacterLeavedFromLocation);

            foreach (var component in _components.Values)
            {
                component.Initialize();
            }

            _timeTicker.OnTick += Update;

            PlayDebug();
        }

        public virtual void DeInitialize()
        {
            _timeTicker.OnTick -= Update;

            _eventAggregator.Remove<CharacterLeavedFromLocationEvent>(OnCharacterLeavedFromLocation);

            foreach (var component in _components.Values)
            {
                component.DeInitialize();
            }

            for (int i = 0; i < _characters.Count; i++)
            {
                _characters[i].View.DestroyAndRemoveFromPool();
                _characters[i].Dispose();
            }

            _characters.Clear();
        }

        private void Update()
        {
            for (int i = 0; i < _characters.Count; i++)
            {
                var element = _characters[i];
                if (element == null)
                {
                    continue;
                }

                _components[element.CharacterType].Update(element);
            }
        }

        public BaseCharacterModel CreateModel(CharacterType type)
        {
            BaseCharacterModel model = new BaseCharacterModel
            {
                CharacterType = type,
                GraphicPresetId = ContentProvider.CharacterGraphicPreset.Data.GetRandom().Id,
                AI = ContentProvider.BehaviorsData.Containers.GetRandom().Elements.Select(x => x.GetClone()).ToList(),
                Stats = new Stats
                {
                    HeathPoint = 100,
                    MovementSpeed = 2f
                }
            };

            return model;
        }

        public void CreateAndSpawn(BaseCharacterModel model)
        {
            CharacterGraphicPresetPair preset = ContentProvider.CharacterGraphicPreset.Data.FirstOrDefault(x => x.Id == model.GraphicPresetId);

            if (preset == null)
            {
                Debug.LogError($"Graphic {model.GraphicPresetId.AddColorTag(Color.yellow)} is not found!".AddColorTag(Color.red));
                return;
            }

            var startPosition = _locationView.WaypointsContainer.GetTypePositions(MapWaypointType.Enter).GetRandom().Position;

            BaseCharacterView view = ViewGenerator.GetOrCreateItemView<BaseCharacterView>(GameConstants.View.DefaultCharacterPath, true, new ViewCreateParams
            {
                Position = startPosition
            });

            model.View = view;
            view.SetGraphic(preset);
            model.Movement = new Movement(view.Transform, startPosition, model.Stats.MovementSpeed);
            model.Movement.IsMoving.AddListener(x => view.Animator.SetBool(Move, x));
            model.ApplyAiSequention(model);

            _characters.Add(model);
        }

        public void ReleaseCharacter(BaseCharacterModel model)
        {
            if (!_characters.Contains(model))
            {
                Debug.LogError("This model has view".AddColorTag(Color.red));
                return;
            }

            model.View.ReleaseItemView();
            _characters.Remove(model);
            model.Dispose();
        }

        private void OnCharacterLeavedFromLocation(CharacterLeavedFromLocationEvent sender)
        {
            ReleaseCharacter(sender.Model);
        }

        private async void PlayDebug()
        {
            for (int i = 0; i < 20; i++)
            {
                CreateAndSpawn(CreateModel(CharacterType.Default));
                await new WaitForSeconds(5f);
            }
        }
    }
}

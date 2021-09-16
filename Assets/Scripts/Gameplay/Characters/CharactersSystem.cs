using System.Collections.Generic;
using Characters;
using Content;
using Events;
using Gameplay.Characters.Components;
using Gameplay.Characters.Models;
using Gameplay.Movement;
using UnityEngine;
using Utils;
using Utils.Events;
using Zenject;

namespace Gameplay.Characters
{
    public class CharactersSystem
    {
        private readonly Dictionary<CharacterType, BaseCharacterComponent> _components;

        private EventAggregator _eventAggregator;
        private TimeTicker _timeTicker;

        public CharactersSystem()
        {
            _components = new Dictionary<CharacterType, BaseCharacterComponent>
            {
                {CharacterType.Default, new BaseCharacterComponent()},
                {CharacterType.Hero, new HeroCharacterComponent()},
                {CharacterType.Enemy, new EnemyCharacterComponent()},
            };
        }

        public virtual void Initialize()
        {
            _timeTicker = ProjectContext.Instance.Container.Resolve<TimeTicker>();
            _eventAggregator = ProjectContext.Instance.Container.Resolve<EventAggregator>();

            foreach (var component in _components)
            {
                component.Value.Initialize();
            }

            _timeTicker.OnTick += Update;

            _eventAggregator.Add<CreateCharacterEvent>(CreateCharacter);
            _eventAggregator.Add<ReleaseCharacterEvent>(ReleaseCharacter);
        }

        public virtual void DeInitialize()
        {
            _eventAggregator.Remove<CreateCharacterEvent>(CreateCharacter);
            _eventAggregator.Remove<ReleaseCharacterEvent>(ReleaseCharacter);

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

        private void ApplyDebug()
        {
            _eventAggregator.SendEvent(new CreateCharacterEvent
            {
                Model = new BaseCharacterModel
                {
                    CharacterType = CharacterType.Hero,
                    GraphicPresetId = ContentProvider.Graphic.CharacterGraphicPreset.GetRandom(),
                    Movement = new WaypointMovement(),
                    Stats = new Stats
                    {
                        HeathPoint = 100,
                        MovementSpeed = 3f
                    }
                }
            });
        }
    }
}

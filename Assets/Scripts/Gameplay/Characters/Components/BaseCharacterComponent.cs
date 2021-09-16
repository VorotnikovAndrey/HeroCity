using System.Collections.Generic;
using Content;
using Gameplay.Characters.Models;
using Gameplay.Locations.View;
using UnityEngine;
using Utils;
using Utils.ObjectPool;
using Zenject;

namespace Gameplay.Characters.Components
{
    public class BaseCharacterComponent
    {
        protected Dictionary<BaseCharacterModel, BaseCharacterView> _characters;
        protected LocationView _locationView;

        public virtual void Initialize(object data = null)
        {
            _characters = new Dictionary<BaseCharacterModel, BaseCharacterView>();
            _locationView = ProjectContext.Instance.Container.Resolve<LocationView>();
        }

        public virtual void DeInitialize()
        {
            _characters = null;
        }

        public virtual void Update()
        {
            foreach (var character in _characters)
            {
                character.Key.Movement.Update();
            }
        }

        public virtual void Add(BaseCharacterModel model)
        {
            if (_characters.ContainsKey(model))
            {
                Debug.LogError("Duplicate model".AddColorTag(Color.red));
                return;
            }

            _characters.Add(model, CreateAndSpawnView(model));
        }

        public virtual void Remove(BaseCharacterModel model)
        {
            if (!_characters.ContainsKey(model))
            {
                Debug.LogError("Model is not found".AddColorTag(Color.red));
                return;
            }

            _characters.Remove(model);

            model.View.ReleaseItemView();
            model.View = null;
        }

        protected virtual BaseCharacterView CreateAndSpawnView(BaseCharacterModel model)
        {
            var graphic = ContentProvider.Graphic.CharacterGraphicPreset.Get(model.GraphicPresetId);
            var view = ViewGenerator.GetOrCreateItemView<BaseCharacterView>(GameConstants.View.DefaultCharacterPath);

            model.View = view;
            model.Movement.Initialize(view.Transform, view.WorldPosition, model.Stats.MovementSpeed);

            view.SetGraphic(graphic);

            return view;
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using Content;
using Gameplay.Characters.Models;
using Gameplay.Locations.View;
using UnityEngine;
using UserSystem;
using Utils;
using Utils.ObjectPool;
using Zenject;

namespace Gameplay.Characters.Components
{
    public class BaseCharacterComponent
    {
        protected Dictionary<BaseCharacterModel, BaseCharacterView> _characters;
        protected LocationView _locationView;
        protected UserManager _userManager;

        public virtual void Initialize(object data = null)
        {
            _characters = new Dictionary<BaseCharacterModel, BaseCharacterView>();
            _locationView = ProjectContext.Instance.Container.Resolve<LocationView>();
            _userManager = ProjectContext.Instance.Container.Resolve<UserManager>();
        }

        public virtual void DeInitialize()
        {
            foreach (var character in _characters.ToArray())
            {
                Remove(character.Key);
            }

            _characters = null;
        }

        public virtual void Update()
        {
            foreach (var character in _characters)
            {
                character.Key.Movement?.Update();
            }
        }

        public virtual void Add(BaseCharacterModel model)
        {
            if (_characters.ContainsKey(model))
            {
                Debug.LogError("Duplicate model".AddColorTag(Color.red));
                return;
            }

            _characters.Add(model, Spawn(model));
        }

        public virtual void Remove(BaseCharacterModel model)
        {
            if (!_characters.ContainsKey(model))
            {
                Debug.LogError("Model is not found".AddColorTag(Color.red));
                return;
            }

            _characters.Remove(model);

            model.View.Deinitialize();
            model.View.ReleaseItemView();
            model.View = null;
        }

        protected virtual BaseCharacterView Spawn(BaseCharacterModel model)
        {
            var graphic = ContentProvider.Graphic.CharacterGraphicPreset.Get(model.GraphicPresetId, model.CharacterType);
            var view = ViewGenerator.GetOrCreateItemView<BaseCharacterView>(GameConstants.View.DefaultCharacterPath);

            model.View = view;
            model.Movement.Initialize(view, view.WorldPosition, model.Stats.MovementSpeed);

            view.Initialize(model);
            view.SetGraphic(graphic.Object);

            return view;
        }
    }
}

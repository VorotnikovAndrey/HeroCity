using System.Linq;
using Content;
using Gameplay.Characters.AI;
using Gameplay.Characters.Models;
using UnityEngine;
using UserSystem;
using Utils.ObjectPool;

namespace Gameplay.Characters.Components
{
    public class HeroCharacterComponent : BaseCharacterComponent
    {
        public override void DeInitialize()
        {
            foreach (var character in _characters)
            {
                character.Key.AIController.DeInitialize();
                character.Key.AIController = null;
            }

            base.DeInitialize();
        }

        protected override BaseCharacterView Spawn(BaseCharacterModel model)
        {
            var position = Vector3.zero;
            if (model.SaveData.LastPosition != null)
            {
                position = new Vector3(model.SaveData.LastPosition[0], model.SaveData.LastPosition[1], model.SaveData.LastPosition[2]);
            }

            var view = ViewGenerator.GetOrCreateItemView<BaseCharacterView>(
                GameConstants.View.DefaultCharacterPath,
                true,
                new ViewCreateParams
                {
                    Position = position
                });

            var graphicData = !string.IsNullOrEmpty(model.GraphicPresetId)
                ? ContentProvider.Graphic.CharacterGraphicPreset.Get(model.GraphicPresetId, model.CharacterType)
                : ContentProvider.Graphic.CharacterGraphicPreset.GetRandom(model.CharacterType);

            model.GraphicPresetId = graphicData.Id;
            model.View = view;

            view.Initialize(model);
            view.SetGraphic(graphicData.Object);

            model.AIController = new HeroAIController();
            model.AIController.Initialize(model); // TODO: Не забыть по DeInitialize
            model.AIController.Start();

            return view;
        }
    }
}

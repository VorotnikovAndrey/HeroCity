using Content;
using Gameplay.Characters.Models;
using Source;
using Utils.ObjectPool;
using Utils.Pathfinding;

namespace Gameplay.Characters.Components
{
    public class HeroCharacterComponent : BaseCharacterComponent
    {
        protected override BaseCharacterView CreateAndSpawnView(BaseCharacterModel model)
        {
            var graphic = ContentProvider.Graphic.CharacterGraphicPreset.Get(model.GraphicPresetId);
            var position = _locationView.WaypointsContainer.GetTypePositions(MapWaypointType.Enter).GetRandom().Position;
            var view = ViewGenerator.GetOrCreateItemView<BaseCharacterView>(
                GameConstants.View.DefaultCharacterPath,
                true,
                new ViewCreateParams
                {
                    Position = position
                });

            model.View = view;
            model.Movement.Initialize(view.Transform, view.WorldPosition, model.Stats.MovementSpeed);

            view.SetGraphic(graphic);

            return view;
        }
    }
}

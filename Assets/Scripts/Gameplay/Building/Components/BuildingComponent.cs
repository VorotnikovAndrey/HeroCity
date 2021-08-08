using Gameplay.Building.Models;
using Gameplay.Building.View;

namespace Gameplay.Building.Components
{
    public class BuildingComponent
    {
        private BuildingModel Model;
        private BuildingView View;

        public void Init(BuildingModel model, BuildingView view)
        {
            Model = model;
            View = view;
        }
    }
}
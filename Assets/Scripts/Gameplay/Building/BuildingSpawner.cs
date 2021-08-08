using Utils.Events;
using Zenject;

namespace Gameplay.Building
{
    public class BuildingSpawner
    {
        [Inject] private EventAggregator _eventAggregator;

        public void Load(object data)
        {
            // TODO: Добавить логику для загрузки и сохранения состояния уровня и его построек
        }
    }
}
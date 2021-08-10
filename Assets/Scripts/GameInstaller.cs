using CameraSystem;
using Defong.Utils;
using Economies;
using Gameplay.Building;
using PopupSystem;
using Stages;
using UserSystem;
using Utils.Events;
using Utils.GameStageSystem;
using Utils.PopupSystem;
using Zenject;

public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        // Main
        Container.Bind<EventAggregator>().AsSingle();
        Container.Bind<UserManager>().AsSingle();
        Container.Bind<PopupManager<PopupType>>().AsSingle();
        Container.BindInterfacesAndSelfTo<TimeTicker>().AsSingle();
        Container.Bind<CameraManager>().AsSingle();
        Container.Bind<StageController>().AsSingle();

        // Stages
        Container.Bind<AbstractStageBase>().To<LobbyStage>().AsSingle();
        Container.Bind<AbstractStageBase>().To<GameplayStage>().AsSingle();

        // Economies
        Container.Bind<LocationsEconomy>().FromScriptableObjectResource("LocationsEconomyData").AsSingle();
        Container.Bind<BuildingsEconomy>().FromScriptableObjectResource("BuildingsEconomyData").AsSingle();
        Container.Bind<ResourcesEconomy>().FromScriptableObjectResource("ResourcesEconomyData").AsSingle();

        // Buildings
        Container.Bind<BuildingsManager>().AsSingle();

        // Debug
        Container.Bind<CheatsManager>().AsSingle();
    }
}
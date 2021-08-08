using CameraSystem;
using Defong;
using Defong.Utils;
using Economies;
using GameStage.Stages;
using PopupSystem;
using Utils.Events;
using Utils.GameStageSystem;
using Utils.PopupSystem;
using Zenject;

public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<EventAggregator>().AsSingle();
        Container.Bind<PopupManager<PopupType>>().AsSingle();
        Container.BindInterfacesAndSelfTo<TimeTicker>().AsSingle();
        Container.Bind<CameraManager>().AsSingle();

        Container.Bind<StageController>().AsSingle();
        Container.Bind<AbstractStageBase>().To<LobbyStage>().AsSingle();
        Container.Bind<AbstractStageBase>().To<GameplayStage>().AsSingle();

        Container.Bind<BuildingsEconomy>().FromScriptableObjectResource("BuildingsEconomyData").AsSingle();
    }
}
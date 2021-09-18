using CameraSystem;
using Economies;
using Gameplay.Characters;
using PopupSystem;
using ResourceSystem;
using Stages;
using UserSystem;
using Utils;
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
        Container.Bind<GameResourceManager>().AsSingle();
        Container.Bind<PopupManager<PopupType>>().AsSingle();
        Container.BindInterfacesAndSelfTo<TimeTicker>().AsSingle();
        Container.Bind<CameraManager>().AsSingle();
        Container.Bind<StageController>().AsSingle();

        // Stages
        Container.Bind<AbstractStageBase>().To<PreloaderStage>().AsSingle();
        Container.Bind<AbstractStageBase>().To<GameplayStage>().AsSingle();

        // SO
        Container.Bind<LocationsEconomy>().FromScriptableObjectResource("LocationsEconomyData").AsSingle();
        Container.Bind<BuildingsEconomy>().FromScriptableObjectResource("BuildingsEconomyData").AsSingle();
        Container.Bind<ResourcesEconomy>().FromScriptableObjectResource("ResourcesEconomyData").AsSingle();
        Container.Bind<ImprovementEconomy>().FromScriptableObjectResource("ImprovementEconomyData").AsSingle();
        Container.Bind<CharacterGraphicPreset>().FromScriptableObjectResource("SO/Characters/CharacterGraphicPreset").AsSingle();
        Container.Bind<SpriteBank>().FromScriptableObjectResource("SO/SpriteBank/SpriteBankData").AsSingle();
    }
}
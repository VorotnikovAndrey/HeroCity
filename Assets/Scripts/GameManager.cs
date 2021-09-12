using Defong.ObjectPool;
using PopupSystem;
using Stages;
using UI;
using UnityEngine;
using UserSystem;
using Utils;
using Utils.Events;
using Utils.GameStageSystem;
using Utils.ObjectPool;
using Utils.PopupSystem;
using Zenject;

public class GameManager : SingletonBehaviour<GameManager>
{
    [Inject] public UserManager UserManager;
    [Inject] public EventAggregator EventAggregator;
    [Inject] public StageController StageController;
    [Inject] public TimeTicker TimeTicker;
    [Inject] public PopupManager<PopupType> PopupManager;

    private CheatsManager _cheatsManager;

    private void Start()
    {
        EventAggregator = new EventAggregator();
        PopupManager.SetMainCanvas(ProjectContext.Instance.Container.Resolve<MainCanvas>());

        ViewGenerator.SetUnitPool(new UnitPool());
        StageController.ChangeStage(StageType.Preloader, null);

        Application.targetFrameRate = 60;

        _cheatsManager = new CheatsManager();
    }
}
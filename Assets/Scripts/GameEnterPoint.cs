using Defong.ObjectPool;
using GameStage;
using PopupSystem;
using UI;
using UnityEngine;
using Utils.Events;
using Utils.GameStageSystem;
using Utils.ObjectPool;
using Utils.PopupSystem;
using Zenject;

public class GameEnterPoint : MonoBehaviour
{
    [Inject] private EventAggregator _eventAggregator;
    [Inject] private StageController _stageController;
    [Inject] private PopupManager<PopupType> _popupManager;

    private void Start()
    {
        _eventAggregator = new EventAggregator();
        _popupManager.SetMainCanvas(ProjectContext.Instance.Container.Resolve<MainCanvas>());

        ViewGenerator.SetUnitPool(new UnitPool());
        _stageController.ChangeStage(StageType.Lobby, null);

        Application.targetFrameRate = 60;
    }
}
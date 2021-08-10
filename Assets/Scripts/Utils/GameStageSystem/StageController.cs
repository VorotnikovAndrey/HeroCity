using System.Collections.Generic;
using Defong.GameStageSystem;
using Events;
using Stages;
using Utils.Events;
using Zenject;

namespace Utils.GameStageSystem
{
    public class StageController
    {
        public IStage Stage { get; private set; }
        
        private readonly Dictionary<StageType, AbstractStageBase> _stages = new Dictionary<StageType, AbstractStageBase>();
        private readonly EventAggregator _eventAggregator;

        [Inject]
        public StageController(EventAggregator eventAggregator)
        {
            var stages = ProjectContext.Instance.Container.ResolveAll<AbstractStageBase>();
            foreach (AbstractStageBase stageBase in stages)
            {
                _stages.Add(stageBase.StageType, stageBase);
            }

            _eventAggregator = eventAggregator;
            _eventAggregator.Add<ChangeStageEvent>(OnChangeStageEvent);
        }

        ~StageController()
        {
            _eventAggregator.Remove<ChangeStageEvent>(OnChangeStageEvent);
        }

        private void OnChangeStageEvent(ChangeStageEvent sender)
        {
            ChangeStage(sender.Stage, sender.Data);
        }

        public void ChangeStage(StageType stage, object data)
        {
            Stage?.DeInitialize();

            Stage = _stages[stage];
            Stage.Initialize(data);
            Stage.Show();
        }
    }
}
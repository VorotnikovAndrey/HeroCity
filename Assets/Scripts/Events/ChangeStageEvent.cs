using Defong.Events;
using GameStage;

namespace Events
{
    public class ChangeStageEvent : BaseEvent
    {
        public StageType Stage;
        public object Data;
    }
}
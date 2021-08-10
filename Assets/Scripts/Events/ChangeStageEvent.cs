using Stages;
using Utils.Events;

namespace Events
{
    public class ChangeStageEvent : BaseEvent
    {
        public StageType Stage;
        public object Data;
    }
}
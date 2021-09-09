using Zenject;

namespace UI
{
    public class MainCanvas : EventMonoBehavior
    {
        public void Awake()
        {
            ProjectContext.Instance.Container.BindInstances(this);
        }
    }
}
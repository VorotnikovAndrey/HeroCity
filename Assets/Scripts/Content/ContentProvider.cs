using Defong;
using Economies;
using Zenject;

namespace Content
{
    public static class ContentProvider
    {
        private static BuildingsEconomy _buildingsEconomy;
        public static BuildingsEconomy BuildingsEconomy
        {
            get
            {
                if (_buildingsEconomy == null)
                {
                    _buildingsEconomy = ProjectContext.Instance.Container.Resolve<BuildingsEconomy>();
                }

                return _buildingsEconomy;
            }
        }
    }
}
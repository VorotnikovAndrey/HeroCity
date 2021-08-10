using Economies;
using Zenject;

namespace Content
{
    public static class ContentProvider
    {
        private static LocationsEconomy _locationsEconomy;
        public static LocationsEconomy LocationsEconomy
        {
            get
            {
                if (_locationsEconomy == null)
                {
                    _locationsEconomy = ProjectContext.Instance.Container.Resolve<LocationsEconomy>();
                }

                return _locationsEconomy;
            }
        }

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

        private static ResourcesEconomy _resourcesEconomy;
        public static ResourcesEconomy ResourcesEconomy
        {
            get
            {
                if (_resourcesEconomy == null)
                {
                    _resourcesEconomy = ProjectContext.Instance.Container.Resolve<ResourcesEconomy>();
                }

                return _resourcesEconomy;
            }
        }
    }
}
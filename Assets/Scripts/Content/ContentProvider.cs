using Characters;
using Characters.AI.Behaviors;
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

        private static CharacterGraphicPreset _characterGraphicPreset;
        public static CharacterGraphicPreset CharacterGraphicPreset
        {
            get
            {
                if (_characterGraphicPreset == null)
                {
                    _characterGraphicPreset = ProjectContext.Instance.Container.Resolve<CharacterGraphicPreset>();
                }

                return _characterGraphicPreset;
            }
        }

        private static BehaviorsData _behaviorsData;
        public static BehaviorsData BehaviorsData
        {
            get
            {
                if (_behaviorsData == null)
                {
                    _behaviorsData = ProjectContext.Instance.Container.Resolve<BehaviorsData>();
                }

                return _behaviorsData;
            }
        }
    }
}
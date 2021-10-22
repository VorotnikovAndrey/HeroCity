using Economies;
using Gameplay.Characters;
using Gameplay.Time;
using Utils;
using Zenject;

namespace Content
{
    public static class ContentProvider
    {
        public class Economies
        {
            private static WeaponShopEconomy _weaponShopEconomy;

            public static WeaponShopEconomy WeaponShopEconomy
            {
                get
                {
                    if (_weaponShopEconomy == null)
                    {
                        _weaponShopEconomy = ProjectContext.Instance.Container.Resolve<WeaponShopEconomy>();
                    }

                    return _weaponShopEconomy;
                }
            }

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

            private static ImprovementEconomy _improvementEconomy;

            public static ImprovementEconomy ImprovementEconomy
            {
                get
                {
                    if (_improvementEconomy == null)
                    {
                        _improvementEconomy = ProjectContext.Instance.Container.Resolve<ImprovementEconomy>();
                    }

                    return _improvementEconomy;
                }
            }
        }

        public class AI
        {
        }

        public class Graphic
        {
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

            private static SpriteBank _spriteBank;

            public static SpriteBank SpriteBank
            {
                get
                {
                    if (_spriteBank == null)
                    {
                        _spriteBank = ProjectContext.Instance.Container.Resolve<SpriteBank>();
                    }

                    return _spriteBank;
                }
            }  
            
            private static CharacterNames _characterNames;

            public static CharacterNames CharacterNames
            {
                get
                {
                    if (_characterNames == null)
                    {
                        _characterNames = ProjectContext.Instance.Container.Resolve<CharacterNames>();
                    }

                    return _characterNames;
                }
            } 
            
            private static DayTimeParams _dayTimeParams;

            public static DayTimeParams DayTimeParams
            {
                get
                {
                    if (_dayTimeParams == null)
                    {
                        _dayTimeParams = ProjectContext.Instance.Container.Resolve<DayTimeParams>();
                    }

                    return _dayTimeParams;
                }
            }
        }
    }
}
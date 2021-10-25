using System.Collections.Generic;
using UnityEngine;
using Utils.ObjectPool;

namespace UI.Popups.Components
{
    public class StatsCharacterPopupPageElement : CharacterPopupPageElement
    {
        private readonly List<StatsInfoElement> _statsInfoElements = new List<StatsInfoElement>();

        [SerializeField] private Transform _statsContentHolder;

        public override void Show()
        {
            base.Show();

            _rawModelHolder.ResetRotation();

            FillStatsBar();
        }

        public override void Hide()
        {
            foreach (var element in _statsInfoElements)
            {
                element.ReleaseItemView();
            }

            _statsInfoElements.Clear();

            base.Hide();
        }

        public void EmitVelocity(Vector3 velocity)
        {
            _rawModelHolder.ApplyVelocity(velocity);
        }

        private void FillStatsBar()
        {
            var stats = new Dictionary<string, string>
            {
                {"HealthPoint", _model.Stats.MaxHealthPoint.ToString()},
                {"ManaPoint", _model.Stats.MaxManaPoint.ToString()},
                {"Strength", _model.Stats.Strength.ToString()},
                {"Dexterity", _model.Stats.Dexterity.ToString()},
                {"Intelligence", _model.Stats.Intelligence.ToString()},
                {"Concentration", _model.Stats.Concentration.ToString()},
                {"MovementSpeed", _model.Stats.MovementSpeed.ToString()},
                {"CriticalChance", _model.Stats.CriticalChance.ToString()},
                {"CriticalFactor", _model.Stats.CriticalFactor.ToString()},
                {"Accuracy", _model.Stats.Accuracy.ToString()},
                {"Evasion", _model.Stats.Evasion.ToString()},
                {"BlockChance", _model.Stats.BlockChance.ToString()},
                {"Protection", _model.Stats.Protection.ToString()},
                {"MagicProtection", _model.Stats.MagicProtection.ToString()},
            };

            foreach (var stat in stats)
            {
                var element = ViewGenerator.GetOrCreateItemView<StatsInfoElement>(GameConstants.View.StatsInfoElement);

                element.SetTitle(stat.Key);
                element.SetValue(stat.Value);
                element.SetParent(_statsContentHolder);

                _statsInfoElements.Add(element);
            }
        }
    }
}
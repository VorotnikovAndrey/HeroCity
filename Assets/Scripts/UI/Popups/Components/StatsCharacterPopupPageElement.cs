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
            foreach (var stat in _model.CharacterStats.BaseStats)
            {
                var element = ViewGenerator.GetOrCreateItemView<StatsInfoElement>(GameConstants.View.StatsInfoElement);

                element.SetKey(stat.Key.ToString());
                element.SetValue(stat.Value.ToString());
                element.SetParent(_statsContentHolder);

                _statsInfoElements.Add(element);
            }
        }
    }
}
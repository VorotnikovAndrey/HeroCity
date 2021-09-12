using System;
using System.Collections.Generic;
using Characters.AI.Behaviors;
using Gameplay.Locations.View;
using Gameplay.Movement;
using UnityEngine;
using Utils;

namespace Characters.Models
{
    public class BaseCharacterModel : IDisposable
    {
        public string GraphicPresetId;
        public CharacterType CharacterType;
        public Movement Movement;
        public Stats Stats;
        public List<ElementBehavior> AI;
        public BaseCharacterView View;

        public void Dispose()
        {
            AI.ForEach(x =>
            {
                x.Interrupt();
                x.DestroyElement();
            });

            AI = null;
            Movement = null;
            Stats = null;
            View = null;
        }

        public void ApplyAiSequention(BaseCharacterModel model)
        {
            if (AI.Count == 0)
            {
                Debug.LogError("Sequention is empty".AddColorTag(Color.red));
                return;
            }

            foreach (var element in AI)
            {
                element.Init(model);
            }

            for (int i = 0; i < AI.Count - 1; i++)
            {
                var nextElement = AI[i + 1];
                AI[i].Ended += x => nextElement.Begin();
            }

            AI[0].Begin();
        }
    }
}
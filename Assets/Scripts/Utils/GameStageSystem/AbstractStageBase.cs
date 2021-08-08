using System.Collections.Generic;
using Defong.GameStageSystem;
using Defong.Utils;
using GameStage;
using UnityEngine;

namespace Utils.GameStageSystem
{
    public abstract class AbstractStageBase : IStage
    {
        public abstract StageType StageType { get; }

        public Dictionary<object, IStage> SubStages { get; } = new Dictionary<object, IStage>();

        public virtual void Initialize(object data)
        {
            Debug.Log($"{StageType.AddColorTag(Color.yellow)} Initialized".AddColorTag(Color.cyan));

            foreach (IStage value in SubStages.Values)
            {
                value.Initialize(data);
            }
        }

        public virtual void DeInitialize()
        {
            Debug.Log($"{StageType.AddColorTag(Color.yellow)} DeInitialized".AddColorTag(Color.cyan));

            foreach (IStage value in SubStages.Values)
            {
                value.DeInitialize();
            }
        }

        public abstract void Show();

        public abstract void Hide();
    }
}

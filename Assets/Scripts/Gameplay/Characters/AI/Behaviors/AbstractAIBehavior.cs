using System;
using Gameplay.Characters.Models;
using UnityEngine;
using Utils;

namespace Gameplay.Characters.AI.Behaviors
{
    public abstract class AbstractAIBehavior
    {
        public event Action Beginning;
        public event Action Ended;
        public event Action<string> Interrupted;

        protected BaseCharacterModel Model;

        public virtual void Initialize(BaseCharacterModel model)
        {
            Model = model;
        }

        public virtual void DeInitialize()
        {
            Model = null;
        }

        public abstract void Begin();
        public abstract void Interrupt();
        protected abstract void End();

        protected void SendBeginning()
        {
            Beginning?.Invoke();
        }

        protected void SendEnded()
        {
            Ended?.Invoke();
        }

        protected void SendInterrupted(string reason)
        {
            Interrupted?.Invoke(reason);
        }
    }
}
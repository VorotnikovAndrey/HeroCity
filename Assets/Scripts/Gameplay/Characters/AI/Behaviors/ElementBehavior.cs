using System;
using Characters.Models;
using UnityEngine;

namespace Characters.AI.Behaviors
{
    [Serializable]
    public abstract class ElementBehavior : EventScriptableObjectBehavior
    {
        public event Action<ElementBehavior> Beginning;
        public event Action<ElementBehavior> Ended;
        public event Action<ElementBehavior, string> Interrupted;

        protected BaseCharacterModel Model;

        public abstract void Begin();
        public abstract void End();
        public abstract void Interrupt();

        public virtual void Init(BaseCharacterModel model)
        {
            Model = model;
        }

        protected void SendBeginning()
        {
            Beginning?.Invoke(this);
        }

        protected void SendEnded()
        {
            Ended?.Invoke(this);
        }

        protected void SendInterrupted(string reason)
        {
            Interrupted?.Invoke(this, reason);
        }

        public ElementBehavior GetClone()
        {
            return Instantiate(this);
        }

        public void DestroyElement()
        {
            Beginning = null;
            Ended = null;
            Interrupted = null;

            Model = null;
            DestroyImmediate(this);
        }
    }
}
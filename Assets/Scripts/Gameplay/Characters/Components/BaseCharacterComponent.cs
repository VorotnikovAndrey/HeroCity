using Characters.Models;
using UnityEngine;

namespace Characters.Components
{
    public class BaseCharacterComponent
    {
        public virtual void Initialize()
        {
       
        }

        public virtual void DeInitialize()
        {
       
        }

        public virtual void Update(BaseCharacterModel model)
        {
            model.Movement.Update(Time.deltaTime);
        }
    }
}

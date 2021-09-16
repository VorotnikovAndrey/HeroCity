using Characters;
using Gameplay.Characters.Models;
using UnityEngine;
using Utils.ObjectPool;

namespace Gameplay.Characters
{
    public class BaseCharacterView : AbstractBaseView
    {
        [SerializeField] protected Transform GraphicHolder;
        [SerializeField] protected Vector3 GraphicOffset;

        public BaseCharacterModel Model { get; protected set; }
        public Animator Animator { get; protected set; }

        protected GameObject Graphic;

        public void Initialize(BaseCharacterModel model)
        {
            Model = model;
        }

        public void DeInitialize()
        {
            Model = null;

            if (Graphic != null)
            {
                Destroy(Graphic);
            }
        }

        public void SetGraphic(GameObject graphic)
        {
            if (Graphic != null)
            {
                Destroy(Graphic);
            }

            Graphic = Instantiate(graphic, GraphicHolder);
            Graphic.transform.localPosition = GraphicOffset;

            // TODO:
            Animator = Graphic.GetComponent<Animator>();
        }
    }
}
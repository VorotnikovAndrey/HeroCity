using UnityEngine;
using Utils.ObjectPool;

namespace Characters
{
    public class BaseCharacterView : AbstractBaseView
    {
        [SerializeField] protected Transform GraphicHolder;
        [SerializeField] protected Vector3 GraphicOffset;

        public virtual Animator Animator { get; protected set; }

        protected GameObject Graphic;

        public void SetGraphic(CharacterGraphicPresetPair preset)
        {
            if (Graphic != null)
            {
                Destroy(Graphic);
            }

            Graphic = Instantiate(preset.Object, GraphicHolder);
            Graphic.transform.localPosition = GraphicOffset;

            // TODO:
            Animator = Graphic.GetComponent<Animator>();
        }
    }
}
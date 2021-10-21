using UnityEngine;

namespace Gameplay.Craft
{
    public class WeaponCraftContent : MonoBehaviour
    {
        [SerializeField] private Transform _content;

        public Transform Content => _content;
    }
}
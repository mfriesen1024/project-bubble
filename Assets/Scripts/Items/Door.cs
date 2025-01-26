using System;
using UnityEngine;

namespace Assets.Scripts.Player
{
    internal class Door:MonoBehaviour
    {
        public PotionType RequiredPotion;
        public Action OnDoorMelt = delegate { };
        [SerializeField] Collider wallCollider;

        // Is this wonky idk.
        public void DoThings()
        {
            OnDoorMelt();

            // This is temporary.
            wallCollider.enabled = false;
        }
    }
}
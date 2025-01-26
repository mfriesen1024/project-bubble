using System;
using UnityEngine;

namespace Assets.Scripts.Items
{
    public class Door : MonoBehaviour
    {
        public PotionType RequiredPotion;
        public Action OnDoorMelt = delegate { };
        [SerializeField] Collider wallCollider;

        // Is this wonky idk.
        public void DoThings()
        {
            OnDoorMelt();

            wallCollider.enabled = false;
            // While waiting for door animations, just delete the entire door.
            Destroy(gameObject);
        }
    }
}
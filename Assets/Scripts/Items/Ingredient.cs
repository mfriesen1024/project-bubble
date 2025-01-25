using System;
using UnityEngine;

namespace Assets.Scripts.Player
{
    internal class Ingredient:MonoBehaviour
    {
        public Action Pop = delegate { };

        [SerializeField] GameObject bubble;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out InteractionController ic))
            {
                bubble.SetActive(false);
            }
        }
    }
}
using Assets.Scripts.Player;
using System;
using UnityEngine;

namespace Assets.Scripts.Items
{
    internal class Ingredient : MonoBehaviour
    {
        public Action Pop = delegate { };
        [SerializeField] internal IngredientType type;
        [SerializeField] GameObject bubble;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out InteractionController ic))
            {
                bubble.SetActive(false);
            }
        }
    }

    public enum IngredientType
    {
        oran,
        idk,
        bird
    }
}
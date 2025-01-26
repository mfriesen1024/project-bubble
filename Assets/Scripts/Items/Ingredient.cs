using Assets.Scripts.Player;
using System;
using UnityEngine;

namespace Assets.Scripts.Items
{
    internal class Ingredient : MonoBehaviour
    {
        public Action Pop = delegate { }; // Pop action
        [SerializeField] internal IngredientType type; // Type of the ingredient
        [SerializeField] GameObject bubble; // Reference to bubble GameObject

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out InteractionController ignored))
            {
                Debug.Log($"Trying to pop bubble of ingredient {name}.");
                bubble.SetActive(false);
            }
            else { Debug.Log($"Btw, no IC found."); }
        }
    }

    public enum IngredientType
    {
        oran,
        idk,
        bird
    }
}

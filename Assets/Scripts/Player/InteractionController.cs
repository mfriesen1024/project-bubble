using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class InteractionController : MonoBehaviour
    {
        public Action OnInteract;
        public IngredientType? heldIngredient = null;
        public PotionType? heldPotion = null;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void FixedUpdate()
        {
            
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.TryGetComponent(out Ingredient bubbledIngredient))
            {
                heldIngredient = bubbledIngredient.type;
                Debug.Log($"Got ingredient {heldIngredient}");
            }
            if(other.TryGetComponent(out Beaker beaker))
            {
                if(heldIngredient != null)
                {
                    heldPotion = (PotionType)(IngredientType)heldIngredient;
                    heldIngredient = null;
                    Debug.Log($"Made potion {heldPotion}");
                }
            }
            if(other.TryGetComponent(out Door door))
            {
                if(heldPotion == door.RequiredPotion)
                {
                    Debug.Log($"Telling {door.name} to do things.");
                    door.DoThings();
                }
            }
        }
    }
}
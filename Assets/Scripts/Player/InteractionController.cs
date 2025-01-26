using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class InteractionController : MonoBehaviour
    {
        public Action OnInteract;
        public IngredientType? heldIngredient;
        public PotionType? heldPotion;

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
            if(TryGetComponent(out Ingredient bubbledIngredient))
            {
                heldIngredient = bubbledIngredient.type;
            }
            if(TryGetComponent(out Beaker beaker))
            {
                if(heldIngredient != null)
                {
                    heldPotion = (PotionType)(IngredientType)heldIngredient;
                }
            }
            if(TryGetComponent(out Door door))
            {
                if(heldPotion == door.RequiredPotion)
                {
                    door.DoThings();
                }
            }
        }
    }
}
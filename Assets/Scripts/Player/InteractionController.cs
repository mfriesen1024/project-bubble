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
        Ingredient heldItem;
        [SerializeField] Vector3 heldItemPos = Vector3.one;

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
            if(other.TryGetComponent(out Ingredient ingredient))
            {
                heldIngredient = ingredient.type;
                heldItem = ingredient;
                Debug.Log($"Got ingredient {heldIngredient}");
                ingredient.transform.parent = transform;
                ingredient.transform.localPosition = heldItemPos;
            }
            if(other.TryGetComponent(out Beaker beaker))
            {
                if(heldIngredient != null)
                {
                    heldPotion = (PotionType)(IngredientType)heldIngredient;
                    heldIngredient = null;
                    heldItem.gameObject.SetActive(false);
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
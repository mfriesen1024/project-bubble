using Assets.Scripts.Items;
using Assets.Scripts.Managers;
using Assets.Scripts.Misc;
using System;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class InteractionController : MonoBehaviour
    {
        public Action OnInteract;
        public IngredientType? heldIngredient = null;
        public PotionType? heldPotion = null;
        private Ingredient heldItem;

        [SerializeField] private Vector3 heldItemPos = Vector3.one;
        [SerializeField] private Animator animator; // Reference to Animator

        private static readonly int IsHolding = Animator.StringToHash("isHolding"); // Animation parameter

        // Initialization
        void Start()
        {
            if (animator == null)
            {
                animator = GetComponent<Animator>();
                if (animator == null)
                {
                    Debug.LogWarning("Animator not assigned and not found on the GameObject.");
                }
            }
        }

        // Called once per frame
        void Update()
        {
            // Ensure "isHolding" parameter updates if the held item is destroyed
            if (heldItem == null && animator != null)
            {
                animator.SetBool(IsHolding, false);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Ingredient ingredient))
            {
                heldIngredient = ingredient.type;
                heldItem = ingredient;
                Debug.Log($"Got ingredient {heldIngredient}");
                ingredient.transform.parent = transform;
                ingredient.transform.localPosition = heldItemPos;

                // Set the animation to holding
                if (animator != null)
                {
                    animator.SetBool(IsHolding, true);
                }
            }

            if (other.TryGetComponent(out Beaker beaker))
            {
                if (heldIngredient != null)
                {
                    heldPotion = (PotionType)(IngredientType)heldIngredient;
                    heldIngredient = null;

                    if (heldItem != null)
                    {
                        heldItem.gameObject.SetActive(false);
                        heldItem = null;
                    }

                    Debug.Log($"Made potion {heldPotion}");

                    // Reset to idle if item is destroyed
                    if (animator != null)
                    {
                        animator.SetBool(IsHolding, false);
                    }
                }
            }

            if (other.TryGetComponent(out Door door))
            {
                if (heldPotion == door.RequiredPotion)
                {
                    Debug.Log($"Telling {door.name} to do things.");
                    door.DoThings();

                    // Reset held potion and update animation
                    heldPotion = null;
                    if (animator != null)
                    {
                        animator.SetBool(IsHolding, false);
                    }
                }
            }

            if (other.TryGetComponent(out EndLevelTrigger ignored) || other.name == "ELT")
            {
                GameManager.instance.UIManager.State = UIState.endLevel;
            }
        }
    }
}

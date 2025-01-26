using UnityEngine;

namespace Assets.Scripts.Items
{
    internal class Beaker : MonoBehaviour
    {
        [SerializeField] private string childName = "ChildName"; // Name of the child to change color
        [SerializeField] private Color colorForPotionZero = Color.white;
        [SerializeField] private Color colorForPotionOne = Color.blue;
        [SerializeField] private Color colorForPotionTwo = Color.green;

        private void OnTriggerEnter(Collider other)
        {
            // Check if the object entering is an Ingredient
            if (other.TryGetComponent<Ingredient>(out var ingredient))
            {
                // Determine the PotionType based on the IngredientType
                PotionType potionType = MapIngredientToPotion(ingredient.type);

                // Change the color of the child
                ChangeColorBasedOnPotion(potionType);

                Debug.Log($"Ingredient {ingredient.type} dropped into Beaker. Changed color to {potionType}.");
            }
        }

        private void ChangeColorBasedOnPotion(PotionType potionType)
        {
            // Find the specific child by name
            Transform child = transform.Find(childName);
            if (child != null)
            {
                Renderer renderer = child.GetComponent<Renderer>();
                if (renderer != null)
                {
                    // Set color based on potion type
                    switch (potionType)
                    {
                        case PotionType.Zero:
                            renderer.material.color = colorForPotionZero;
                            break;

                        case PotionType.One:
                            renderer.material.color = colorForPotionOne;
                            break;

                        case PotionType.Two:
                            renderer.material.color = colorForPotionTwo;
                            break;

                        default:
                            Debug.LogWarning("Unhandled PotionType.");
                            break;
                    }
                }
                else
                {
                    Debug.LogWarning($"Renderer not found on child {childName}");
                }
            }
            else
            {
                Debug.LogWarning($"Child {childName} not found under Beaker GameObject.");
            }
        }

        private PotionType MapIngredientToPotion(IngredientType ingredientType)
        {
            // Map IngredientType to PotionType (custom logic here)
            switch (ingredientType)
            {
                case IngredientType.oran:
                    return PotionType.Zero;

                case IngredientType.idk:
                    return PotionType.One;

                case IngredientType.bird:
                    return PotionType.Two;

                default:
                    Debug.LogWarning($"Unhandled IngredientType: {ingredientType}");
                    return PotionType.Zero; // Default fallback
            }
        }
    }

    public enum PotionType
    {
        Zero,
        One,
        Two
    }
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
public class BrewingStation : DropingObject
{
    public Image brewingHold; // just an image offscreen to put brewing ingredients in until they're turned into a potion
    public List<IngredientObject> ingredients;

    void Update()
    {
        if (ingredients.Count == 3)
        {
            //wait some time?
            BrewPotion();
        }
    }

    public override void ActWith(DragableObject obj)
    {
        if (!obj.TryGetComponent<IngredientObject>(out IngredientObject ingredient)) return;
        ingredients.Add(ingredient);

        ingredient.transform.SetParent(brewingHold.transform);
    }

    public void BrewPotion()
    {
        //put ingredients into potion for their logic

        //put out potion as a pickup

        ingredients.Clear();
    }
}
*/
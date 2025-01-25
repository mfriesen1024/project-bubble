using UnityEngine;

public class PotionObject : DragableObject
{
    public IngredientObject[] ingredients;

    public override void Activate(DropingObject obj)
    {
        for (int i = 0; i < ingredients.Length; i++)
        { 
            ingredients[i].UseEffect(obj);
        }

        base.Activate(obj);
    }
}

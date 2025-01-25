using UnityEngine;

public class IngredientObject : DragableObject
{
    public override void Activate()
    {
        //add this object to brewing station list
        
        //then remove from play?
        base.Activate();
    }

    public virtual void CreateEffect()
    {

    }
}

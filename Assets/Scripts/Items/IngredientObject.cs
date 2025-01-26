using UnityEngine;


/*
public class IngredientObject : DragableObject
{
    public enum IngredientEffect
    {
        BREAKWALL,
        DAMAGE,
        CRATER,// ie destroy every dropingobject
        HEAL,
        SPARKLE,
        MELTLOCK,
        NOTHING
    }

    public IngredientEffect effect = IngredientEffect.NOTHING;
    public int effectValue; // amount of damage or health

    public virtual void UseEffect(DropingObject obj)
    {
        switch (effect)
        { 
            case IngredientEffect.BREAKWALL: // this could be divided into
                                             // different break walls for different types of ingredients
                if (obj.TryGetComponent(out WallObject wall))
                    Destroy(wall.gameObject);
                return;

            case IngredientEffect.DAMAGE:
                //getcomponent and remove health
                return;

            case IngredientEffect.CRATER: // ie destroy every dropingobject
                Destroy(obj.gameObject);
                return;

            case IngredientEffect.HEAL:
                //getcomponent and add health
                return;

            case IngredientEffect.SPARKLE:
                //add some extra effects
                return;

            case IngredientEffect.MELTLOCK:
                //check if lock
                return;

            case IngredientEffect.NOTHING:
                // ;)
                return;
        }

        Activate(obj);
    }
}
*/
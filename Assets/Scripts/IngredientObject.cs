using UnityEngine;

public class IngredientObject : MonoBehaviour
{
    public void AddToBrew()
    {
        //add this object to brewing station list
        //for now delete
        Destroy(gameObject);
    }

    public virtual void CreateEffect()
    {

    }
}

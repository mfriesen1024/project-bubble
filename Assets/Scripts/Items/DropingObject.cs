using UnityEngine;
using UnityEngine.EventSystems;

public abstract class DropingObject : MonoBehaviour, IDropHandler
{
    //ABSTRACT OBJECT DRAGABLES ARE DROPPED ON eg: WALLS, ENEMIES, PLAYER

    protected bool activating; //used for brewing and potion usage
    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedObject = eventData.pointerDrag;
        DragableObject dragableObject = droppedObject.GetComponent<DragableObject>();

        if (transform.childCount == 0)
        { 
            dragableObject.parentAfterDrag = transform;
        }
    }

    public virtual void ActWith(DragableObject obj)
    {
        if (activating)
        { 
            obj.Activate(this);
        }
    }
}

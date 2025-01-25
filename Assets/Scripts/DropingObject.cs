using UnityEngine;
using UnityEngine.EventSystems;

public class DropingObject : MonoBehaviour, IDropHandler
{
    public bool activating; //used for brewing and potion usage
    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedObject = eventData.pointerDrag;
        DragableObject dragableObject = droppedObject.GetComponent<DragableObject>();

        if (transform.childCount == 0)
        { 
            dragableObject.parentAfterDrag = transform;
        }
    }

    public void Activate(DragableObject obj)
    {
        if (activating)
        { 
            Debug.Log("Dropper");
            obj.Activate();
        }
    }
}

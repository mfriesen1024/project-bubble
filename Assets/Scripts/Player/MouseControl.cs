using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MouseControl : MonoBehaviour
{
    [SerializeField] private Camera camera;
    public GameObject pointer;
    public DragableObject heldObject;

    private void Update()
    {
        //Interact with WorldObject
        { 
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit raycastHit))
            {
                pointer.transform.position = raycastHit.point;
                
                if (raycastHit.transform.gameObject.TryGetComponent<DropingObject>(out DropingObject drop)) 
                {
                    if (Input.GetMouseButtonUp(0) && heldObject != null)
                    { 
                        drop.Activate(heldObject);
                    }
                }
            }
        }

        //Interact with UIObject
        if (EventSystem.current.IsPointerOverGameObject())
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            pointerData.position = Input.mousePosition;

            List<RaycastResult> resultsList = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, resultsList);
            for (int i = 0; i < resultsList.Count; i++)
            {
                if (resultsList[i].gameObject.TryGetComponent<DragableObject>(out DragableObject drag))
                {
                    if (Input.GetMouseButtonDown(0))
                    { 
                        heldObject = drag;
                    }
                }
            }
        }

        // let go of held object
        if (Input.GetMouseButtonUp(0) && heldObject != null)
        {
            heldObject = null;
        }
    }
}

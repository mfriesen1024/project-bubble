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
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            pointer.transform.position = raycastHit.point;

            if (raycastHit.transform.gameObject.TryGetComponent<DragableObject>(out DragableObject drag))
            { 
                heldObject = drag;
            }

            if (raycastHit.transform.gameObject.TryGetComponent<DropingObject>(out DropingObject drop)) 
            {
                drop.Activate(heldObject);
            }
        }
    }
}

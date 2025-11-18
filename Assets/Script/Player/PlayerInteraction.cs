using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public Transform InteractPosition;

    public float interactionDistance = 1.2f;


    public KeyCode interactKey = KeyCode.E;

    void Update()
    {
        Vector3 rayOrigin = InteractPosition.position;
        Vector3 rayDirection = InteractPosition.forward;

        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        Color lineColor = Color.red;

        if (Physics.Raycast(ray, out hit, interactionDistance))
        {   
            lineColor = Color.green;

            Debug.DrawLine(rayOrigin, hit.point, lineColor);

            IInteractable interactable = hit.collider.GetComponent<IInteractable>();

            if (interactable != null)
            {

                if (Input.GetKeyDown(interactKey))
                {
                    interactable.Interact(); 
                }
            }
        }
        else
        {
            Debug.DrawRay(rayOrigin, rayDirection * interactionDistance, lineColor);
            
        }
    }
}
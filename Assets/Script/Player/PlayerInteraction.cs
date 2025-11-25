using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionDistance = 1.8f;
    public float sphereRadius = 0.4f;
    public Transform LayPoint;
    public KeyCode interactKey = KeyCode.E;
    public Text interactText;
    public LayerMask interactLayer;      

    public bool isRiding = false;
    public bool uiOpened = false;

    IInteractable currentObj;

    void Update()
    {
        if (isRiding || uiOpened)
        {
            HidePopup();
            return;
        }

        Vector3 origin = LayPoint.position;
        Vector3 dir = LayPoint.forward;

        Debug.DrawRay(origin, dir * interactionDistance, Color.green);

        RaycastHit hit;
        if (Physics.SphereCast(origin, sphereRadius, dir, out hit, interactionDistance, interactLayer))
        {
            //Debug.Log("Hit: " + hit.collider.name);

            IInteractable interactable = hit.collider.GetComponent<IInteractable>();

            if (interactable != null)
            {
                ShowPopup(interactable.GetPrompt());

                if (Input.GetKeyDown(interactKey))
                    interactable.Interact(gameObject);

                currentObj = interactable;
                return;
            }
        }

        HidePopup();
        currentObj = null;
    }

    void ShowPopup(string msg)
    {
        if (interactText != null)
        {
            interactText.text = msg;
            interactText.enabled = true;
        }
    }

    void HidePopup()
    {
        if (interactText != null)
        {
            interactText.text = "";
            interactText.enabled = false;
        }
    }
}

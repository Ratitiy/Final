using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionDistance = 2.5f;
    public float sphereRadius = 0.4f;
    public KeyCode interactKey = KeyCode.E;
    public Text interactText;

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

        Transform cam = Camera.main.transform;
        Vector3 origin = cam.position;
        Vector3 dir = cam.forward;

        Debug.DrawRay(origin, dir * interactionDistance, Color.red);

        int mask = ~LayerMask.GetMask("Player");

        RaycastHit hit;
        if (Physics.SphereCast(origin, sphereRadius, dir, out hit, interactionDistance, mask))
        {
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

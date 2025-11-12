using UnityEngine;
using UnityEngine.UI;

public class PlayerInteractor : MonoBehaviour
{
    public Camera cam;
    public float interactDistance = 3f;
    public LayerMask interactableLayer;
    public Text ShowText;//text Ui

    IInteractable current;

    void Start()
    {
        if (!cam) cam = Camera.main;
        ShowPrompt(null);
    }

    void Update()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance, interactableLayer))
        {
            var target = hit.collider.GetComponentInParent<IInteractable>();
            if (target != null)
            {
                // เปลี่ยนโฟกัส
                if (current != target)
                {
                    current?.OnLoseFocus();
                    current = target;
                    current.OnFocus();
                }
                ShowPrompt(current.GetPrompt());

                if (Input.GetKeyDown(KeyCode.E))
                    current.Interact(gameObject);
                return;
            }
        }
        if (current != null)
        {
            current.OnLoseFocus();
            current = null;
        }
        ShowPrompt(null);
    }

    void ShowPrompt(string Message)
    {
        if (!ShowText) return;
        if (string.IsNullOrEmpty(Message))
        {
            ShowText.enabled = false;
        }
        else
        {
            ShowText.enabled = true;
            ShowText.text = Message;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (!cam) return;
        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(cam.transform.position, cam.transform.forward * interactDistance);
    }
}

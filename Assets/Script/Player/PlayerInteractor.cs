using UnityEngine;
using UnityEngine.UI;

public class PlayerInteractor : MonoBehaviour
{
    public Camera cam;
    public float interactDistance = 3f;
    public float sphereRadius = 0.8f;
    public LayerMask interactableLayer;
    public Text ShowText; // text UI

    IInteractable current;

    void Start()
    {
        if (!cam) cam = Camera.main;
        ShowPrompt(null);
    }

    void Update()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        Debug.DrawRay(ray.origin, ray.direction * interactDistance, Color.green);

        if (Physics.SphereCast(ray, sphereRadius, out RaycastHit hit, interactDistance, interactableLayer))
        {
            Debug.Log("Ray hit: " + hit.collider.name);
            var target = hit.collider.GetComponentInParent<IInteractable>();
            if (target != null)
            {
                if (current != target)
                {
                    current?.OnLoseFocus();
                    current = target;
                    current.OnFocus();
                    Debug.Log("Focused on: " + current);
                }
                ShowPrompt(current.GetPrompt());

                if (Input.GetKeyDown(KeyCode.E))
                {
                    Debug.Log("Pressed E on: " + current);
                    current.Interact(gameObject);
                }
                return;
            }
        }

        if (current != null)
        {
            Debug.Log("Lost focus on: " + current);
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
        Vector3 origin = cam.transform.position;
        Vector3 direction = cam.transform.forward;
        Gizmos.DrawWireSphere(origin + direction * interactDistance, sphereRadius);
    }
}

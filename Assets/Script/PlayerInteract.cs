using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public float interactDistance = 2f;
    public Vector3 rayOffset = new Vector3(0, 1.2f, 0);

    [Header("UI")]
    public GameObject pressFUI;

    [Header("Debug")]
    public bool showRay = true;

    bool canInteract;
    Collider currentTarget;

    void Update()
    {
        CheckInteractable();

        if (canInteract && Input.GetKeyDown(KeyCode.F))
        {
            currentTarget.SendMessage(
                "Interact",
                gameObject,
                SendMessageOptions.DontRequireReceiver
            );
        }
    }

    void CheckInteractable()
    {
        Vector3 origin = transform.position + rayOffset;
        Vector3 dir = transform.forward;

        if (Physics.Raycast(origin, dir, out RaycastHit hit, interactDistance))
        {
            currentTarget = hit.collider;
            canInteract = true;

            if (pressFUI && !pressFUI.activeSelf)
                pressFUI.SetActive(true);

            if (showRay)
                Debug.DrawRay(origin, dir * hit.distance, Color.red);
        }
        else
        {
            currentTarget = null;
            canInteract = false;

            if (pressFUI && pressFUI.activeSelf)
                pressFUI.SetActive(false);

            if (showRay)
                Debug.DrawRay(origin, dir * interactDistance, Color.green);
        }
    }
}

using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public float interactDistance = 2f;
    public float sphereRadius = 0.5f; // เพิ่มขนาดความกว้างของวงกลม (รัศมี)
    public Vector3 rayOffset = new Vector3(0, 1.2f, 0);
    public LayerMask interactLayer;

    [Header("UI")]
    public GameObject pressFUI;

    [Header("Debug")]
    public bool showRay = true;

    bool canInteract;
    public Collider currentTarget;

    void Update()
    {
        CheckInteractable();

        // เปลี่ยนเป็นปุ่ม F ตามเดิมที่คุณใช้
        if (canInteract && Input.GetKeyDown(KeyCode.F))
        {
            // ส่งข้อความ Interact ไปยัง Object ที่โดนชน
            currentTarget.SendMessage(
                "Interact",
                gameObject,
                SendMessageOptions.DontRequireReceiver
            );
        }
    }

    void CheckInteractable()
    {
        float backstep = 1.0f;
        Vector3 origin = transform.position + rayOffset - (transform.forward * backstep);
        Vector3 dir = transform.forward;

        // เพิ่ม interactLayer เข้าไปในพารามิเตอร์สุดท้าย
        if (Physics.SphereCast(origin, sphereRadius, dir, out RaycastHit hit, interactDistance + backstep, interactLayer))
        {
            currentTarget = hit.collider;
            canInteract = true;

            if (pressFUI && !pressFUI.activeSelf)
                pressFUI.SetActive(true);
        }
        else
        {
            currentTarget = null;
            canInteract = false;

            if (pressFUI && pressFUI.activeSelf)
                pressFUI.SetActive(false);
        }
    }

    // ส่วนของ Debug เพื่อให้มองเห็นขนาดของ SphereCast ในหน้า Scene
    void OnDrawGizmos()
    {
        if (!showRay) return;

        Gizmos.color = canInteract ? Color.red : Color.green;
        Vector3 origin = transform.position + rayOffset;
        Vector3 dir = transform.forward;

        // วาดเส้นและทรงกลมจำลอง
        Gizmos.DrawLine(origin, origin + dir * interactDistance);
        Gizmos.DrawWireSphere(origin + dir * interactDistance, sphereRadius);
    }
}
using UnityEngine;

public class BikeController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float boostSpeed = 16f;
    public float turnSpeed = 120f;
    public float brakeForce = 50f;

    public Transform exitPoint;
    public CamaraSwitch camaraSwitch;
    public Transform motorcycleCamTarget;
    public GameObject Drive;

    bool isDriving = false;
    GameObject currentPlayer;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        
        rb.centerOfMass = new Vector3(0f, -0.5f, 0f);
    }

    void FixedUpdate()
    {
        if (!isDriving) return;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        bool boosting = Input.GetKey(KeyCode.LeftShift);
        float currentSpeed = boosting ? boostSpeed : moveSpeed;

        
        Vector3 move = transform.forward * v * currentSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + move);

       
        Quaternion turnRot = Quaternion.Euler(0, h * turnSpeed * Time.fixedDeltaTime, 0);
        rb.MoveRotation(rb.rotation * turnRot);

       
        if (Input.GetKey(KeyCode.Space))
        {
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, Vector3.zero, brakeForce * Time.fixedDeltaTime);
        }

        // ลงรถ
        if (Input.GetKeyDown(KeyCode.F))
            ExitMotorcycle();
        
        Vector3 flatUp = Vector3.up;
        Quaternion keepUpright = Quaternion.FromToRotation(transform.up, flatUp) * rb.rotation;
        rb.MoveRotation(Quaternion.Lerp(rb.rotation, keepUpright, 10f * Time.fixedDeltaTime));

    }

    public void EnterMotorcycle(GameObject player)
    {
        currentPlayer = player;
        player.GetComponent<PlayerInteraction>().isRiding = true;

        player.GetComponent<PlayerMovement>().enabled = false;
        player.GetComponent<CharacterController>().enabled = false;

        foreach (var r in player.GetComponentsInChildren<Renderer>())
            r.enabled = false;

        Drive.SetActive(true);
        isDriving = true;

        var carry = player.GetComponent<PlayerCarry>();
        if (carry != null)
        {
            carry.PlaceOnBike();
        }

        if (camaraSwitch != null)
            camaraSwitch.SetTarget(motorcycleCamTarget);
    }

    void ExitMotorcycle()
    {
        if (currentPlayer == null) return;

        var carry = currentPlayer.GetComponent<PlayerCarry>();
        if (carry != null)
        {
            carry.TakeOffBike();
        }

        Drive.SetActive(false);

        currentPlayer.transform.position = exitPoint.position;
        currentPlayer.transform.rotation = exitPoint.rotation;

        currentPlayer.GetComponent<PlayerInteraction>().isRiding = false;
        currentPlayer.GetComponent<PlayerMovement>().enabled = true;
        currentPlayer.GetComponent<CharacterController>().enabled = true;

        foreach (var r in currentPlayer.GetComponentsInChildren<Renderer>())
            r.enabled = true;

        isDriving = false;

        Vector3 pushDir = (currentPlayer.transform.position - transform.position).normalized;
        currentPlayer.transform.position += pushDir * 0.5f;

        if (camaraSwitch != null)
            camaraSwitch.SetTarget(currentPlayer.transform);
    }
}

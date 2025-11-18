using UnityEngine;

public class BikeController : MonoBehaviour
{
    
    public float moveSpeed = 10f;
    public float boostSpeed = 16f;
    public float turnSpeed = 120f;
    public float brakeForce = 12f;

   
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

    }

    void Update()
    {
        if (!isDriving) return;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        bool boosting = Input.GetKey(KeyCode.LeftShift);
        float currentSpeed = boosting ? boostSpeed : moveSpeed;

        rb.MovePosition(rb.position + transform.forward * v * currentSpeed * Time.deltaTime);

        rb.MoveRotation(rb.rotation * Quaternion.Euler(0, h * turnSpeed * Time.deltaTime, 0));

        if (Input.GetKey(KeyCode.Space))
            transform.position -= transform.forward * brakeForce * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.F))
            ExitMotorcycle();
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

        
        if (camaraSwitch != null)
            camaraSwitch.SetTarget(motorcycleCamTarget);

        
    }

    void ExitMotorcycle()
    {
        if (currentPlayer == null) return;
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

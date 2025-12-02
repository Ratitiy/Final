using UnityEngine;

public class car : MonoBehaviour
{
    
    public Transform[] waypoints; 
    public float speed = 5f;     
    public float turnSpeed = 5f;  

    private int currentIndex = 0;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;  
    }
    void FixedUpdate()
    {
        
        if (waypoints.Length == 0) return;

        
        Transform target = waypoints[currentIndex];


        Vector3 nextPos = Vector3.MoveTowards(rb.position, target.position, speed * Time.fixedDeltaTime);

        rb.MovePosition(nextPos);


        Vector3 direction = target.position - transform.position;
        if (direction != Vector3.zero)
        {
            Quaternion lookRot = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, turnSpeed * Time.fixedDeltaTime);
        }


        if (Vector3.Distance(transform.position, target.position) < 0.2f)
        {
            currentIndex++;
            if (currentIndex >= waypoints.Length) currentIndex = 0;
        }
    }
}
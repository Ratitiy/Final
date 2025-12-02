using UnityEngine;

public class car : MonoBehaviour
{
    
    public Transform[] waypoints; 
    public float speed = 5f;     
    public float turnSpeed = 5f;  

    private int currentIndex = 0; 

    void Update()
    {
        
        if (waypoints.Length == 0) return;

        
        Transform target = waypoints[currentIndex];

       
        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            speed * Time.deltaTime
        );

        
        Vector3 direction = target.position - transform.position;
        if (direction != Vector3.zero)
        {
            Quaternion lookRot = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, turnSpeed * Time.deltaTime);
        }

        
        if (Vector3.Distance(transform.position, target.position) < 0.2f)
        {
            currentIndex++; 

            
            if (currentIndex >= waypoints.Length)
            {
                currentIndex = 0;
            }
        }
    }
}
using UnityEngine;

public class RatMove : MonoBehaviour
{
    public Transform target;
    public float speed = 4f;
    public float stopDistance = 0.2f;

    [HideInInspector] public Vector3 offset;

    void Start()
    {
        
        speed += Random.Range(-0.5f, 0.5f);
    }

    void Update()
    {
        if (target == null) return;

        Vector3 finalDestination = target.position + offset;

        
        transform.position = Vector3.MoveTowards(transform.position, finalDestination, speed * Time.deltaTime);

       
        Vector3 direction = finalDestination - transform.position;
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
        }

       
        if (Vector3.Distance(transform.position, finalDestination) < stopDistance)
        {
            Destroy(gameObject);
        }
    }
}
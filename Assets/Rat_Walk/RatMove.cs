using UnityEngine;

public class RatMove : MonoBehaviour
{
    public Transform target;
    public float speed = 5f;
    public float stopDistance = 0.1f;

    void Update()
    {
        if (target == null) return;

        
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

       
        if (Vector3.Distance(transform.position, target.position) < stopDistance)
        {
            Destroy(gameObject);
        }
    }
}

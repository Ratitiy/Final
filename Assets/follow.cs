using UnityEngine;

public class follow : MonoBehaviour
{
    public Transform target; 
    public Vector3 offset = new Vector3(0, 30f, 0);

    void LateUpdate()
    {
        if (target != null)
        {
            
            transform.position = target.position + offset;

            
            float targetYRotation = target.eulerAngles.y;

            
            transform.rotation = Quaternion.Euler(0, targetYRotation, 0);
        }
    }
}

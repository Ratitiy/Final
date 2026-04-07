using UnityEngine;

public class MinimapFollow : MonoBehaviour
{
    public Transform player; 

    void LateUpdate()
    {
        if (player == null) return;

        
        Vector3 newPosition = player.position;
        newPosition.y = transform.position.y;
        transform.position = newPosition;

        
        //transform.rotation = Quaternion.Euler(90f, player.eulerAngles.y, 0f);
    }
}

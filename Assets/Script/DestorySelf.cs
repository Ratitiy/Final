using UnityEngine;

public class DestorySelf : MonoBehaviour
{
    [Header("Life Settings")]
    public float lifeTime = 5f; 

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }
}
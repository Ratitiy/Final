using UnityEngine;

public class Maker : MonoBehaviour
{
    private Transform targetransformt;
    private RectTransform marker;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        marker = GetComponent<RectTransform>();
    }
    public void Target(Transform target)
    {
        targetransformt = target;
        gameObject.SetActive(target != null);
    }

    // Update is called once per frame
    void Update()
    {
        if (targetransformt == null)
        {
            gameObject.SetActive(false);    
            return;
            

        }
        Vector3 S = Camera.main.WorldToScreenPoint(targetransformt.position);

        marker.position = S;
    }
}

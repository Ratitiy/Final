using UnityEngine;
using UnityEngine.UI; // จำเป็นสำหรับการคุม Image

public class Maker : MonoBehaviour
{
    public Transform target;          
    public Vector3 offset = new Vector3(0, 2.5f, 0);

    public Image markerIcon;          

    private Camera mainCam;

    void Start()
    {
        mainCam = Camera.main; 

        gameObject.SetActive(false);
    }

    void LateUpdate()
    {
        if (target == null)
        {
            markerIcon.enabled = false;
            return;
        }
        Vector3 targetPos = target.position + offset;

        Vector3 screenPos = mainCam.WorldToScreenPoint(targetPos);

        if (screenPos.z < 0)
        {
            markerIcon.enabled = false;
        }
        else
        {
            markerIcon.enabled = true;
            transform.position = screenPos;
        }
    }

    public void Target(Transform newTarget)
    {
        target = newTarget;
        gameObject.SetActive(true); 
        markerIcon.enabled = true;
    }

    public void SetDisable()
    {
        target = null;
        gameObject.SetActive(false);
    }
}
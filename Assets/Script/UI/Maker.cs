using UnityEngine;
using UnityEngine.Rendering;

public class Maker : MonoBehaviour
{
    private RectTransform marker;
    public float height = 10f;
    private Transform npc;

    void Start()
    {
        marker = GetComponent<RectTransform>();
        
    }
    public void Target(Transform target)
    {
        transform.parent = target;
        transform.localPosition = new Vector3(0f, height, 0f);
        //targetransformt = target;
        transform.localRotation = Quaternion.identity;
        gameObject.SetActive(true);
    }

    void SetDisable()
    {
        npc = null;
        transform.parent = null;
        gameObject.SetActive(false);
    }
}

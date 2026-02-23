using UnityEngine;
using TMPro;

public class DeliveryMarker : MonoBehaviour
{
    public Transform player;
    public TextMeshProUGUI distanceText;

    void Update()
    {
        if (DeliveryManager.Instance.currentTarget == null)
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);

        Vector3 dir = DeliveryManager.Instance.currentTarget.position - player.position;
        dir.y = 0;

        transform.rotation = Quaternion.LookRotation(dir);

        float dist = Vector3.Distance(player.position, DeliveryManager.Instance.currentTarget.position);
        distanceText.text = Mathf.RoundToInt(dist) + " m";
    }
}
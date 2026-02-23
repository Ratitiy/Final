using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DeliveryMarker : MonoBehaviour
{
    public Transform player;
    public Transform cameraTransform;
    public TextMeshProUGUI distanceText;

    Image arrowImage;

    void Start()
    {
        arrowImage = GetComponent<Image>();
    }

    void Update()
    {
        if (DeliveryManager.Instance == null ||
            !DeliveryManager.Instance.hasActiveDelivery ||
            DeliveryManager.Instance.currentTarget == null)
        {
            arrowImage.enabled = false;
            distanceText.enabled = false;
            return;
        }

        arrowImage.enabled = true;
        distanceText.enabled = true;

        Vector3 targetDir =
            DeliveryManager.Instance.currentTarget.position - player.position;
        targetDir.y = 0;

        float angle = Vector3.SignedAngle(
            cameraTransform.forward,
            targetDir,
            Vector3.up
        );

        transform.localRotation = Quaternion.Euler(0, 0, -angle);

        float dist = Vector3.Distance(
            player.position,
            DeliveryManager.Instance.currentTarget.position
        );

        distanceText.text = Mathf.RoundToInt(dist) + " m";
    }
}
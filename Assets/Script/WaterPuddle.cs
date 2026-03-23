using UnityEngine;

public class WaterPuddle : MonoBehaviour
{
    [Header("Puddle Settings")]
    public float slipDuration = 3f;

    private void OnTriggerEnter(Collider other)
    {
        MotocycleV2 bike = other.GetComponent<MotocycleV2>();
        if (bike == null)
        {
            bike = other.GetComponentInParent<MotocycleV2>();
        }
        if (bike == null && other.transform.root != null)
        {
            bike = other.transform.root.GetComponentInChildren<MotocycleV2>();
        }
        if (bike != null && !bike.isControlsInverted)
        {
            bike.TriggerSlipEffect(slipDuration);
            Destroy(gameObject);
        }
    }
}
using UnityEngine;

public class RamenLogic : MonoBehaviour
{
    [Header("Spill Settings")]
    public float spillThreshold = 0.15f;    
    public float qualityDrainRate = 15f;
    public ParticleSystem spillParticle;

    [Header("Sensitivity")]
    public float moveSensitivity = 30f;     
    public float turnSensitivity = 20f;     

    [Header("Status")]
    public float ramenQuality = 100f;

    private Vector3 lastPos;
    private Quaternion lastRot;

    void Start()
    {
        lastPos = transform.position;
        lastRot = transform.rotation;
    }

    void LateUpdate()
    {
        
        float distance = Vector3.Distance(transform.position, lastPos);
        float moveForce = distance * moveSensitivity;

      
        float angle = Quaternion.Angle(transform.rotation, lastRot);
        float turnForce = angle * (turnSensitivity * 0.01f);

        float totalForce = moveForce + turnForce;

        if (totalForce > 0.0001f)
        {
            string color = totalForce > spillThreshold ? "red" : "yellow";
            Debug.Log($"<color={color}>RAMEN FORCE: {totalForce:F4}</color>");
        }

        if (totalForce > spillThreshold)
        {
            Spill();
        }

        lastPos = transform.position;
        lastRot = transform.rotation;
    }

    void Spill()
    {
        if (ramenQuality <= 0) return;

        if (spillParticle != null && !spillParticle.isPlaying)
        {
            spillParticle.Play();
        }

        ramenQuality -= qualityDrainRate * Time.deltaTime;
        ramenQuality = Mathf.Max(ramenQuality, 0);
    }
}
using UnityEngine;

public class RamenLogic : MonoBehaviour
{
    [Header("Spill Settings")]
    public float qualityDrainRate = 15f;
    public float impactDamage = 10f;
    public ParticleSystem spillParticle;

    [Header("Status")]
    public float ramenQuality = 100f;
    public bool isOnVehicle = false;

    void Update()
    {
        if (!isOnVehicle)
        {
            StopSpill();
            return;
        }

        
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if (Mathf.Abs(h) > 0.2f && Mathf.Abs(v) > 0.1f)
        {
            StartSpill();
        }
        else
        {
            StopSpill();
        }
    }

   
    private void OnTriggerEnter(Collider other)
    {
        if (isOnVehicle && other.CompareTag("Obstacle"))
        {
            ramenQuality -= impactDamage;
            ramenQuality = Mathf.Max(ramenQuality, 0);
            Debug.Log("ชนวัตถุ! คุณภาพลดเหลือ: " + ramenQuality);

            if (spillParticle != null) spillParticle.Play();
        }
    }

    void StartSpill()
    {
        if (ramenQuality <= 0) return;
        if (spillParticle != null && !spillParticle.isPlaying) spillParticle.Play();
        ramenQuality -= qualityDrainRate * Time.deltaTime;
    }

    void StopSpill()
    {
        if (spillParticle != null && spillParticle.isPlaying) spillParticle.Stop();
    }
}
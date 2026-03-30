using UnityEngine;
using TMPro; 
using UnityEngine.UI; 

public class RamenLogic : MonoBehaviour
{
    [Header("Settings")]
    public float qualityDrainRate = 15f;
    public float impactDamage = 10f;
    public ParticleSystem spillParticle;

    [Header("UI Settings")]
    public TextMeshProUGUI qualityText; 
    public string prefix = "Ramen Quality: ";

    [Header("Status")]
    public float ramenQuality = 100f;
    public bool isOnVehicle = false;

    void Update()
    {
        
        UpdateUI();

        if (!isOnVehicle) { StopSpill(); return; }

<<<<<<< Updated upstream
        
=======
>>>>>>> Stashed changes
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if (Mathf.Abs(h) > 0.2f && Mathf.Abs(v) > 0.1f) StartSpill();
        else StopSpill();
    }

    void UpdateUI()
    {
        if (qualityText != null)
        {
           
            qualityText.text = prefix + ramenQuality.ToString("F0") + "%";

           
            if (ramenQuality > 70) qualityText.color = Color.green;
            else if (ramenQuality > 30) qualityText.color = Color.yellow;
            else qualityText.color = Color.red;
        }
    }

<<<<<<< Updated upstream
   
    private void OnTriggerEnter(Collider other)
=======
    public void TakeImpactDamage()
>>>>>>> Stashed changes
    {
        if (!isOnVehicle) return;
        ramenQuality -= impactDamage;
        ramenQuality = Mathf.Max(ramenQuality, 0);
        if (spillParticle != null) spillParticle.Play();
    }

    void StartSpill()
    {
        if (ramenQuality <= 0) return;
        if (spillParticle != null && !spillParticle.isPlaying) spillParticle.Play();
        ramenQuality -= qualityDrainRate * Time.deltaTime;
        ramenQuality = Mathf.Max(ramenQuality, 0);
    }

    void StopSpill()
    {
        if (spillParticle != null && spillParticle.isPlaying) spillParticle.Stop();
    }
}
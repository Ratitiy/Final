using UnityEngine;

public class BikeCollisionDetector : MonoBehaviour
{
    private BikeCargo bikeCargo;

    void Start()
    {
        
        bikeCargo = GetComponent<BikeCargo>();
    }

    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            SendDamageToRamen();
        }
    }

   
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            SendDamageToRamen();
        }
    }

    void SendDamageToRamen()
    {
      
        if (bikeCargo != null && bikeCargo.storedItem != null)
        {
            RamenLogic ramen = bikeCargo.storedItem.GetComponent<RamenLogic>();
            if (ramen != null)
            {
                ramen.TakeImpactDamage();
            }
        }
    }
}
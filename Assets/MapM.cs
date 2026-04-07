using UnityEngine;

public class MapM : MonoBehaviour
{
    public GameObject largeMapPanel;
    private bool isMapOpen = false;

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.M))
        {
            ToggleMap();
        }
    }

    void ToggleMap()
    {
        isMapOpen = !isMapOpen; 
        largeMapPanel.SetActive(isMapOpen); 

        if (isMapOpen)
        {
            Time.timeScale = 0.2f; 
        }
        else
        {
            Time.timeScale = 1f; 
        }
    }
}

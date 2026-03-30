using UnityEngine;

public class UpgradeNPC : MonoBehaviour
{
    public GameObject upgradeCanvas;

    
    public void Interact()
    {
        OpenUpgradeMenu();
    }

    public void OpenUpgradeMenu()
    {
        upgradeCanvas.SetActive(true);

        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
    }

    public void CloseUpgradeMenu()
    {
        upgradeCanvas.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
    }
}
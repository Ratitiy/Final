using UnityEngine;
using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour
{
    public GameObject panel;

    GameObject playerObj;   
    BikeController bike;

    public Text moneyText;
    public int playerMoney = 500;

    public int speedCost = 200;
    public int accelCost = 150;
    public int turnCost = 150;

    public void OpenShop(GameObject player, BikeController bikeController)
    {
        playerObj = player;          
        bike = bikeController;

        panel.SetActive(true);
        UpdateMoneyUI();

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        playerObj.GetComponent<PlayerMovement>().uiOpened = true;

        
        playerObj.GetComponent<PlayerMovement>().enabled = false;
        playerObj.GetComponent<CharacterController>().enabled = false;
    }

    public void CloseShop()
    {
        Debug.Log("CloseShop CALLED");

        if (playerObj == null)
            Debug.Log("playerObj IS NULL!!!");
        
        panel.SetActive(false);

        

        playerObj.GetComponent<PlayerMovement>().uiOpened = false;

        if (playerObj != null)
        {
            playerObj.GetComponent<PlayerMovement>().enabled = true;
            playerObj.GetComponent<CharacterController>().enabled = true;
        }
        
    }

    public void UpgradeSpeed()
    {
        if (playerMoney >= speedCost)
        {
            bike.moveSpeed += 1f;
            playerMoney -= speedCost;
            UpdateMoneyUI();
        }
    }

    public void UpgradeAcceleration()
    {
        if (playerMoney >= accelCost)
        {
            bike.boostSpeed += 3f;
            playerMoney -= accelCost;
            UpdateMoneyUI();
        }
    }

    public void UpgradeTurn()
    {
        if (playerMoney >= turnCost)
        {
            bike.turnSpeed += 5f;
            playerMoney -= turnCost;
            UpdateMoneyUI();
        }
    }

    public void UpdateMoneyUI()
    {
        moneyText.text = "Money: " + playerMoney;
    }
}

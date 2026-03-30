using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    public MotocycleV2 motor;
    public int upgradeCost = 100;

    [Header("UI Status Text")]
    public TextMeshProUGUI statusText;

    [Header("Buttons")]
    public Button steeringButton;

    void Update()
    {
        if (motor == null) return;

        // แสดงค่าพลังและราคาปัจจุบัน
        statusText.text = $"[ Current Stats ]\n" +
                          $"Max Speed: {motor.maxSpeedKmh:0} KM/H\n" +
                          $"Accel (Torque): {motor.motorTorque:0}\n" +
                          $"Steering: {motor.steeringAngle:0} / 25\n" +
                          $"Next Upgrade Cost: $ {upgradeCost}"; // โชว์ราคาด้วยจะได้รู้

        if (motor.steeringAngle >= 25f)
        {
            steeringButton.interactable = false;
            steeringButton.GetComponentInChildren<TextMeshProUGUI>().text = "STEERING MAX";
        }

        // --- ลบ upgradeCost = ... ออกจากตรงนี้เด็ดขาด! ---
    }

    public void BuyMaxSpeed()
    {
        if (CanAfford())
        {
            MoneyManager.Instance.AddMoney(-upgradeCost);
            motor.UpgradeMaxSpeed(5f);
            IncreaseCost(); // ซื้อสำเร็จค่อยเพิ่มราคา
        }
    }

    public void BuyAcceleration()
    {
        if (CanAfford())
        {
            MoneyManager.Instance.AddMoney(-upgradeCost);
            motor.UpgradeAcceleration(200f);
            IncreaseCost(); // ซื้อสำเร็จค่อยเพิ่มราคา
        }
    }

    public void BuySteering()
    {
        if (CanAfford() && motor.steeringAngle < 25f)
        {
            MoneyManager.Instance.AddMoney(-upgradeCost);
            motor.UpgradeSteering(2f);
            IncreaseCost(); // ซื้อสำเร็จค่อยเพิ่มราคา
        }
    }

    // ฟังก์ชันช่วยเพิ่มราคา (เรียกใช้หลังจากซื้อสำเร็จ)
    void IncreaseCost()
    {
        upgradeCost = (int)(upgradeCost * 1.2f);
    }

    bool CanAfford()
    {
        if (MoneyManager.Instance == null) return false;
        return MoneyManager.Instance.money >= upgradeCost;
    }
}
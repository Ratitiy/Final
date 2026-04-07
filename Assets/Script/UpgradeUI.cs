using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class UpgradeManager : MonoBehaviour
{
    public MotocycleV2 motor;
    public int upgradeCost = 100;

    [Header("Upgrade Settings")]
    public int maxUpgradeLevel = 5;
    private int currentUpgradeCount = 0;

    [Header("UI Progress Bar")]
    public Image upgradeProgressBar; // ลาก Image ที่เป็นหลอด Fill มาใส่ที่นี่
    public TextMeshProUGUI statusText;

    // ฟังก์ชันซื้อ (เรียกจาก Button เหมือนเดิม)
    public void BuyMaxSpeed()
    {
        if (currentUpgradeCount < maxUpgradeLevel && CanAfford())
        {
            MoneyManager.Instance.AddMoney(-upgradeCost);
            motor.UpgradeMaxSpeed(5f);
            UpdateProgress();
        }
    }

    // ตัวอย่างของ Acceleration (ทำเหมือนกันทุกปุ่ม)
    public void BuyAcceleration()
    {
        if (currentUpgradeCount < maxUpgradeLevel && CanAfford())
        {
            MoneyManager.Instance.AddMoney(-upgradeCost);
            motor.UpgradeAcceleration(200f);
            UpdateProgress();
        }
    }

    void UpdateProgress()
    {
        currentUpgradeCount++; // เพิ่มจำนวนครั้ง
        upgradeCost = (int)(upgradeCost * 1.0f); // เพิ่มราคา

        // --- ส่วนการจัดการหลอด UI ---
        if (upgradeProgressBar != null)
        {
            // คำนวณเป็น % (0.0 ถึง 1.0)
            // เช่น อัพ 1 ครั้ง = 1/4 = 0.25 (หลอดขึ้นมา 25%)
            float progress = (float)currentUpgradeCount / maxUpgradeLevel;
            upgradeProgressBar.fillAmount = progress;
        }

        if (statusText != null)
            statusText.text = $"Level: {currentUpgradeCount}/{maxUpgradeLevel}";
    }

    bool CanAfford()
    {
        return MoneyManager.Instance != null && MoneyManager.Instance.money >= upgradeCost;
    }
}
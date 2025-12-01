using UnityEngine;

public class Dorr : MonoBehaviour, IInteractable
{
    public string GetPrompt() => isEnterHouse ? "เข้าบ้าน (FPS)" : "ออกจากบ้าน (TPS)";

    [Header("Settings")]
    public Transform ExitPoint;
    public bool isEnterHouse = false; 

    [Header("References")]
    public CamaraSwitch camSwitch;

    private void Start()
    {
        if (camSwitch == null)
            camSwitch = FindAnyObjectByType<CamaraSwitch>();
    }

    public void Interact(GameObject player)
    {
        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

        player.transform.position = ExitPoint.position;
        player.transform.rotation = ExitPoint.rotation;

        if (cc != null) cc.enabled = true;

        HandleCameraMode(player);
    }

    void HandleCameraMode(GameObject player)
    {
        if (camSwitch == null) return;

        if (isEnterHouse)
        {
            Transform fpsPos = player.transform.Find("FPSPosition");

            if (fpsPos == null)
            {
                fpsPos = player.transform;
                Debug.LogWarning("ไม่เจอ FPSPosition ใน Player");
            }

            camSwitch.SwitchToFPS(fpsPos);
        }
        else
        {
            camSwitch.SwitchToTPS(player.transform);
        }
    }
}
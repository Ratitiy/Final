using System.Collections;
using UnityEngine;

public class MotorcycleSeat : MonoBehaviour
{
    public Transform seatPoint;
    public Motorcycle motorcycle;

    [Header("Cinemachine Cameras")]
    public GameObject playerCamera; 
    public GameObject bikeCamera;   

    GameObject currentPlayer;
    bool isMounting = false;

    void Interact(GameObject player)
    {
        if (currentPlayer == null && !isMounting)
        {
            StartCoroutine(MountRoutine(player));
        }
    }

    void Update()
    {
        if (currentPlayer != null && !isMounting && Input.GetKeyDown(KeyCode.F))
        {
            Dismount();
        }
    }

    IEnumerator MountRoutine(GameObject player)
    {
        isMounting = true;
        currentPlayer = player;

        // ปิดระบบต่างๆ ของผู้เล่น
        if (player.TryGetComponent(out PlayerInteract interact)) interact.enabled = false;
        if (player.TryGetComponent(out Move move)) move.enabled = false;
        if (player.TryGetComponent(out CharacterController cc)) cc.enabled = false;
        foreach (var col in player.GetComponentsInChildren<Collider>()) col.enabled = false;
        var mesh = player.GetComponentInChildren<SkinnedMeshRenderer>();
        if (mesh) mesh.enabled = false;

        player.transform.SetParent(seatPoint);
        player.transform.localPosition = Vector3.zero;
        player.transform.localRotation = Quaternion.identity;

        motorcycle.enabled = true;

        // --- สลับกล้อง Cinemachine ---
        if (playerCamera != null) playerCamera.SetActive(false); // ปิดกล้องคน
        if (bikeCamera != null) bikeCamera.SetActive(true);      // เปิดกล้องรถ
        // ---------------------------

        yield return new WaitForSeconds(0.5f);
        isMounting = false;
    }

    void Dismount()
    {
        motorcycle.enabled = false;

        currentPlayer.transform.SetParent(null);
        Vector3 exitPoint = currentPlayer.transform.position + (currentPlayer.transform.right * 1.2f) + (Vector3.up * 0.1f);
        currentPlayer.transform.position = exitPoint;
        currentPlayer.transform.rotation = Quaternion.Euler(0, currentPlayer.transform.eulerAngles.y, 0);

        // คืนค่าระบบผู้เล่น
        if (currentPlayer.TryGetComponent(out CharacterController cc)) cc.enabled = true;
        if (currentPlayer.TryGetComponent(out Move move)) move.enabled = true;
        foreach (var col in currentPlayer.GetComponentsInChildren<Collider>()) col.enabled = true;
        var mesh = currentPlayer.GetComponentInChildren<SkinnedMeshRenderer>();
        if (mesh) mesh.enabled = true;
        if (currentPlayer.TryGetComponent(out PlayerInteract interact)) interact.enabled = true;

        // --- สลับกล้องคืน ---
        if (bikeCamera != null) bikeCamera.SetActive(false);     // ปิดกล้องรถ
        if (playerCamera != null) playerCamera.SetActive(true);  // เปิดกล้องคน
        // ------------------

        currentPlayer = null;
    }
}
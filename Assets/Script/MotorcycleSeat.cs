using UnityEngine;

public class MotorcycleSeat : MonoBehaviour
{
    public Transform seatPoint;
    public Motorcycle motorcycle;

    GameObject currentPlayer;

    void Interact(GameObject player)
    {
        if (currentPlayer == null)
            Mount(player);
    }

    void Update()
    {
        if (currentPlayer != null && Input.GetKeyDown(KeyCode.F))
        {
            Dismount();
        }
    }

    void Mount(GameObject player)
    {
        currentPlayer = player;

        // 👉 ผูก player กับที่นั่ง
        player.transform.SetParent(seatPoint);
        player.transform.localPosition = Vector3.zero;
        player.transform.localRotation = Quaternion.identity;

        // 👉 ปิด movement ของ player
        if (player.TryGetComponent(out Move move))
            move.enabled = false;

        if (player.TryGetComponent(out CharacterController cc))
            cc.enabled = false;

        // 👉 ปิด collider player (กันชนรถ)
        foreach (var col in player.GetComponentsInChildren<Collider>())
            col.enabled = false;

        // 👉 เปิดระบบรถ
        motorcycle.enabled = true;

        // 👉 ซ่อนโมเดล (ถ้าต้องการ)
        var mesh = player.GetComponentInChildren<SkinnedMeshRenderer>();
        if (mesh) mesh.enabled = false;
    }

    void Dismount()
    {
        motorcycle.enabled = false;

        // 👉 เอา player ออกจากรถ
        currentPlayer.transform.SetParent(null);
        currentPlayer.transform.position += currentPlayer.transform.right * 1.2f;

        if (currentPlayer.TryGetComponent(out CharacterController cc))
            cc.enabled = true;

        if (currentPlayer.TryGetComponent(out Move move))
            move.enabled = true;

        foreach (var col in currentPlayer.GetComponentsInChildren<Collider>())
            col.enabled = true;

        var mesh = currentPlayer.GetComponentInChildren<SkinnedMeshRenderer>();
        if (mesh) mesh.enabled = true;

        currentPlayer = null;
    }
}

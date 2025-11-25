
using UnityEngine;

public class Dorr : MonoBehaviour, IInteractable
{
    public string GetPrompt() => "[E]Enter";

    public Transform ExitPoint;

    public void Interact(GameObject player)
    {
        Debug.Log("InteractDoor");

        // 1. หา Component CharacterController ของผู้เล่น
        CharacterController cc = player.GetComponent<CharacterController>();

        // 2. ถ้ามี ต้องสั่งปิด (Disable) ก่อน ไม่งั้นย้ายตำแหน่งไม่ได้
        if (cc != null)
        {
            cc.enabled = false;
        }

        // 3. ย้ายตำแหน่งไปยังจุดออก
        player.transform.position = ExitPoint.position;

        // 4. สั่งเปิด (Enable) กลับคืนเพื่อให้เดินต่อได้
        if (cc != null)
        {
            cc.enabled = true;
        }
    }

    void Start()
    {

    }

    void Update()
    {

    }
}
using UnityEngine;

public interface IInteractable
{
    string GetPrompt();          // ข้อความ
    void Interact(GameObject interactor); // Press E
    void OnFocus();              // ไฮไลต์ตอนเล็ง (ถ้าอยาก)
    void OnLoseFocus();          // ปิดไฮไลต์
}

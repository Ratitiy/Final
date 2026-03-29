using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogManager : MonoBehaviour
{
    public GameObject dialogUI;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogText;

    // เพิ่มตัวแปรนี้เข้ามาเพื่อบอกว่ากล่องข้อความเปิดอยู่ไหม
    public bool isDialogActive = false;

    private Queue<string> sentences;

    void Start()
    {
        sentences = new Queue<string>();
        dialogUI.SetActive(false);
        isDialogActive = false; // เริ่มเกมมายังไม่ได้คุย
    }

    public void StartDialog(Dialog dialog)
    {
        isDialogActive = true; // เปลี่ยนสถานะเป็นกำลังคุยอยู่
        dialogUI.SetActive(true);

        nameText.text = dialog.name;
        sentences.Clear();

        foreach (string sentence in dialog.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialog();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(0.02f);
        }
    }

    // เปลี่ยนจาก private เป็น public เพื่อให้ปุ่ม E เรียกใช้ตอนจบได้
    public void EndDialog()
    {
        isDialogActive = false; // เปลี่ยนสถานะเป็นคุยจบแล้ว
        dialogUI.SetActive(false);
        Debug.Log("จบการสนทนา");
    }
}
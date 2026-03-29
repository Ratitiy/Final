using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogManager : MonoBehaviour
{
    // 1. เพิ่มตัวแปรสำหรับลากกล่อง UI มาใส่
    public GameObject dialogUI;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogText;

    public bool isDialogActive = false;
    private Queue<DialogLine> linesQueue;

    void Start()
    {
        linesQueue = new Queue<DialogLine>();

        // 2. สั่งปิด UI ไว้ก่อนเสมอตอนเริ่มเกม
        dialogUI.SetActive(false);
        isDialogActive = false;
    }

    public void StartDialog(Dialog dialog)
    {
        isDialogActive = true;

        // 3. สั่งเปิด UI ขึ้นมาเมื่อเริ่มบทสนทนา
        dialogUI.SetActive(true);

        linesQueue.Clear();

        foreach (DialogLine line in dialog.lines)
        {
            linesQueue.Enqueue(line);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (linesQueue.Count == 0)
        {
            EndDialog();
            return;
        }

        DialogLine currentLine = linesQueue.Dequeue();
        nameText.text = currentLine.name;

        StopAllCoroutines();
        StartCoroutine(TypeSentence(currentLine.sentence));
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

    public void EndDialog()
    {
        isDialogActive = false;

        // 4. สั่งปิด UI เมื่อคุยจบ
        dialogUI.SetActive(false);
        Debug.Log("จบการสนทนา");
    }
}
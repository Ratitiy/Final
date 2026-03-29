using UnityEngine;

// เก็บข้อมูล 1 ประโยค (มีชื่อคนพูด + ข้อความ)
[System.Serializable]
public class DialogLine
{
    public string name;

    [TextArea(3, 10)]
    public string sentence;
}

[System.Serializable]
public class Dialog
{
    // เก็บเป็นชุดข้อความเรียงต่อกัน
    public DialogLine[] lines;
}
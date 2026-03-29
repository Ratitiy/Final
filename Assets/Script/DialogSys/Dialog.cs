using UnityEngine;

[System.Serializable]
public class Dialog
{
    public string name; // ชื่อคนพูด

    [TextArea(3, 10)]
    public string[] sentences; // ประโยคต่างๆ ที่จะพูด
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelScene : BaseScene
{
    // Start is called before the first frame update
    public TMPro.TextMeshProUGUI textUI;
    private void Awake()
    {
        MessageCenter.Instance.Register(MessageCenter.MessageType.ActionPoint, Point);
    }
    private void OnDestroy()
    {
        MessageCenter.Instance.Remove(MessageCenter.MessageType.ActionPoint, Point);
    }
    public void ReturnToStartScene()
    {
        UIManager.Instance().ShowUIForm("StartPanel");
    }

    public void Point(object obj)
    {
        int number = (int)obj;
        string point = number.ToString() + "/3";
        textUI.text = point;
    }
}

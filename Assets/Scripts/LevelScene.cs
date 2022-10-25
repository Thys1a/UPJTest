using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelScene : BaseScene
{
    // Start is called before the first frame update
    public List<Image> images;
    private int pre = 3, total = 3;

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
        if (number == pre)
        {
            return;
        }
        else
        {
            for(int i = 0; i < total;i++)
            {
                if (i < total - number) images[i].gameObject.SetActive(false);
                else images[i].gameObject.SetActive(true);
            }
            pre = number;
        }
    }
}

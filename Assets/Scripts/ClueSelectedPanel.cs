using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClueSelectedPanel : BaseUIForm
{
    
    void Start()
    {

    }

    /// <summary>
    /// ѡ�������ı���������д�ж�˳���
    /// </summary>
    public void BelieveText()
    {
        MessageCenter.Instance.Send(MessageCenter.MessageType.ClueSelectedType, 0);
        UIManager.Instance().CloseUIForms("ClueSelectedPanel");
    }

    /// <summary>
    /// ѡ��������Ƶ��������д�ж�˳���
    /// </summary>
    public void BelieveAudioSource()
    {
        MessageCenter.Instance.Send(MessageCenter.MessageType.ClueSelectedType, 1);
        UIManager.Instance().CloseUIForms("ClueSelectedPanel");
    }
}

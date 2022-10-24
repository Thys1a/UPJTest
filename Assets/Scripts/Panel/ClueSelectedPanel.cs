using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClueSelectedPanel : BaseUIForm
{
    public TMPro.TextMeshProUGUI text;
    public Text textBtn, audioBtn;
    private ClueEntity clue;

    private void Awake()
    {
        MessageCenter.Instance.Register(MessageCenter.MessageType.ShowClueSelectedPanel, DataPrepare);
    }

    private void OnDestroy()
    {
        MessageCenter.Instance.Register(MessageCenter.MessageType.ShowClueSelectedPanel, DataPrepare);
    }

    private void DataPrepare(object obj)
    {
        clue = (ClueEntity)obj;
        text.text = clue.question;
        textBtn.text = clue.textOption;
        audioBtn.text = clue.audioOption;
        base.Display();
    }
    public override void Display()
    {

    }

    /// <summary>
    /// ѡ�������ı���������д�ж�˳���
    /// </summary>
    public void BelieveText()
    {
        MessageCenter.Instance.Send(MessageCenter.MessageType.ClueSelectedType, 0);
        ClosePanel();
    }

    /// <summary>
    /// ѡ��������Ƶ��������д�ж�˳���
    /// </summary>
    public void BelieveAudioSource()
    {
        MessageCenter.Instance.Send(MessageCenter.MessageType.ClueSelectedType, 1);
        ClosePanel();
    }

    private void ClosePanel()
    {
        UIManager.Instance().CloseUIForms("CluePanel");
        UIManager.Instance().CloseUIForms("ClueSelectedPanel");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StartPanel : BaseUIForm
{
    public AudioSource audio;
    public void OnClick()
    {
        audio.Play();
    }
    
    public void StartGame()
    {
        OnClick();
        MessageCenter.Instance.Send(MessageCenter.MessageType.Archive, true);

    }

    public void LoadGame()
    {
        OnClick();
        MessageCenter.Instance.Send(MessageCenter.MessageType.Archive, false);
    }

    public void ShowStaffPanel()
    {
        OnClick();
        UIManager.Instance().ShowUIForm("StaffPanel");
    }

    public void ExitGame()
    {
        OnClick();
        Application.Quit();
    }
}

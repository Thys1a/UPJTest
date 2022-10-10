using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScene : BaseScene
{
    public void StartGame()
    {
        MessageCenter.Instance.Send(MessageCenter.MessageType.Archive, true);

    }

    public void LoadGame()
    {
        MessageCenter.Instance.Send(MessageCenter.MessageType.Archive, false);
    }

    public void ShowStaffPanel() 
    {
        UIManager.Instance().ShowUIForm("StaffPanel");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}

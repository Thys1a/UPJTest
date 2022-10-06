using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScene : BaseScene
{
    public void StartGame()
    {
        MessageCenter.Instance.Send(MessageCenter.MessageType.EndNormalProcess, null);
    }

    public void LoadGame()
    {

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

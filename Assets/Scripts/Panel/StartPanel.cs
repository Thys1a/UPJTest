using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StartPanel : BaseUIForm
{
    public AudioSource se;
    public GameObject parent;
    float load;

    private void Update()
    {
        load += 2*Time.deltaTime ;
    }

    public void OnClick()
    {
        //se.Play();
    }
    
    public void StartGame()
    {
        OnClick();
        Test.Instance.LoadNextLevel();       
        StartCoroutine(WaitForSecondsRealtime(1.2f, () =>
        {           
            MessageCenter.Instance.Send(MessageCenter.MessageType.Archive, true);            
        }));
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

  
    public IEnumerator WaitForSecondsRealtime(float duration, Action action = null)
    {
        yield return new WaitForSecondsRealtime(duration);
        action?.Invoke();       
    }

    
}

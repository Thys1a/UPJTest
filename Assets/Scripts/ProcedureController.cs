using System;
using System.Collections.Generic;
using UnityEngine;

public class ProcedureController: Singleton<ProcedureController>
{
    List<string> list = null;
    int point;
    private ClickManager logicController;
    private ProcedureEntity procedure;

    void Start()
    {
        MessageCenter.Instance.Register(MessageCenter.MessageType.EndLevel, EndControl);
        MessageCenter.Instance.Register(MessageCenter.MessageType.EndNormalProcess, EndControl);
        MessageCenter.Instance.Register(MessageCenter.MessageType.ClueNumber, SetClueNumber);
        MessageCenter.Instance.Register(MessageCenter.MessageType.SceneSwitching, StartControl);

        this.gameObject.AddComponent<ProcedureEntity>();
        this.gameObject.AddComponent<ClickManager>();
        procedure = this.GetComponent<ProcedureEntity>();procedure.enabled = false;
        logicController = this.GetComponent<ClickManager>();logicController.enabled = false;
        Debug.Log("流程控制器开始工作……");

        //todo：reflect entity
        list = new List<string>();
        list.Add("Start");
        list.Add("Level1");
        list.Add("Level2");
        point = 0;

        procedure.enabled = true;
        PreCheck();
        
    }

    private void SetClueNumber(object number)
    {
        OpenOrCloseLogicControl(true);
        logicController.clueNumber = (int)number;
        
    }

    private void OnDestroy()
    {
        MessageCenter.Instance.Remove(MessageCenter.MessageType.EndLevel, EndControl);
        MessageCenter.Instance.Remove(MessageCenter.MessageType.EndNormalProcess, EndControl);
        MessageCenter.Instance.Remove(MessageCenter.MessageType.ClueNumber, SetClueNumber);
        MessageCenter.Instance.Remove(MessageCenter.MessageType.SceneSwitching, StartControl);
    }
    private void OpenOrCloseLogicControl(bool open)
    {
        if (logicController == null)
        {
            Debug.LogError("未找到logicManger");
            return;
        }
        if (open) {
            logicController.enabled = true;
            Debug.Log("加载logicManger");
        }
        else
        {
            logicController.enabled = false;
            Debug.Log("卸载logicManger");
        }
    }
    private void PreCheck()
    {

        if (procedure == null)
        {
            Debug.LogError("未找到流程实体");
            return;
        }
        if (point < 0 || point > list.Count) 
        { 
            Debug.LogError("流程指针越界。");
            return; 
        }
        if (point == list.Count)
        {
            Debug.Log("所有流程结束。"); 
            return;
        }
        if (SceneMgr.Instance.GetScene().name != list[point]) SceneMgr.Instance.LoadScene(list[point]);
        else StartControl(null);
        if (point < list.Count-1) SceneMgr.Instance.PreLoadScene(list[point + 1]);



    }
    private void StartControl(object obj)
    {
        if (procedure.EnterProcess(list[point]))
        {
            Debug.Log("进入流程……");
            procedure.StartProcess();
        }
        else
        {
            Debug.LogError("流程进入失败，请检查。");
        }
    }
    private void EndControl(object obj)
    {
        OpenOrCloseLogicControl(false);
        Debug.Log("第"+point+"个流程结束。");
        list[point] = null;
        point += 1;
        PreCheck();
    }

}

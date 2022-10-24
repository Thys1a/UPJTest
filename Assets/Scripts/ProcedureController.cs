using System;
using System.Collections.Generic;
using UnityEngine;

public class ProcedureController: Singleton<ProcedureController>
{
    List<string> list = null;
    int point;
    int count;
    private ClickManager logicController;
    private ProcedureEntity procedure;
    Save save;
    List<string[]> data = null;

    void Start()
    {
        MessageCenter.Instance.Register(MessageCenter.MessageType.EndLevel, EndControl);
        MessageCenter.Instance.Register(MessageCenter.MessageType.EndNormalProcess, EndControl);
        MessageCenter.Instance.Register(MessageCenter.MessageType.ClueNumber, SetClueNumber);
        MessageCenter.Instance.Register(MessageCenter.MessageType.SceneSwitching, StartControl);
        MessageCenter.Instance.Register(MessageCenter.MessageType.Archive, CreateOrLoadArchive);
        MessageCenter.Instance.Register(MessageCenter.MessageType.RecordUpdate, OnRecordUpdate);
        MessageCenter.Instance.Register(MessageCenter.MessageType.RecordActionPoint, OnActionpointUpdate);

        this.gameObject.AddComponent<ProcedureEntity>();
        this.gameObject.AddComponent<ClickManager>();
        this.gameObject.AddComponent<CluePool>();
        procedure = this.GetComponent<ProcedureEntity>();procedure.enabled = false;
        logicController = this.GetComponent<ClickManager>();logicController.enabled = false;
        save = new Save();

        //todo：reflect entity
        list = new List<string>();
        list.Add("Start");
        list.Add("Level1");
        list.Add("Level2");
        list.Add("End");
        Debug.Log("流程控制器开始工作……");
        procedure.enabled = true;

        PreCheck();
        
    }


    private void OnDestroy()
    {
        MessageCenter.Instance.Remove(MessageCenter.MessageType.EndLevel, EndControl);
        MessageCenter.Instance.Remove(MessageCenter.MessageType.EndNormalProcess, EndControl);
        MessageCenter.Instance.Remove(MessageCenter.MessageType.ClueNumber, SetClueNumber);
        MessageCenter.Instance.Remove(MessageCenter.MessageType.SceneSwitching, StartControl);
        MessageCenter.Instance.Remove(MessageCenter.MessageType.Archive, CreateOrLoadArchive);
        MessageCenter.Instance.Remove(MessageCenter.MessageType.RecordUpdate, OnRecordUpdate);
        MessageCenter.Instance.Remove(MessageCenter.MessageType.RecordActionPoint, OnActionpointUpdate);
    }


    private void PreCheck(string jump=null)
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
            ReturnToStart();
            return;
        }
        if (SceneMgr.Instance.WaitingNumber()>0) SceneMgr.Instance.LoadScene(jump);
        else StartControl(null);
    }

    private void ReturnToStart()
    {
        point = 0;
        count = 0;
        PreCheck();
    }

    private void StartControl(object obj)
    {

        if (procedure.EnterProcess(list[point]))
        {
            Debug.Log("进入流程……");
            if (!save.isArchiveEmpty())
            {
                save.UpdateData(point, count, SceneMgr.Instance.GetScene().name);
                AutoSave();
            }
            procedure.StartProcess();
        }
        else
        {
            Debug.LogError("流程进入失败，请检查。");
        }
    }

    /// <summary>
    /// 设置线索数量并打开逻辑控制
    /// </summary>
    /// <param name="number"></param>
    private void SetClueNumber(object number)
    {
        OpenOrCloseLogicControl(true);
        logicController.clueNumber = (int)number;
        if (data!=null)
        {
            logicController.SetRecord(data);
            logicController.SetActionpoint(save.GetActionPont());
            data = null;
        }

    }

    /// <summary>
    /// 流程结束，回收资源
    /// </summary>
    /// <param name="obj"></param>
    private void EndControl(object obj)
    {
        if (obj != null) {
            count += (int)obj;
            OpenOrCloseLogicControl(false);
            MessageCenter.Instance.Send(MessageCenter.MessageType.ClueRecycle, null);
        }
        Debug.Log("第" + point + "个流程结束。");
        list[point] = null;
        point += 1;

        
        PreCheck();
    }


    #region utils
    private void OpenOrCloseLogicControl(bool open)
    {
        if (logicController == null)
        {
            Debug.LogError("未找到logicManger");
            return;
        }
        if (open)
        {
            logicController.enabled = true;
            Debug.Log("加载logicManger");
        }
        else
        {
            logicController.enabled = false;
            Debug.Log("卸载logicManger");
        }
    }

    private void CreateOrLoadArchive(object obj)
    {
        bool isCreate = (bool)obj;
        if (isCreate) {
            save.CreateArchive();
            point = 0;
            count = 0;
            if (SceneMgr.Instance.GetScene().name == SysDefine.ScenesName.START) EndControl(null);
            else EndControl(0);
        }
        else
        {
            string scene = save.LoadArchive();
            SceneMgr.Instance.PreLoadScene(scene);
            point = save.GetData()[0];
            count = save.GetData()[1];
            data = save.GetRecord();
            Debug.Log("跳转至存档流程……");
            PreCheck(scene);
        }
        UIManager.Instance().CloseUIForms("StartPanel");
    }

    private void OnRecordUpdate(object obj)
    {
        string[] info = (string[])obj;
        if (info.Length < 3) Debug.LogError("wrong record");
        if (!save.isArchiveEmpty())
        {
            save.UpdateRecord(info[0], int.Parse(info[1]), info[2]);
            AutoSave();
        }
    }
    private void OnActionpointUpdate(object obj)
    {
        if (!save.isArchiveEmpty())
        {
            int actionPoint = (int)obj;
            save.UpdateActionpoint(actionPoint);
            AutoSave();
        }
    }

    private void AutoSave()
    {
        save.SaveData();
    }
    #endregion
}

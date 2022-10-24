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

        //todo��reflect entity
        list = new List<string>();
        list.Add("Start");
        list.Add("Level1");
        list.Add("Level2");
        list.Add("End");
        Debug.Log("���̿�������ʼ��������");
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
            Debug.LogError("δ�ҵ�����ʵ��");
            return;
        }
        if (point < 0 || point > list.Count) 
        { 
            Debug.LogError("����ָ��Խ�硣");
            return; 
        }
        if (point == list.Count)
        {
            Debug.Log("�������̽�����");
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
            Debug.Log("�������̡���");
            if (!save.isArchiveEmpty())
            {
                save.UpdateData(point, count, SceneMgr.Instance.GetScene().name);
                AutoSave();
            }
            procedure.StartProcess();
        }
        else
        {
            Debug.LogError("���̽���ʧ�ܣ����顣");
        }
    }

    /// <summary>
    /// �����������������߼�����
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
    /// ���̽�����������Դ
    /// </summary>
    /// <param name="obj"></param>
    private void EndControl(object obj)
    {
        if (obj != null) {
            count += (int)obj;
            OpenOrCloseLogicControl(false);
            MessageCenter.Instance.Send(MessageCenter.MessageType.ClueRecycle, null);
        }
        Debug.Log("��" + point + "�����̽�����");
        list[point] = null;
        point += 1;

        
        PreCheck();
    }


    #region utils
    private void OpenOrCloseLogicControl(bool open)
    {
        if (logicController == null)
        {
            Debug.LogError("δ�ҵ�logicManger");
            return;
        }
        if (open)
        {
            logicController.enabled = true;
            Debug.Log("����logicManger");
        }
        else
        {
            logicController.enabled = false;
            Debug.Log("ж��logicManger");
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
            Debug.Log("��ת���浵���̡���");
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

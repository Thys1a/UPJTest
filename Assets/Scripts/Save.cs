using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Save
{
    Archive archive;
    string filePath = Application.dataPath + SysDefine.SYS_PATH_DATA;
    public void CreateArchive()
    {
        archive = new Archive(SceneMgr.Instance.GetScene().name);
    }
    public string LoadArchive()
    {
        LoadData();
        return archive.GetScene();
    }

    public void UpdateData(int point, int deadCount, string scene)
    {
        archive.Update(point, deadCount,scene);
        archive.DeleteRecord();

    }

    public void UpdateRecord(string name, int click, string type)
    {
        
        archive.Update(name, click,type);
    }
    public void UpdateActionpoint(int actionPoint)
    {
        archive.Update(actionPoint);
    }
    public bool isArchiveEmpty()
    {
        return archive == null;
    }

    public void SaveData()
    {
        StoreUtil<Archive>.saveData(filePath, archive);
    }
    private void LoadData()
    {
        //判断要读取的游戏文档信息是否存在
        if (File.Exists(filePath))
        {

            archive = StoreUtil<Archive>.loadData(filePath);
            
            archive.SetRecord(StoreUtil<List<Archive.InfoArray>>.loadData(filePath,"_record"));
        }
    }

    public List<string[]> GetRecord()
    {
        
        return archive.GetRecord();
    }

    public int[] GetData()
    {
        return archive.GetData();
    }

    internal int GetActionPont()
    {
        return archive.GetActionpoint();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Archive
{
    [SerializeField]
    string currentScene;
    [SerializeField]
    int point;
    [SerializeField]
    int deadCount;
    [SerializeField]
    List<InfoArray> _record = new List<InfoArray>();
    //[SerializeField]
    List<string[]> record = new List<string[]>();

    public List<InfoArray> GetList()
    {
        return _record;
    }

    public Archive(string currentScene, int point = 0, int deadCount = 0)
    {
        this.currentScene = currentScene;
        this.point = point;
        this.deadCount = deadCount;
    }
    public void Update(int point, int deadCount, string scene)
    {
        this.currentScene = scene;
        this.point = point;
        this.deadCount = deadCount;
    }
    public void Update(string name, int click, string type)
    {
        if (record == null) Debug.LogError("no record");
        string[] info = new string[] { name, type };
        record.Add(info);
        _record.Add(new InfoArray(info));
    }
    public void SetRecord(List<string[]> record)
    {
        this.record = record;
        
    }
    public void SetRecord(List<InfoArray> infos)
    {
        _record = infos;
        record = new List<string[]>();
        foreach (var item in infos)
        {
            record.Add(item.info);
        }

    }
    public void DeleteRecord()
    {
        this.record.Clear();
        this._record.Clear();
    }

    public string GetScene()
    {
        return currentScene;
    }

    public List<string[]> GetRecord()
    {
        List<string[]>  infos = new List<string[]>();
        foreach (var item in record)
        {
            infos.Add(item);
        }
        return infos;
    }

    public int[] GetData()
    {
        return new int[] { point, deadCount };
    }
    [Serializable]
    public class InfoArray
    {
        [SerializeField]
        public string[] info;
        public InfoArray(string[] info)=>this.info = info;
    }
}

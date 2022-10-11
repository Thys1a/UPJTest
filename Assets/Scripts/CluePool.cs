using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.Pool;

public class CluePool:MonoBehaviour
{

    ObjectPool<GameObject> pool;//对象池
    GameObject clueParent;
    XmlNode node;
    public int clueNumber;

    void Start()
    {
        MessageCenter.Instance.Register(MessageCenter.MessageType.ClueGen, ConstructClueList);
        MessageCenter.Instance.Register(MessageCenter.MessageType.ClueRecycle, RecycleClueList);
        pool = new ObjectPool<GameObject>(Create, actionOnGet, actionOnRelease, actionOnDestroy, true, 10, 20);
    }
    private void OnDestroy()
    {
        MessageCenter.Instance.Remove(MessageCenter.MessageType.ClueGen, ConstructClueList);
        MessageCenter.Instance.Remove(MessageCenter.MessageType.ClueRecycle, RecycleClueList);
    }
    private void GonstructClue(XmlNode node,GameObject obj)
    {
        List<XmlNode> inner = XMLUtil.GetChildNodes(node);
        if (inner == null)
        {
            Debug.Log( "Null Clue.");
            return;
        }
        
        ClueEntity clueEntity = obj.GetComponent<ClueEntity>();
        float x = 0, y = 0;
        foreach (XmlNode i in inner)
        {
            string[] item = XMLUtil.GetNameValueOfNode(i);
            switch (item[0])
            {
                case "name": clueEntity.clueName = item[1]; obj.name = item[1]; break;
                case "num": clueEntity.clueNum = int.Parse(item[1]); break;
                case "pre": clueEntity.precursor = int.Parse(item[1]); break;
                case "valid": clueEntity.validBit = bool.Parse(item[1]);
                    if(clueEntity.validBit)
                       clueNumber++;
                    break;
                case "x": x = float.Parse(item[1]); break;
                case "y": y = float.Parse(item[1]); break;
                default:
                    Debug.Log( "clue 生成：遇到不可解析的属性。");
                    break;
            }
        }
        obj.name = clueEntity.clueName;
        obj.transform.position = new Vector3(x, y, obj.transform.position.z);
        obj.SetActive(true);
        obj.GetComponent<Collider2D>().enabled = true;
    }
    private void ConstructClueList(object obj)
    {
        List<XmlNode> clueNodes = (List<XmlNode>)obj;
        clueNumber = 0;
        clueParent = new GameObject(SysDefine.Clue);
        foreach (XmlNode clueNode in clueNodes)
        {
            node = clueNode;
            pool.Get();
            Debug.Log(clueNumber);
            
                   
        }
        MessageCenter.Instance.Send(MessageCenter.MessageType.ClueNumber, clueNumber);
    }
    private void RecycleClueList(object obj)
    {
        if (clueParent == null) return;
        for (int i = 0; i < clueParent.transform.childCount; i++)
        {
            pool.Release(clueParent.transform.GetChild(i).gameObject);
        }
        Destroy(clueParent);
    }

    #region pool
    private void actionOnDestroy(GameObject obj)
    {
        Destroy(obj);
    }

    private void actionOnRelease(GameObject obj)
    {
        obj.SetActive(false);
    }

    private void actionOnGet(GameObject obj)
    {
        obj.transform.parent = clueParent.transform;
        GonstructClue(node, obj);
    }

    private GameObject Create()
    {
        return  Instantiate((GameObject)Resources.Load(SysDefine.SYS_PATH_CLUE), clueParent.transform);
    }
    #endregion

}

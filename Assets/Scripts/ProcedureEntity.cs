using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class ProcedureEntity:MonoBehaviour
{
    private XmlNode root=null;
    private string procedureName = null;

    public bool EnterProcess(string name)
    {
        procedureName = name;
        root =XMLUtil.GetSelectedXmlNodeFromRoot(XMLUtil.LoadXml(SysDefine.SYS_PATH_PROCESSDOC),procedureName);
        if (root == null) return false;
        return true;

    }
    public void StartProcess()
    {
        Debug.Log(procedureName + " 开始解析。");
        List<XmlNode> nodes = XMLUtil.GetChildNodes(root);
        if (nodes == null)
        {
            Debug.Log(procedureName + "：Null NodeList.");
        }
        foreach(XmlNode node in nodes)
        {
            string[] item = XMLUtil.GetNameValueOfNode(node);
            switch (item[0])
            {
                case "next":PreLoadNextScene(item[1]);break;
                case "clues":LoadClues(node);break;
                case "background":LoadBackground(item[1]);break;
                case "end": MessageCenter.Instance.Send(MessageCenter.MessageType.EndNormalProcess,null);break;
                default:Debug.Log(procedureName + "：遇到不可解析的结点。");
                    break;
            }
        }
        EndParsing();
    }



    private void EndParsing()
    {
        
        Debug.Log(procedureName + "：解析结束。");
    }

    #region parse
    private void PreLoadNextScene(string v)
    {
        SceneMgr.Instance.PreLoadScene(v);
    }

    private void LoadBackground(string v)
    {
        Debug.Log(v);
        GameObject.FindGameObjectWithTag("Background").GetComponent<SpriteRenderer>().sprite = Resources.Load(SysDefine.SYS_PATH_BG+v, typeof(Sprite)) as Sprite;
    }

    private void LoadClues(XmlNode v)
    {
        List<XmlNode> clueNodes = XMLUtil.GetChildNodes(v);
        if (clueNodes == null)
        {
            Debug.Log(procedureName + "：Null Clue NodeList.");return;
        }
        MessageCenter.Instance.Send(MessageCenter.MessageType.ClueGen, clueNodes);
    }


    #endregion
}

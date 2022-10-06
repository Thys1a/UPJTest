using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class ProcedureEntity:MonoBehaviour
{
    private XmlNode root=null;
    private string procedureName = null;
    private int clueNumber;
    GameObject tempClueFab;
    private GameObject clueParent;

    public bool EnterProcess(string name)
    {
        procedureName = name;
        clueNumber = 0;
        root =XMLUtil.GetSelectedXmlNodeFromRoot(XMLUtil.LoadXml(SysDefine.SYS_PATH_PROCESSDOC),procedureName);
        if (root == null) return false;
        return true;

    }
    public void StartProcess()
    {
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
                case "clues":LoadClues(node);break;
                case "background":LoadBackground(item[1]);break;
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

    private void LoadBackground(string v)
    {
        Debug.Log(v);
    }

    private void LoadClues(XmlNode v)
    {
        List<XmlNode> clueNodes = XMLUtil.GetChildNodes(v);
        if (clueNodes == null)
        {
            Debug.Log(procedureName + "：Null Clue NodeList.");return;
        }
        clueParent = new GameObject("Clues");
        foreach (XmlNode node in clueNodes)
        {
            List<XmlNode> inner = XMLUtil.GetChildNodes(node);
            if (inner == null)
            {
                Debug.Log(procedureName + "：Null Clue.");
                break;
            }
            tempClueFab = Instantiate((GameObject)Resources.Load(SysDefine.SYS_PATH_CLUE),clueParent.transform);
            ClueEntity clueEntity = tempClueFab.GetComponent<ClueEntity>();
            float x=0, y=0;
            foreach (XmlNode i in inner)
            {
                string[] item = XMLUtil.GetNameValueOfNode(i);
                switch (item[0])
                {
                    case "name": clueEntity.clueName=item[1] ;tempClueFab.name = item[1]; break;
                    case "num":clueEntity.clueNum = int.Parse(item[1]);break;
                    case "pre":clueEntity.precursor = int.Parse(item[1]);break;
                    case "valid":clueEntity.validBit=bool.Parse(item[1]); break;
                    case "x":x=float.Parse(item[1]); break;
                    case "y": y = float.Parse(item[1]); break;
                    default:
                        Debug.Log(procedureName + "：遇到不可解析的属性。");
                        break;
                }
            }
            tempClueFab.transform.position = new Vector3(x, y, tempClueFab.transform.position.z);
            clueNumber += 1;
                
        }
        MessageCenter.Instance.Send(MessageCenter.MessageType.ClueNumber, clueNumber);
    }


    #endregion
}

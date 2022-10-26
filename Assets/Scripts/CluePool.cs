using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.Pool;

public class CluePool:MonoBehaviour
{

    ObjectPool<GameObject> pool;//对象池
    public GameObject clueParent;
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
            clueEntity.precursor = -1;
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
                case "sprite": clueEntity.clueIcon = Resources.Load(SysDefine.SYS_PATH_SPRITE + item[1], typeof(Sprite)) as Sprite;break;
                case "audio":clueEntity.clueSound.clip= Resources.Load<AudioClip>(SysDefine.SYS_PATH_VOICE + item[1]);break;
                case "text":clueEntity.clueText = item[1];break;
                case "question":clueEntity.question = item[1];break;
                case "text_option":clueEntity.textOption = item[1];break;
                case "audio_option":clueEntity.audioOption = item[1];break;

                case "click_audio":clueEntity.clickSound.clip = Resources.Load<AudioClip>(SysDefine.SYS_PATH_SOUNDEFFECT + item[1]); break;
                case "click_sprite": 
                    if(clueEntity.action != "multiSprite") clueEntity.clickIcon = Resources.Load(SysDefine.SYS_PATH_SPRITE + item[1], typeof(Sprite)) as Sprite;
                    else
                    {
                        if (clueEntity.pointer == null) clueEntity.pointer= new List<Sprite>();
                        ((List<Sprite>)clueEntity.pointer).Add(Resources.Load(SysDefine.SYS_PATH_SPRITE + item[1], typeof(Sprite)) as Sprite);
                    }
                    break;
                case "action": clueEntity.action = item[1]; break;


                //case "story_text":clueEntity.stroyDescription = item[1];break;
                //case "switching_text": clueEntity.switchingDescription = item[1];break;
                //case "story_sprite":clueEntity.storyPicture= Resources.Load(SysDefine.SYS_PATH_SPRITE + item[1], typeof(Sprite)) as Sprite; break;
                default:
                    Debug.Log( "clue 生成：遇到不可解析的属性。");
                    break;
            }
        }
        obj.name = clueEntity.clueName;
        obj.GetComponent<SpriteRenderer>().sprite = clueEntity.clueIcon;
        obj.GetComponent<BoxCollider2D>().size = new Vector2(clueEntity.clueIcon.bounds.size.x, clueEntity.clueIcon.bounds.size.y);

        obj.transform.position = new Vector3(x, y, obj.transform.position.z);
        obj.SetActive(true);
        obj.GetComponent<Collider2D>().enabled = true;
    }
    private void ConstructClueList(object obj)
    {
        List<XmlNode> clueNodes = (List<XmlNode>)obj;
        clueNumber = 0;
        if (clueParent == null) clueParent = new GameObject(SysDefine.Clue);
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
        clueParent = null;
    }

    #region pool
    private void actionOnDestroy(GameObject obj)
    {
        Destroy(obj);
    }

    private void actionOnRelease(GameObject obj)
    {
        obj.GetComponent<SpriteRenderer>().sprite = null;
        obj.GetComponent<ClueEntity>().clueSound.clip = null;

        obj.GetComponent<ClueEntity>().clickSound.clip = null;
        obj.GetComponent<ClueEntity>().pointer = null;
        obj.GetComponent<ClueEntity>().action = "";
        obj.SetActive(false);

    }

    private void actionOnGet(GameObject obj)
    {
        
        try { obj.transform.parent = clueParent.transform; }
        catch (Exception)
        {
            
            if (clueParent == null) clueParent = new GameObject(SysDefine.Clue);
            if (obj == null) obj = Create();
            obj.transform.parent = clueParent.transform;
        }
        GonstructClue(node, obj);
    }

    private GameObject Create()
    {
        return  Instantiate((GameObject)Resources.Load(SysDefine.SYS_PATH_CLUE), clueParent.transform);
    }
    #endregion

}

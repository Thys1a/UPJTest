using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickManager : MonoBehaviour
 {
    public List<ClueEntity> clueList = new List<ClueEntity>();   //��һ��listȥװ���е�����


    ClueEntity tempClueEntity;   //��������Ǹ�������Ʒ�����ClueEntity    
    public int selectedType = -1;

    public Text clueSelectedPanelText;   //����ѡ���������ֵ��Ǹ�text   
    public int actionPoint = 3;   //�ж�������   
    public int clueNumber;
    public int deadCount=0;
    string[] info= new string[3];

    private void Start()
    {
        MessageCenter.Instance.Register(MessageCenter.MessageType.ClueSelectedType, getChoice);
        MessageCenter.Instance.Send(MessageCenter.MessageType.ActionPoint, actionPoint);
    }


    private void OnDestroy()
    {
        MessageCenter.Instance.Remove(MessageCenter.MessageType.ClueSelectedType, getChoice);
    }

    void Update()
    {
        Click();
    }
    private void OnEnable()
    {
        ResetManager();
    }
    public void ResetManager()
    {
        clueList.Clear();
        selectedType = -1;
        actionPoint = 3;
        MessageCenter.Instance.Send(MessageCenter.MessageType.ActionPoint, actionPoint);

    }


    /// <summary>
    /// ���������Ʒ����
    /// </summary>
    void Click()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log("click on UI");
                return;
            }
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            //�������Ʒ���ϴ���collider����Ʒ
            if (hit.collider )
            {
                
                if (hit.transform.gameObject.tag == "Clue") 
                {
                   
                    tempClueEntity = hit.transform.gameObject.GetComponent<ClueEntity>();
                    if (clueList.Contains(tempClueEntity)) {
                        Debug.Log(tempClueEntity.gameObject.name+ "has been added");
                        return;
                    }
                    Debug.Log(tempClueEntity.gameObject.name + "was clicked");

                    //�����������
                    UIManager.Instance().ShowUIForm("CluePanel");
                    MessageCenter.Instance.Send(MessageCenter.MessageType.ShowCluePanel, tempClueEntity);

                    //��������
                    PlayClue();                                      
                }
            }                        
        }
    }

    /// <summary>
    /// �����ı�����Ƶ����
    /// </summary>
    public void PlayClue()
    {
        //clueSelectedPanelText.text= tempClueEntity.clueText;
        //tempClueEntity.clueAudioSource.GetComponent<AudioSource>().Play();
        Debug.Log("Play>>");
    }

    private void getChoice(object obj)
    {
        tempClueEntity.clueType = (int)obj;
        
        CheckClue();
        AddToList(tempClueEntity);
        CheckPoint();
    }

    public void SetRecord(List<string[]> data)
    {
        GameObject parent = GameObject.Find(SysDefine.Clue);
        ClueEntity clueEntity;
        foreach (var item in data)
        {
            clueEntity = parent.transform.Find(item[0]).GetComponent<ClueEntity>();
            clueEntity.clueType = int.Parse(item[1]);
            clueList.Add(clueEntity);
        }
        
    }
    public void SetActionpoint(int number) {
        actionPoint = number;
        MessageCenter.Instance.Send(MessageCenter.MessageType.ActionPoint, actionPoint);
    }

    #region check
    /// <summary>
    /// �������
    /// </summary>
    void CheckClue()
    {
        bool flag = true;
        
        flag = CheckValidBit() && CheckPrecursor() && CheckType();
        //˳���Ϊ��ʱ���ж���-1
        
        //��Чλ���󣬲�����ȷ������
        if (!flag) actionPoint--;
        Debug.Log("��ǰ�ж������Ϊ" + actionPoint);
        MessageCenter.Instance.Send(MessageCenter.MessageType.ActionPoint, actionPoint);
        MessageCenter.Instance.Send(MessageCenter.MessageType.RecordActionPoint, actionPoint);
    }

    /// <summary>
    /// �����Чλ
    /// </summary>
    bool CheckValidBit()
    {
        //�������Ч����
        if (tempClueEntity.validBit == true )
            return true;
        return false;
    }

    /// <summary>
    /// ���ǰ��
    /// </summary>
    /// <returns></returns>
    bool CheckPrecursor()
    {
        int pre = tempClueEntity.precursor;
        switch (pre)
        {
            case -1:return true;
            case 0:if (selectedType == -1) return true;break;
            default:
                for(int i=clueList.Count-1;i>=0;i--)
                {
                    if (clueList[i].validBit == true)
                    {
                        if (clueList[i].clueNum == pre) return true;
                        else return false;
                    }
                }
                break;
        }

        return false;
    }

    /// <summary>
    /// �����������
    /// </summary>
    /// <returns></returns>
    bool CheckType()
    {
        if (tempClueEntity.clueType == selectedType)
        {
            return true;
            
        }
        if(tempClueEntity.validBit==true&& selectedType==-1) selectedType = tempClueEntity.clueType;//��һ�����ֵ���Ч����
        return false;
    }

    /// <summary>
    /// ����ж���
    /// </summary>
    void CheckPoint()
    {        
        if (actionPoint == 0)
        {
            Debug.Log("�ж���Ϊ0����Ϸʧ�ܣ���������");
            ResetManager();
            deadCount++;
        }
        if(clueList.Count == clueNumber)
        {
            if (actionPoint == 1)
            {
                Debug.Log("�ж���Ϊ1����Ϸʧ�ܣ���������");
                ResetManager();
                deadCount++;
            }
            ///ͨ�صļ���������Ը��ƣ�
            if (actionPoint == 2 )
            {
                Debug.Log("�ж���Ϊ2��������һ�ء����ؿ�ʧ�ܴ�����"+deadCount);
                ResetManager();
                MessageCenter.Instance.Send(MessageCenter.MessageType.EndLevel, deadCount);
                deadCount = 0;
            }
            
        }
    }
    #endregion

    /// <summary>
    /// ��������ӵ�����list��
    /// </summary>
    /// <param name="list">һ������</param>
    public void AddToList(ClueEntity clueEntity)
    {
        clueList.Add(clueEntity);
        info[0] = clueEntity.clueName;
        info[1] = (clueList.Count - 1).ToString();
        info[2] = clueEntity.clueType.ToString();
        MessageCenter.Instance.Send(MessageCenter.MessageType.RecordUpdate, info);
    }
}





   



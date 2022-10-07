using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickManager : MonoBehaviour
 {
    public List<ClueEntity> clueList = new List<ClueEntity>();   //��һ��listȥװ���е�����


    ClueEntity tempClueEntity;   //��������Ǹ�������Ʒ�����ClueEntity    
    public int selectedType = -1;

/*    public Text clueSelectedPanelText;   //����ѡ���������ֵ��Ǹ�text   
<<<<<<< HEAD*/
    public int actionPoint = 3;   //�ж�������   
/*=======
    public int actionPoint = 3;   //�ж�������   
>>>>>>> fb6f9e080510aebd05ce3606c14958064de1a687*/
    public int clueNumber;

    private void Start()
    {
        MessageCenter.Instance.Register(MessageCenter.MessageType.ClueSelectedType, getChoice);
    }
//<<<<<<< HEAD
    private void OnDestroy()
    {
        MessageCenter.Instance.Remove(MessageCenter.MessageType.ClueSelectedType, getChoice);
/*=======
    private void OnDestroy()
    {
        MessageCenter.Instance.Remove(MessageCenter.MessageType.ClueSelectedType, getChoice);
>>>>>>> fb6f9e080510aebd05ce3606c14958064de1a687*/
    }

    void Update()
    {
        Click();
    }
//<<<<<<< HEAD
    public void ResetManager()
    {
        clueList.Clear();
        selectedType = -1;
        actionPoint = 3;
        //clueNumber = 0;
/*
=======
    public void ResetManager()
    {
        clueList.Clear();
        selectedType = -1;
        actionPoint = 3;
        //clueNumber = 0;

>>>>>>> fb6f9e080510aebd05ce3606c14958064de1a687*/
    }


    /// <summary>
    /// ���������Ʒ����
    /// </summary>
    void Click()
    {
        if (Input.GetMouseButtonDown(0))
//<<<<<<< HEAD
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
/*=======
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
>>>>>>> fb6f9e080510aebd05ce3606c14958064de1a687*/
            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            //�������Ʒ���ϴ���collider����Ʒ
            if (hit.collider )
            {               
                if (hit.transform.gameObject.tag == "Clue") 
                {
                   
                    tempClueEntity = hit.transform.gameObject.GetComponent<ClueEntity>();
//<<<<<<< HEAD
                    if (clueList.Contains(tempClueEntity)) {
                        Debug.Log(tempClueEntity.gameObject.name+ "has been added");
                        return;
                    }
                    Debug.Log(tempClueEntity.gameObject.name + "was clicked");

                    //��������ѡ�����
/*=======
                    if (clueList.Contains(tempClueEntity)) {
                        Debug.Log(tempClueEntity.gameObject.name+ "has been added");
                        return;
                    }
                    Debug.Log(tempClueEntity.gameObject.name + "was clicked");

                    //��������ѡ�����
>>>>>>> fb6f9e080510aebd05ce3606c14958064de1a687*/
                    UIManager.Instance().ShowUIForm("ClueSelectedPanel");
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

//<<<<<<< HEAD
    private void getChoice(object obj)
    {
        tempClueEntity.clueType = (int)obj;
        
        CheckClue();
        AddToList(tempClueEntity);
        CheckePoint();
    }


/*
=======
    private void getChoice(object obj)
    {
        tempClueEntity.clueType = (int)obj;
        
        CheckClue();
        AddToList(tempClueEntity);
        CheckePoint();
    }



>>>>>>> fb6f9e080510aebd05ce3606c14958064de1a687*/
    #region check
    /// <summary>
    /// �������
    /// </summary>
    void CheckClue()
//<<<<<<< HEAD
    {
        bool flag = true;
        
        flag = CheckValidBit() && CheckPrecursor() && CheckType();
        //˳���Ϊ��ʱ���ж���-1
/*=======
    {
        bool flag = true;
        
        flag = CheckValidBit() && CheckPrecursor() && CheckType();
        //˳���Ϊ��ʱ���ж���-1
>>>>>>> fb6f9e080510aebd05ce3606c14958064de1a687*/
        
        //��Чλ���󣬲�����ȷ������
        if (!flag) actionPoint--;
        Debug.Log("��ǰ�ж������Ϊ" + actionPoint);
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
//<<<<<<< HEAD
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

/*=======
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

>>>>>>> fb6f9e080510aebd05ce3606c14958064de1a687*/
        return false;
    }

    /// <summary>
    /// �����������
    /// </summary>
    /// <returns></returns>
    bool CheckType()
    {
//<<<<<<< HEAD
        if (tempClueEntity.clueType == selectedType)
        {
            return true;
            
/*=======
        if (tempClueEntity.clueType == selectedType)
        {
            return true;
            
>>>>>>> fb6f9e080510aebd05ce3606c14958064de1a687*/
        }
        if(tempClueEntity.validBit==true&& selectedType==-1) selectedType = tempClueEntity.clueType;//��һ�����ֵ���Ч����
        return false;
    }

    /// <summary>
    /// ����ж���
    /// </summary>
    void CheckePoint()
    {        
        if (actionPoint == 0)
        {
            Debug.Log("�ж���Ϊ0����Ϸʧ�ܣ���������");
            ResetManager();
        }
        if(clueList.Count == clueNumber)
//<<<<<<< HEAD
        {
            if (actionPoint == 1)
            {
                Debug.Log("�ж���Ϊ1����Ϸʧ�ܣ���������");
                ResetManager();
            }
            ///ͨ�صļ���������Ը��ƣ�
            if (actionPoint == 2 )
            {
                Debug.Log("�ж���Ϊ2��������һ��");
                ResetManager();
                MessageCenter.Instance.Send(MessageCenter.MessageType.EndLevel, null);
            }
            
        }
    }
/*=======
        {
            if (actionPoint == 1)
            {
                Debug.Log("�ж���Ϊ1����Ϸʧ�ܣ���������");
                ResetManager();
            }
            ///ͨ�صļ���������Ը��ƣ�
            if (actionPoint == 2 )
            {
                Debug.Log("�ж���Ϊ2��������һ��");
                ResetManager();
                MessageCenter.Instance.Send(MessageCenter.MessageType.EndLevel, null);
            }
            
        }
    }
>>>>>>> fb6f9e080510aebd05ce3606c14958064de1a687*/
    #endregion

    /// <summary>
    /// ��������ӵ�����list��
    /// </summary>
    /// <param name="list">һ������</param>
    public void AddToList(ClueEntity clueEntity)
    {
        clueList.Add(clueEntity);
    }
}





   



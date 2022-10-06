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

    public Text clueSelectedPanelText;   //����ѡ���������ֵ��Ǹ�text   
    public int actionPoint = 3;   //�ж�������   
    public int clueNumber;

    private void Start()
    {
        MessageCenter.Instance.Register(MessageCenter.MessageType.ClueSelectedType, getChoice);
    }
    private void OnDestroy()
    {
        MessageCenter.Instance.Remove(MessageCenter.MessageType.ClueSelectedType, getChoice);
    }

    void Update()
    {
        Click();
    }
    public void ResetManager()
    {
        clueList.Clear();
        selectedType = -1;
        actionPoint = 3;
        //clueNumber = 0;

    }


    /// <summary>
    /// ���������Ʒ����
    /// </summary>
    void Click()
    {
        if (Input.GetMouseButtonDown(0))
        {
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

                    //��������ѡ�����
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

    private void getChoice(object obj)
    {
        tempClueEntity.clueType = (int)obj;
        
        CheckClue();
        AddToList(tempClueEntity);
        CheckePoint();
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
    void CheckePoint()
    {        
        if (actionPoint == 0)
        {
            Debug.Log("�ж���Ϊ0����Ϸʧ�ܣ���������");
            ResetManager();
        }
        if(clueList.Count == clueNumber)
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





   



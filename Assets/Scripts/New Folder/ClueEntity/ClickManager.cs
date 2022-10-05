using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickManager : MonoBehaviour
 {
    public List<ClueEntity> clueList = new List<ClueEntity>();   //��һ��listȥװ���е�����
    private ClueEntity clueEntity;       //һ��ClueEntity��һ������   
    public GameObject clueSelectedPanel;   //����������ֵ�����ѡ�����   
    GameObject temp;   //��������Ǹ�����   
    ClueEntity tempClueEntity;   //��������Ǹ�������Ʒ�����ClueEntity    
    public Text clueSelectedPanelText;   //����ѡ���������ֵ��Ǹ�text   
    private int actionPoint = 3;   //�ж�������   
    int firstClueType=-1;   //��¼��һ��Ϊ��Ч������clueList[firstClueType]
    bool firstOrder;   //�������õ���һ��������ʱ��Ϊtrue
    bool secondOrder;   //�������õ��ڶ���������ʱ��Ϊtrue

    private void Start()
    {
        clueList.Clear();
    }

    void Update()
    {
        Click();
    }


    /// <summary>
    /// ���������Ʒ����
    /// </summary>
    void Click()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
        RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

        if (Input.GetMouseButtonDown(0))
        {
            //�������Ʒ���ϴ���collider����Ʒ
            if (hit.collider )
            {               
                if (hit.transform.gameObject.tag == "Clue") 
                {
                    temp = hit.transform.gameObject;
                    tempClueEntity = temp.gameObject.GetComponent<ClueEntity>();
                    Debug.Log(temp.name);
               
                    //��������ѡ�����
                    clueSelectedPanel.SetActive(true);
                    //��������
                    PlayClue();

                    //˳���Ϊ��ʱ���ж���-1
                    if (clueList.Count == 0)
                    {
                        actionPoint--;
                        Debug.Log("��ǰ�ж������Ϊ" + actionPoint);
                    }

                    //��ȡ��һ����Ч��Ϣ������
                    if(firstClueType ==-1)
                    {
                        for (int i = 0; i < clueList.Count; i++)
                        {
                            ClueEntity clueEntity = clueList[i];
                            if (clueEntity.validBit == true)
                            {
                                firstClueType = clueList[i].clueType;
                                break;
                            }
                        }
                    }                                                     
                }
            }                        
        }
    }

    /// <summary>
    /// �����ı�����Ƶ����
    /// </summary>
    public void PlayClue()
    {
        clueSelectedPanelText = temp.GetComponent<ClueEntity>().clueText;
        temp.GetComponent<ClueEntity>().clueAudioSource.GetComponent<AudioSource>().Play();
    }

    /// <summary>
    /// ѡ�������ı���������д�ж�˳���
    /// </summary>
    public void BelieveText()
    {
        tempClueEntity.clueType = 0;
        AddToList(tempClueEntity);

        //TODO:���ı�������ӽ���Ʒ��



        //ֻ��һ��������ʱ����Ҫ���
        if (clueList.Count != 1)
            CheckClue();

        ///����ж��㷽���ĵ��ÿ��ܷ������λ�ò�̫���ʣ�
        CheckePoint();
    }

    /// <summary>
    /// ѡ��������Ƶ��������д�ж�˳���
    /// </summary>
    public void BelieveAudioSource()
    {
        tempClueEntity.clueType = 1;
        AddToList(tempClueEntity);

        //TODO:����Ƶ������ӽ���Ʒ��


        //ֻ��һ��������ʱ����Ҫ���
        if (clueList.Count != 1)
            CheckClue();
        CheckePoint();
    }

    /// <summary>
    /// �������
    /// </summary>
    void CheckClue()
    {
        //��Чλ���󣬲�����ȷ������
        if (CheckValidBit() == -1)
        {
            actionPoint += CheckValidBit();
            Debug.Log("��ǰ�ж������Ϊ" + actionPoint);
            return;
        }
        //��Чλ��ȷ
        else
        {
            //û��ǰ��
            if (CheckPrecursor() == -1)
            {
                actionPoint += CheckPrecursor();
                Debug.Log("��ǰ�ж������Ϊ" + actionPoint);
                return;
            }
            //��ǰ���������������Ƿ�һ��
            else
            {
                //���Ͳ�һ��
                if (CheckType() == -1)
                {
                    actionPoint--;
                    Debug.Log("��ǰ�ж������Ϊ" + actionPoint);
                }
                return;
            }
        }
    }

    /// <summary>
    /// �����Чλ
    /// </summary>
    int CheckValidBit()
    {
        //�������Ч����
        if (tempClueEntity.validBit == true )
            return 0;
        return -1;
    }

    /// <summary>
    /// ���ǰ��
    /// </summary>
    /// <returns></returns>
    int CheckPrecursor()
    {
        //��Ч��Ϣǰ��Ϊ-1����Ϊ�ڼ����Чλ��ʱ���Ѿ��۹�һ�������Բ���Ҫ�ٿ�һ��
        if (tempClueEntity.precursor == -1)
            return 0;
        if (firstOrder == false)
        {
            for (int i = 0; i < clueList.Count; i++)
            {
                ClueEntity clueEntiry = clueList[i];
                if (clueEntiry.precursor != 0)
                {
                    return -1;
                }
                else
                    firstOrder = true;
            }
        }
        else if (secondOrder == false)
        {
            if (tempClueEntity.precursor == 2)
            {
                return -1;
            }
            else
                secondOrder = true;
        }
        else
            return 0;

        return 0;
    }

    /// <summary>
    /// �����������
    /// </summary>
    /// <returns></returns>
    int CheckType()
    {
        if (tempClueEntity.clueType == firstClueType)
            return 0;
        return -1;
    }

    /// <summary>
    /// ����ж���
    /// </summary>
    void CheckePoint()
    {        
        if (actionPoint == 0)
        {
            Debug.Log("�ж���Ϊ0����Ϸʧ�ܣ���������");
            //�������¼���
        }
        if (actionPoint == 1)
        {
            Debug.Log("�ж���Ϊ1����Ϸʧ�ܣ����Լ���");
        }
        ///ͨ�صļ���������Ը��ƣ�
        if (actionPoint == 2)
        {
            for (int i = 0; i < clueList.Count; i++)
            {
                if (clueList[i].clueName == 1)
                {
                    for (int a = 0; a < clueList.Count; a++)
                    {
                        if (clueList[a].clueName == 2)
                            for (int b = 0; b < clueList.Count; b++)
                            {
                                if (clueList[b].clueName == 3)
                                {
                                    Debug.Log("�ж���Ϊ2�������ռ���ϣ�������һ��");
                                    //��������
                                }
                            }
                    }
                }
            }
        }
    }

    /// <summary>
    /// ��������ӵ�����list��
    /// </summary>
    /// <param name="list">һ������</param>
    public void AddToList(ClueEntity clueEntity)
    {
        clueList.Add(clueEntity);
    }
}





   



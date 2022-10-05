using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickManager : MonoBehaviour
 {
    public List<ClueEntity> clueList = new List<ClueEntity>();   //用一个list去装所有的线索
    private ClueEntity clueEntity;       //一个ClueEntity即一个线索   
    public GameObject clueSelectedPanel;   //点击线索出现的线索选择面板   
    GameObject temp;   //鼠标点击的那个线索   
    ClueEntity tempClueEntity;   //鼠标点击的那个线索物品的组件ClueEntity    
    public Text clueSelectedPanelText;   //线索选择面板里出现的那个text   
    private int actionPoint = 3;   //行动点数量   
    int firstClueType=-1;   //记录第一个为有效线索的clueList[firstClueType]
    bool firstOrder;   //第三关拿到第一个线索的时候为true
    bool secondOrder;   //第三关拿到第二个线索的时候为true

    private void Start()
    {
        clueList.Clear();
    }

    void Update()
    {
        Click();
    }


    /// <summary>
    /// 点击线索物品触发
    /// </summary>
    void Click()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
        RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

        if (Input.GetMouseButtonDown(0))
        {
            //点击的物品身上带有collider的物品
            if (hit.collider )
            {               
                if (hit.transform.gameObject.tag == "Clue") 
                {
                    temp = hit.transform.gameObject;
                    tempClueEntity = temp.gameObject.GetComponent<ClueEntity>();
                    Debug.Log(temp.name);
               
                    //开启线索选择面板
                    clueSelectedPanel.SetActive(true);
                    //播放线索
                    PlayClue();

                    //顺序表为空时，行动点-1
                    if (clueList.Count == 0)
                    {
                        actionPoint--;
                        Debug.Log("当前行动点个数为" + actionPoint);
                    }

                    //获取第一个有效信息的类型
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
    /// 播放文本和音频线索
    /// </summary>
    public void PlayClue()
    {
        clueSelectedPanelText = temp.GetComponent<ClueEntity>().clueText;
        temp.GetComponent<ClueEntity>().clueAudioSource.GetComponent<AudioSource>().Play();
    }

    /// <summary>
    /// 选择相信文本线索，填写行动顺序表
    /// </summary>
    public void BelieveText()
    {
        tempClueEntity.clueType = 0;
        AddToList(tempClueEntity);

        //TODO:将文本线索添加进物品栏



        //只有一个线索的时候不需要检查
        if (clueList.Count != 1)
            CheckClue();

        ///检查行动点方法的调用可能放在这个位置不太合适？
        CheckePoint();
    }

    /// <summary>
    /// 选择相信音频线索，填写行动顺序表
    /// </summary>
    public void BelieveAudioSource()
    {
        tempClueEntity.clueType = 1;
        AddToList(tempClueEntity);

        //TODO:将音频线索添加进物品栏


        //只有一个线索的时候不需要检查
        if (clueList.Count != 1)
            CheckClue();
        CheckePoint();
    }

    /// <summary>
    /// 检查线索
    /// </summary>
    void CheckClue()
    {
        //有效位错误，不是正确的线索
        if (CheckValidBit() == -1)
        {
            actionPoint += CheckValidBit();
            Debug.Log("当前行动点个数为" + actionPoint);
            return;
        }
        //有效位正确
        else
        {
            //没有前驱
            if (CheckPrecursor() == -1)
            {
                actionPoint += CheckPrecursor();
                Debug.Log("当前行动点个数为" + actionPoint);
                return;
            }
            //有前驱则检查线索类型是否一致
            else
            {
                //类型不一致
                if (CheckType() == -1)
                {
                    actionPoint--;
                    Debug.Log("当前行动点个数为" + actionPoint);
                }
                return;
            }
        }
    }

    /// <summary>
    /// 检查有效位
    /// </summary>
    int CheckValidBit()
    {
        //如果是有效线索
        if (tempClueEntity.validBit == true )
            return 0;
        return -1;
    }

    /// <summary>
    /// 检查前驱
    /// </summary>
    /// <returns></returns>
    int CheckPrecursor()
    {
        //无效信息前驱为-1，因为在检查有效位的时候已经扣过一次了所以不需要再扣一次
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
    /// 检查线索类型
    /// </summary>
    /// <returns></returns>
    int CheckType()
    {
        if (tempClueEntity.clueType == firstClueType)
            return 0;
        return -1;
    }

    /// <summary>
    /// 检查行动点
    /// </summary>
    void CheckePoint()
    {        
        if (actionPoint == 0)
        {
            Debug.Log("行动点为0，游戏失败，重新来过");
            //场景重新加载
        }
        if (actionPoint == 1)
        {
            Debug.Log("行动点为1，游戏失败，可以继续");
        }
        ///通关的检测条件可以改善？
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
                                    Debug.Log("行动点为2，线索收集完毕，进入下一关");
                                    //场景加载
                                }
                            }
                    }
                }
            }
        }
    }

    /// <summary>
    /// 将线索添加到线索list里
    /// </summary>
    /// <param name="list">一个线索</param>
    public void AddToList(ClueEntity clueEntity)
    {
        clueList.Add(clueEntity);
    }
}





   



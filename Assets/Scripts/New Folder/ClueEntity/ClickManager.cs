using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickManager : MonoBehaviour
 {
    public List<ClueEntity> clueList = new List<ClueEntity>();   //用一个list去装所有的线索


    ClueEntity tempClueEntity;   //鼠标点击的那个线索物品的组件ClueEntity    
    public int selectedType = -1;

    public Text clueSelectedPanelText;   //线索选择面板里出现的那个text   
    public int actionPoint = 3;   //行动点数量   
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
    /// 点击线索物品触发
    /// </summary>
    void Click()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            //点击的物品身上带有collider的物品
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

                    //开启线索选择面板
                    UIManager.Instance().ShowUIForm("ClueSelectedPanel");
                    //播放线索
                    PlayClue();                                      
                }
            }                        
        }
    }

    /// <summary>
    /// 播放文本和音频线索
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
    /// 检查线索
    /// </summary>
    void CheckClue()
    {
        bool flag = true;
        
        flag = CheckValidBit() && CheckPrecursor() && CheckType();
        //顺序表为空时，行动点-1
        
        //有效位错误，不是正确的线索
        if (!flag) actionPoint--;
        Debug.Log("当前行动点个数为" + actionPoint);
    }

    /// <summary>
    /// 检查有效位
    /// </summary>
    bool CheckValidBit()
    {
        //如果是有效线索
        if (tempClueEntity.validBit == true )
            return true;
        return false;
    }

    /// <summary>
    /// 检查前驱
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
    /// 检查线索类型
    /// </summary>
    /// <returns></returns>
    bool CheckType()
    {
        if (tempClueEntity.clueType == selectedType)
        {
            return true;
            
        }
        if(tempClueEntity.validBit==true&& selectedType==-1) selectedType = tempClueEntity.clueType;//第一个出现的有效线索
        return false;
    }

    /// <summary>
    /// 检查行动点
    /// </summary>
    void CheckePoint()
    {        
        if (actionPoint == 0)
        {
            Debug.Log("行动点为0，游戏失败，重新来过");
            ResetManager();
        }
        if(clueList.Count == clueNumber)
        {
            if (actionPoint == 1)
            {
                Debug.Log("行动点为1，游戏失败，重新来过");
                ResetManager();
            }
            ///通关的检测条件可以改善？
            if (actionPoint == 2 )
            {
                Debug.Log("行动点为2，进入下一关");
                ResetManager();
                MessageCenter.Instance.Send(MessageCenter.MessageType.EndLevel, null);
            }
            
        }
    }
    #endregion

    /// <summary>
    /// 将线索添加到线索list里
    /// </summary>
    /// <param name="list">一个线索</param>
    public void AddToList(ClueEntity clueEntity)
    {
        clueList.Add(clueEntity);
    }
}





   



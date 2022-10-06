using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClueSelectedPanel : BaseUIForm
{
    
    void Start()
    {

    }

    /// <summary>
    /// 选择相信文本线索，填写行动顺序表
    /// </summary>
    public void BelieveText()
    {
        MessageCenter.Instance.Send(MessageCenter.MessageType.ClueSelectedType, 0);
        UIManager.Instance().CloseUIForms("ClueSelectedPanel");
    }

    /// <summary>
    /// 选择相信音频线索，填写行动顺序表
    /// </summary>
    public void BelieveAudioSource()
    {
        MessageCenter.Instance.Send(MessageCenter.MessageType.ClueSelectedType, 1);
        UIManager.Instance().CloseUIForms("ClueSelectedPanel");
    }
}

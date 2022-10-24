using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MessageCenter
{
    //单例类
    private static MessageCenter instance;
    public static MessageCenter Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new MessageCenter();
            }
            return instance;
        }
    }

    //保存所有消息事件的字典
    //key使用字符串保存消息的名称
    //value使用一个带自定义参数的事件，用来调用所有注册的消息
    private Dictionary<MessageType, EventMode> EventDictionary;
    private class EventMode : UnityEvent<object>
    {

    }

    /// <summary>
    /// 消息类型
    /// </summary>
    public enum MessageType
    {
        SceneSwitching,//场景切换
        ClueSelectedType,//线索的选择
        EndLevel,//关卡流程结束
        EndNormalProcess,//普通流程结束
        ClueNumber,//有效线索个数
        ClueGen,//转到线索生成
        ClueRecycle,//线索回收
        Archive,//是否读取存档
        RecordUpdate,//存档记录更新
        ActionPoint,//行动点个数
        ShowCluePanel,
        ShowClueSelectedPanel,
        RecordActionPoint,
    }

    /// <summary>
    /// 私有构造函数
    /// </summary>
    private MessageCenter()
    {
        EventDictionary = new Dictionary<MessageType, EventMode>();
    }

    /// <summary>
    /// 添加监听者
    /// </summary>
    /// <param name="key">消息名</param>
    /// <param name="action">消息事件</param>
    public void Register(MessageType key, UnityAction<object> listener)
    {
        EventMode tempEvent = null;
        if (EventDictionary.TryGetValue(key, out tempEvent))
        {
            tempEvent.AddListener(listener);
        }
        else
        {
            tempEvent = new EventMode();
            tempEvent.AddListener(listener);
            EventDictionary.Add(key, tempEvent);
        }

    }


    /// <summary>
    /// 注销消息事件
    /// </summary>
    /// <param name="key">消息名</param>
    /// <param name="action">消息事件</param>
    public void Remove(MessageType key, UnityAction<object> listener)
    {
        if (instance == null) return;
        EventMode tempEvent = null;
        if (EventDictionary.TryGetValue(key, out tempEvent))
        {
            tempEvent.RemoveListener(listener);
        }
    }


    /// <summary>
    /// 发送消息
    /// </summary>
    /// <param name="key">消息名</param>

    public void Send(MessageType key, object data)
    {

        EventMode tempEvent;
        if (EventDictionary.TryGetValue(key, out tempEvent))
        {
            tempEvent.Invoke(data);
        }
    }

    /// <summary>
    /// 清空所有消息
    /// </summary>
    public void Clear()
    {
        EventDictionary.Clear();

    }

}

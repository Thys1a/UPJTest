using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MessageCenter
{
    //������
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

    //����������Ϣ�¼����ֵ�
    //keyʹ���ַ���������Ϣ������
    //valueʹ��һ�����Զ���������¼���������������ע�����Ϣ
    private Dictionary<MessageType, EventMode> EventDictionary;
    private class EventMode : UnityEvent<object>
    {

    }

    /// <summary>
    /// ��Ϣ����
    /// </summary>
    public enum MessageType
    {
        SceneSwitching,ClueSelectedType,EndLevel,EndNormalProcess,
        ClueNumber,
        ClueGen,
        ClueRecycle
    }

    /// <summary>
    /// ˽�й��캯��
    /// </summary>
    private MessageCenter()
    {
        EventDictionary = new Dictionary<MessageType, EventMode>();
    }

    /// <summary>
    /// ��Ӽ�����
    /// </summary>
    /// <param name="key">��Ϣ��</param>
    /// <param name="action">��Ϣ�¼�</param>
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
    /// ע����Ϣ�¼�
    /// </summary>
    /// <param name="key">��Ϣ��</param>
    /// <param name="action">��Ϣ�¼�</param>
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
    /// ������Ϣ
    /// </summary>
    /// <param name="key">��Ϣ��</param>

    public void Send(MessageType key, object data)
    {

        EventMode tempEvent;
        if (EventDictionary.TryGetValue(key, out tempEvent))
        {
            tempEvent.Invoke(data);
        }
    }

    /// <summary>
    /// ���������Ϣ
    /// </summary>
    public void Clear()
    {
        EventDictionary.Clear();

    }

}

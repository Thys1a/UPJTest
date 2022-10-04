using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���͵���
/// </summary>
/// <typeparam name="T"></typeparam>
public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T instance;
    public static T Instance
    {
        get { return instance; }
    }

    protected virtual void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = (T)this;
    }

    /*//��һ���������ж������ģʽ��ʱ�򣬾���Ҫȥ����
    //���ٵ�ʱ����Ϊ��
    protected virtual void OnDestory()
    {
        if (instance == this)
        {
            instance = null;
        }
    }*/
}
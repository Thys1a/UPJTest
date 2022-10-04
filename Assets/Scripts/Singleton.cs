using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 泛型单例
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

    /*//在一个场景中有多个单例模式的时候，就需要去销毁
    //销毁的时候设为空
    protected virtual void OnDestory()
    {
        if (instance == this)
        {
            instance = null;
        }
    }*/
}

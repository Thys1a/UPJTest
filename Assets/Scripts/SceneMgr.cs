using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMgr 
{
    //单例类
    private static SceneMgr instance;
    public static SceneMgr Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new SceneMgr();
            }
            return instance;
        }
    }
    private Dictionary<string, AsyncOperation> PreLoadingScenesDict;

    private SceneMgr()
    {
        PreLoadingScenesDict = new Dictionary<string, AsyncOperation>();
        SceneManager.activeSceneChanged += OnSceneSwitching;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
        SceneManager.sceneLoaded += OnSceneLoaded;

    }


    #region 场景加载
    //todo：过渡场景？

    /// <summary>
    /// 预加载场景（LoadSceneMode.Additive）
    /// </summary>
    /// <param name="name"></param>
    public void PreLoadScene(string name)
    {

        AsyncOperation pre = SceneManager.LoadSceneAsync(name);
        pre.allowSceneActivation = false;

        if (PreLoadingScenesDict.ContainsKey(name))
        {
            PreLoadingScenesDict[name] = pre;
        }
        else
        {
            PreLoadingScenesDict.Add(name, pre);
        }
    }

    /// <summary>
    /// 加载场景，并发送切换消息
    /// </summary>
    /// <param name="name">场景名称</param>
    public void LoadScene(string name=null)
    {
        if (name == null)
        {
            if (PreLoadingScenesDict.Count > 0)
            {
                name = PreLoadingScenesDict.First().Key;
            }
            else
            {
                Debug.LogError("scene name was not provided");
                return;
            }
        }
        if (PreLoadingScenesDict.ContainsKey(name))
        {
            PreLoadingScenesDict[name].allowSceneActivation = true;
            PreLoadingScenesDict.Remove(name);
        }
        else
        {
            AsyncOperation op = SceneManager.LoadSceneAsync(name);
        }
    }

    /// <summary>
    /// 异步加载场景
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public AsyncOperation LoadSceneAsync(string name)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(name);
        return op;
    }
    #endregion

    //todo：资源加载（卸载/加载）？
    //todo：栈？
    //todo：场景切换效果？

    /// <summary>
    /// 获取当前活动场景
    /// </summary>
    /// <returns></returns>
    public Scene GetScene()
    {
        
        return SceneManager.GetActiveScene();
    }
    public int WaitingNumber()
    {
        return PreLoadingScenesDict.Count;
    }

    /// <summary>
    /// 卸载场景
    /// </summary>
    /// <param name="name"></param>
    public void DestroyScene(string name)
    {
        SceneManager.UnloadSceneAsync(name);
    }


    private void OnSceneSwitching(Scene current, Scene next) {
        Debug.Log(current+" to "+next);
        MessageCenter.Instance.Send(MessageCenter.MessageType.SceneSwitching, (object)next);
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded: " + scene.name+";mode: "+ mode);
    }
    private void OnSceneUnloaded(Scene current)
    {
        Debug.Log("OnSceneUnloaded: " + current);
    }
}

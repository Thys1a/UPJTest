using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMgr 
{
    //������
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


    #region ��������
    //todo�����ɳ�����

    /// <summary>
    /// Ԥ���س�����LoadSceneMode.Additive��
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
    /// ���س������������л���Ϣ
    /// </summary>
    /// <param name="name">��������</param>
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
    /// �첽���س���
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public AsyncOperation LoadSceneAsync(string name)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(name);
        return op;
    }
    #endregion

    //todo����Դ���أ�ж��/���أ���
    //todo��ջ��
    //todo�������л�Ч����

    /// <summary>
    /// ��ȡ��ǰ�����
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
    /// ж�س���
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

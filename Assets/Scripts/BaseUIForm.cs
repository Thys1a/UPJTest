using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUIForm : MonoBehaviour
{

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //ҳ����ʾ
    public virtual void Display()
    {
        UIManager.Instance().SetUIMask(this.gameObject);
        this.gameObject.SetActive(true);
    }

    //ҳ������(���ڡ�ջ��������)
    public virtual void Hiding()
    {
        UIManager.Instance().CancelUIMask(this.gameObject);
        this.gameObject.SetActive(false);
    }
    //ҳ��������ʾ
    public virtual void Redisplay()
    {
        UIManager.Instance().SetUIMask(this.gameObject);
        this.gameObject.SetActive(true);
    }
    //ҳ�涳��(���ڡ�ջ��������)
    public virtual void Freeze()
    {
        this.gameObject.SetActive(true);
    }
}

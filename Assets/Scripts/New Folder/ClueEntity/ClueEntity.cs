using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClueEntity : MonoBehaviour
{    
    public string clueName;  //������
    public int clueNum;
    public Sprite clueIcon;  //����ͼƬ
    public string clueText;  //�����������ı�
    public AudioSource clueSound;  //������������Ƶ
    public bool validBit;  //��������Чλ
    public int precursor;  //������ǰ��
    public int clueType;  //�������ͣ�����ϲ�����д����ѡ���ʱ�����д
    public string question;
    public string textOption, audioOption;
    public Sprite clickIcon; // �����ͼƬ
    public Sprite cluePanelBG;  //������屳��ͼƬ

    public AudioSource clickSound; //���ʱ�������
    public string action;
    public object pointer;//����ָ��

    //public string stroyDescription;
    //public Sprite storyPicture;
    //public string switchingDescription;
   /* private void Start()
    {
        clueSound.enabled = true;
    }*/
}


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClueEntity : MonoBehaviour
{    
    public string clueName;  //线索名
    public int clueNum;
    public Sprite clueIcon;  //线索图片
    public string clueText;  //线索包含的文本
    public AudioSource clueSound;  //线索包含的音频
    public bool validBit;  //线索的有效位
    public int precursor;  //线索的前驱
    public int clueType;  //线索类型，面板上不用填写，在选择的时候会填写
    public string question;
    public string textOption, audioOption;
    public Sprite clickIcon; // 点击的图片
    public Sprite cluePanelBG;  //线索面板背景图片

    public AudioSource clickSound; //点击时候的声音
    public string action;
    public object pointer;//自由指针

    //public string stroyDescription;
    //public Sprite storyPicture;
    //public string switchingDescription;
   /* private void Start()
    {
        clueSound.enabled = true;
    }*/
}


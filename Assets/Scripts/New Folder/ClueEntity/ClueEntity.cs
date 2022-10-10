using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClueEntity : MonoBehaviour
{    
    public string clueName;  //线索名
    public int clueNum;
    public Texture clueIcon;  //线索图片
    public string clueText;  //线索包含的文本
    public AudioSource clueAudioSource;  //线索包含的音频
    public bool validBit;  //线索的有效位
    public int precursor;  //线索的前驱
    public int clueType;  //线索类型，面板上不用填写，在选择的时候会填写
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton <AudioManager >
{
    //背景音乐数组
    public AudioSource[] backgroundMusic;
    //音效数组
    public AudioSource[] soundEffect;

    protected override  void Awake()
    {
        for (int i = 0; i < backgroundMusic.Length; i++)
            backgroundMusic[i] = GetComponent<AudioSource >();
        for (int i = 0; i < soundEffect.Length; i++)
            soundEffect[i] = GetComponent<AudioSource >();
    }

    /// <summary>
    /// 通过数组编号来播放背景音乐
    /// </summary>
    /// <param name="i">背景音乐的数组编号</param>
    public void BackGroundMusicPlay(int i)
    {
        backgroundMusic[i].Play();
    }

    /// <summary>
    /// 通过数组编号来暂停背景音乐
    /// </summary>
    /// <param name="i">背景音乐的数组编号</param>
    public void BackGroundMusicPause(int i)
    {
        backgroundMusic[i].Pause();
    }

    /// <summary>
    /// 通过数组编号来播放音效
    /// </summary>
    /// <param name="i">音效的数组编号</param>
    public void MusicEffectPlay(int i)
    {
        backgroundMusic[i].Pause();
    }

    /*/// <summary>
    /// 通过数组编号来暂停音效
    /// </summary>
    /// <param name="i">音效的数组编号</param>
    public void MusicEffectPause(int i)
    {
        backgroundMusic[i].Stop();
    }*/
}

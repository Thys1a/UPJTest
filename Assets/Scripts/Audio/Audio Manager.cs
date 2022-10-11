using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton <AudioManager >
{
    //������������
    public AudioSource[] backgroundMusic;
    //��Ч����
    public AudioSource[] soundEffect;

    protected override  void Awake()
    {
        for (int i = 0; i < backgroundMusic.Length; i++)
            backgroundMusic[i] = GetComponent<AudioSource >();
        for (int i = 0; i < soundEffect.Length; i++)
            soundEffect[i] = GetComponent<AudioSource >();
    }

    /// <summary>
    /// ͨ�������������ű�������
    /// </summary>
    /// <param name="i">�������ֵ�������</param>
    public void BackGroundMusicPlay(int i)
    {
        backgroundMusic[i].Play();
    }

    /// <summary>
    /// ͨ������������ͣ��������
    /// </summary>
    /// <param name="i">�������ֵ�������</param>
    public void BackGroundMusicPause(int i)
    {
        backgroundMusic[i].Pause();
    }

    /// <summary>
    /// ͨ����������������Ч
    /// </summary>
    /// <param name="i">��Ч��������</param>
    public void MusicEffectPlay(int i)
    {
        backgroundMusic[i].Pause();
    }

    /*/// <summary>
    /// ͨ������������ͣ��Ч
    /// </summary>
    /// <param name="i">��Ч��������</param>
    public void MusicEffectPause(int i)
    {
        backgroundMusic[i].Stop();
    }*/
}

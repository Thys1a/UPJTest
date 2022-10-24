using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TypewitterEffect : MonoBehaviour
{
    string content, cur_content;
    UnityAction action;
    Coroutine coroutine;
    float second = .1f;
    public TMPro.TextMeshProUGUI textUI;
    public AudioSource audioSource;
    public void StartPlaying(string content, UnityAction callback = null, float s = 0.1f)
    {
        this.content = content;
        this.action = callback;
        this.second = s;
        this.cur_content = null;
        coroutine = StartCoroutine(nameof(PlayText));
    }

    public void StopPlaying()
    {
        StopCoroutine(coroutine);
        
    }

    IEnumerator PlayText()
    {
        for (int i = 1; i <= content.Length; i++)
        {
            audioSource.Play();
            cur_content = content.Substring(0, i);
            textUI.text = cur_content;
            yield return new WaitForSeconds(second);
        }
        FinishPlaying();
        yield return null;


    }
    private void FinishPlaying()
    {
        if (action != null) action();

    }
}

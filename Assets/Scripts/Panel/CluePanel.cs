using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CluePanel : BaseUIForm
{
    public Button clueBtn,replayBtn;
    public TMPro.TextMeshProUGUI text;
    private ClueEntity clue = null;
    private bool isFinished = false;
    private bool textOK = false;
    private int i = 0;

    private void Awake()
    {
        MessageCenter.Instance.Register(MessageCenter.MessageType.ShowCluePanel, DataPrepare);
    }
    private void OnDestroy()
    {
        MessageCenter.Instance.Remove(MessageCenter.MessageType.ShowCluePanel, DataPrepare);
    }
    private void Start()
    {
        replayBtn.onClick.AddListener(Replay);
        clueBtn.onClick.AddListener(OnClickClue);
    }

    public void DataPrepare(object clueEntity)
    {
        clue = (ClueEntity)clueEntity;
        clueBtn.GetComponent<Image>().sprite = clue.clickIcon;
        
        base.Display();
        Play();
        
    }
    public override void Display()
    {
       
        
        
    }
    public override void Hiding()
    {

        StopPlaying();
        ResetData();
        base.Hiding();
    }

    #region public method
    public void Replay()
    {
        if (!isFinished) return;
        textOK = false;
        Play();
    }

    public void OnClickClue()
    {
        if (!isFinished) return;
        if (clue.clickSound.clip!=null)clue.clickSound.Play();
        bool flag = false;
        if (clue.action != "")
        {
            if (clue.action == "multiSprite")
            {
                if (i < ((List<Sprite>)clue.pointer).Count) clueBtn.image.sprite = ((List<Sprite>)clue.pointer)[i];
                else flag = true;
                i++;
            }
        }
        else flag = true;
        if (flag) {
            
            MessageCenter.Instance.Send(MessageCenter.MessageType.ShowClueSelectedPanel, clue);
        }
    }
    #endregion

    private void ResetData()
    {
        clue = null;
        isFinished = false;
        textOK = false;
        clueBtn.GetComponent<Image>().sprite = null;

    }
    void Play() {
        StartCoroutine(nameof(CheckPlayingState));
        this.GetComponent<TypewitterEffect>().StartPlaying(clue.clueText, FinishTextPlaying);
    }

    IEnumerator CheckPlayingState()
    {
        clue.clueSound.Play();
        //while (clue.clueSound.isPlaying)
        //{
        //    yield return new WaitForSeconds(clue.clueSound.clip.length);
        //}
        UIManager.Instance().ShowUIForm("ClueSelectedPanel");
        yield return new WaitUntil(() => clue.clueSound.isPlaying == false);
        yield return new WaitUntil(()=>textOK==true);
        text.text = null;
        isFinished = true;
    }

    void FinishTextPlaying()
    {
        textOK = true;
        
    }
    void StopPlaying()
    {
        clue.clueSound.Stop();
        this.GetComponent<TypewitterEffect>().StopPlaying();
    }
}

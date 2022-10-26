using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Test : Singleton <Test>
{
    public GameObject loadingPanel;
    public Slider slider;
    public Text text;
    public bool load;
    public Transform  parentPanel;

    public void LoadNextLevel()
    {
        StartCoroutine(Loadlevel());
        Invoke("ClosePanel", 1.3f);
    }

    IEnumerator Loadlevel()
    {
        slider.value = 0;
        loadingPanel.SetActive(true);
        loadingPanel.transform.SetParent(parentPanel);
        while (slider .value != 1f )
        {
            slider.value += 1*Time .deltaTime ;
            text.text = slider.value * 100 + "%";
            yield return 3;
        }
        if (slider.value == 1)
            load = true;
    }

    public void ClosePanel()
    {
        loadingPanel.SetActive(false);
    }
}

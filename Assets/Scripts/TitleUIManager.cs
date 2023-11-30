using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;
using UnityEngine.EventSystems;

public class TitleUIManager : MonoBehaviour
{
    public Image fadePanel;
    public GameObject audioOnOff;
    
    void Start()
    {
    }

    public void SetAudioOnOff(bool target)
    {
        audioOnOff.SetActive(target);
    }

    public void StartGame()
    {
        fadePanel.gameObject.SetActive(true);
        var seq = DOTween.Sequence();
        seq.Append(fadePanel.DOFade(1f, 1));
        seq.InsertCallback(1, () => SceneManager.LoadScene(1));
    }
}

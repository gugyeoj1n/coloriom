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
    private TitleGameManager gameManager;
    
    public Image fadePanel;
    public GameObject audioOnOff;
    public TMP_Text bestScoreText;
    public GameObject shopPanel;
    public TMP_Text currentCoinText;
    public TMP_Text currentTimeText;
    public TMP_Text currentPassText;
    public TMP_Text currentAdText;
    public TMP_Text currentVersionText;
    public GameObject updatePanel;
    
    void Start()
    {
        gameManager = FindObjectOfType<TitleGameManager>();
    }

    public void SetAudioOnOff(bool target)
    {
        audioOnOff.SetActive(target);
    }

    public void OpenShop()
    {
        RefreshShop();
        shopPanel.SetActive(true);
    }

    public void RefreshShop()
    {
        gameManager.LoadItems();
        currentCoinText.text = gameManager.coin + "코인 보유 중";
        currentTimeText.text = gameManager.timeItem + "개 보유 중";
        currentPassText.text = gameManager.passItem + "개 보유 중";
        currentAdText.text = (gameManager.advertisePass > 0) ? "활성화됨" : "비활성화됨";
    }

    public void StartGame()
    {
        fadePanel.gameObject.SetActive(true);
        var seq = DOTween.Sequence();
        seq.Append(fadePanel.DOFade(1f, 1));
        seq.InsertCallback(1, () => SceneManager.LoadScene(1));
    }
    
    public void StartChallenge()
    {
        fadePanel.gameObject.SetActive(true);
        var seq = DOTween.Sequence();
        seq.Append(fadePanel.DOFade(1f, 1));
        seq.InsertCallback(1, () => SceneManager.LoadScene(2));
    }

    public void StartTutorial()
    {
        fadePanel.gameObject.SetActive(true);
        var seq = DOTween.Sequence();
        seq.Append(fadePanel.DOFade(1f, 1));
        seq.InsertCallback(1, () => SceneManager.LoadScene(3));
    }

    public void OpenUpdate()
    {
        updatePanel.SetActive(true);
    }
}

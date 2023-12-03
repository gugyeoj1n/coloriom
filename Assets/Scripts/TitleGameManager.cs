using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleGameManager : MonoBehaviour
{
    private TitleUIManager ui;
    
    public int timeItem;
    public int passItem;
    public int advertisePass;
    public int coin;
    public int normalBestScore;
    public int challengeBestScore;
    public bool audio;
    
    void Awake()
    {
        ui = FindObjectOfType<TitleUIManager>();
        
        LoadAudio();
        LoadItems();
    }

    public void LoadAudio()
    {
        if (!PlayerPrefs.HasKey("Audio"))
        {
            PlayerPrefs.SetInt("Audio", 1);
            audio = true;
        }
        else
        {
            if (PlayerPrefs.GetInt("Audio") == 1)
            {
                ui.SetAudioOnOff(false);
                audio = true;
            }
            else
            {
                ui.SetAudioOnOff(true);
                audio = false;
            }
        }

        OnOffAllAudio(audio);
    }

    public void LoadItems()
    {
        if (PlayerPrefs.HasKey("TimeItem"))
        {
            timeItem = PlayerPrefs.GetInt("TimeItem");
        }
        else
        {
            timeItem = 0;
            PlayerPrefs.SetInt("TimeItem", 0);
        }

        if (PlayerPrefs.HasKey("PassItem"))
        {
            passItem = PlayerPrefs.GetInt("PassItem");
        }
        else
        {
            passItem = 0;
            PlayerPrefs.SetInt("PassItem", 0);
        }

        if (PlayerPrefs.HasKey("AdvertisePass"))
        {
            advertisePass = PlayerPrefs.GetInt("AdvertisePass");
        }
        else
        {
            advertisePass = 0;
            PlayerPrefs.SetInt("AdvertisePass", 0);
        }
        
        if (PlayerPrefs.HasKey("Coin"))
        {
            coin = PlayerPrefs.GetInt("Coin");
        }
        else
        {
            coin = 3000;
            PlayerPrefs.SetInt("Coin", 3000);
        }
        
        if (PlayerPrefs.HasKey("NormalBestScore"))
        {
            normalBestScore = PlayerPrefs.GetInt("NormalBestScore");
        }
        else
        {
            normalBestScore = 0;
            PlayerPrefs.SetInt("NormalBestScore", 0);
        }
        
        if (PlayerPrefs.HasKey("ChallengeBestScore"))
        {
            challengeBestScore = PlayerPrefs.GetInt("ChallengeBestScore");
        }
        else
        {
            challengeBestScore = 0;
            PlayerPrefs.SetInt("ChallengeBestScore", 0);
        }

        ui.bestScoreText.text = string.Format("일반 {0}점\n도전 {1}점", normalBestScore, challengeBestScore);
    }

    public void SetAudio()
    {
        if (PlayerPrefs.GetInt("Audio") == 1)
        {
            ui.SetAudioOnOff(true);
            PlayerPrefs.SetInt("Audio", 0);
            audio = false;
        }
        else
        {
            ui.SetAudioOnOff(false);
            PlayerPrefs.SetInt("Audio", 1);
            audio = true;
        }

        OnOffAllAudio(audio);
    }

    private void OnOffAllAudio(bool target)
    {
        AudioSource[] audios = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audio in audios)
        {
            audio.volume = target ? 1 : 0;
        }
    }

    public void PurchaseTime()
    {
        if (PlayerPrefs.GetInt("Coin") < 1000) return;
        
        PlayerPrefs.SetInt("TimeItem", PlayerPrefs.GetInt("TimeItem") + 1);
        PlayerPrefs.SetInt("Coin", PlayerPrefs.GetInt("Coin") - 1000);
        
        ui.RefreshShop();
    }
    
    public void PurchasePass()
    {
        if (PlayerPrefs.GetInt("Coin") < 800) return;
        
        PlayerPrefs.SetInt("PassItem", PlayerPrefs.GetInt("PassItem") + 1);
        PlayerPrefs.SetInt("Coin", PlayerPrefs.GetInt("Coin") - 800);
        
        ui.RefreshShop();
    }
}

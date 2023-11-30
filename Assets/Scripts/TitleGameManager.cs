using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleGameManager : MonoBehaviour
{
    private TitleUIManager ui;
    
    public int timeItem;
    public int colorItem;
    public int advertisePass;
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
        timeItem = PlayerPrefs.HasKey("TimeItem") ? PlayerPrefs.GetInt("TimeItem") : 0;
        colorItem = PlayerPrefs.HasKey("ColorItem") ? PlayerPrefs.GetInt("ColorItem") : 0;
        advertisePass = PlayerPrefs.HasKey("AdvertisePass") ? PlayerPrefs.GetInt("AdvertisePass") : 0;
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
}

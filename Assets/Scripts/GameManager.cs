using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header ("Objects")]
    public Transform mold;
    public GameObject colorObject;
    [Header ("Colors")]
    public Color[] colors;
    public Color targetColor;
    [Header ("Variables")]
    public int multiply = 1;
    public int score = 0;
    public float time = 120f;
    [Header ("UI")]
    public TMP_Text multiplyText;
    public TMP_Text scoreText;
    public TMP_Text timeText;
    public Image targetImage;
    
    void Start()
    {
        InitMold();
    }

    private void InitMold()
    {
        for (int i = 0; i < 49; i++)
        {
            GameObject colorInst = Instantiate(colorObject, mold);
            colorInst.GetComponent<Image>().color = colors[Random.Range(0, colors.Length)];
        }
    }

    void Update()
    {
        time -= Time.deltaTime;
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        timeText.text = string.Format("{0:0}:{1:00}", minutes, seconds);
    }

    public void BackToLobby()
    {
        SceneManager.LoadScene(0);
    }
}

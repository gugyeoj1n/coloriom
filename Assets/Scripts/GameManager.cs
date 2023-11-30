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
    private int row = 7;
    private int col = 7;
    [Header ("Colors")]
    public Color[] colors;
    public Color targetColor;
    [Header ("Variables")]
    public int multiply = 1;
    public int score = 0;
    public float time = 121f;
    public bool isGaming = true;
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
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                GameObject colorInst = Instantiate(colorObject, mold);
                colorInst.GetComponent<Image>().color = colors[Random.Range(0, colors.Length)];
                
                if (i == 0 || i == row - 1 || j == 0 || j == col - 1)
                {
                    if (i == 0)
                    {
                        if (j == 0)
                            colorInst.GetComponent<ColorObject>().type = ColorObject.OutlineType.TopLeft;
                        else if (j == col - 1)
                            colorInst.GetComponent<ColorObject>().type = ColorObject.OutlineType.TopRight;
                        else
                            colorInst.GetComponent<ColorObject>().type = ColorObject.OutlineType.Top;
                    }
                    else if (i == row - 1)
                    {
                        if (j == 0)
                            colorInst.GetComponent<ColorObject>().type = ColorObject.OutlineType.BottomLeft;
                        else if (j == col - 1)
                            colorInst.GetComponent<ColorObject>().type = ColorObject.OutlineType.BottomRight;
                        else
                            colorInst.GetComponent<ColorObject>().type = ColorObject.OutlineType.Bottom;
                    }

                    if (j == 0 && i != 0 && i != row - 1)
                        colorInst.GetComponent<ColorObject>().type = ColorObject.OutlineType.Left;
                    else if (j == col - 1 && i != 0 && i != row - 1)
                        colorInst.GetComponent<ColorObject>().type = ColorObject.OutlineType.Right;
                } else
                    colorInst.GetComponent<ColorObject>().type = ColorObject.OutlineType.Inside;
            }
        }
        /*for (int i = 0; i < 49; i++)
        {
            GameObject colorInst = Instantiate(colorObject, mold);
            colorInst.GetComponent<Image>().color = colors[Random.Range(0, colors.Length)];
        }*/
    }

    void Update()
    {
        if (!isGaming) return;
        time -= Time.deltaTime;
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        timeText.text = string.Format("{0:0}:{1:00}", minutes, seconds);
    }

    public void Pause()
    {
        isGaming = false;
    }

    public void Resume()
    {
        isGaming = true;
    }

    public void BackToLobby()
    {
        SceneManager.LoadScene(0);
    }
}

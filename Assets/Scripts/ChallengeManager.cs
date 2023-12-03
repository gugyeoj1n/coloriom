using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ChallengeManager : MonoBehaviour
{
    [Header ("Objects")]
    public Transform mold;
    public GameObject colorObject;
    private int row = 7;
    private int col = 7;
    [Header ("Colors")]
    public Color[] colors;
    public Color targetColor;
    [Header("Variables")]
    public int multiply = 1;
    public int score = 0;
    public float time = 121f;
    public bool isGaming = true;
    public bool isMoving = false;
    public int timeItems;
    public int passItems;
    public GameObject[,] objects;
    public float animationSpeed = 0.5f;
    public Color recentColor;
    [Header ("Input")]
    private InputManager inputManager;
    private bool canMove = false;
    public Vector2 clickPos;
    public GameObject startBlock;
    public GameObject endBlock;
    [Header ("UI")]
    public TMP_Text multiplyText;
    public TMP_Text scoreText;
    public TMP_Text timeText;
    public TMP_Text timeEffect;
    public Image targetImage;
    public GameObject overBackground;
    public GameObject overPanel;
    public TMP_Text overScoreText;
    public TMP_Text timeCount;
    public TMP_Text passCount;
    public GameObject audioOff;
    public GameObject combinationPanel;
    public Transform combinationContent;
    public GameObject combinationObject;
    [Header ("Audio")]
    private AudioSource audio;
    public AudioClip[] clips;
    
    void Start()
    {
        audio = GetComponent<AudioSource>();
        InitMold();
        inputManager = new InputManager();
        GenerateTarget();
        GenerateCombinations();
        InitItems();
    }

    private void PlayAudio(AudioClip clip)
    {
        audio.clip = clip;
        audio.Play();
    }

    private void InitItems()
    {
        timeItems = PlayerPrefs.GetInt("TimeItem");
        passItems = PlayerPrefs.GetInt("PassItem");

        timeCount.text = timeItems.ToString();
        passCount.text = passItems.ToString();
    }

    private void GenerateTarget()
    {
        /*if(multiply < colors.Length)
        multiply++;
        Color[] mixedColors = GetRandomColors(multiply);*/

        Color[] mixed = GetRandomColors(2);
        targetColor = MixColor(mixed[0], mixed[1]);
        targetImage.transform.DOScale(Vector3.one * 1.2f, 0.75f).OnComplete(() =>
        {
            targetImage.transform.DOScale(Vector3.one, 0.75f);
        });
        targetImage.DOColor(targetColor,  1.5f);
    }

    private void CheckClear()
    {
        if (recentColor == targetColor)
        {
            Clear();
        }
        
        Destroy(startBlock);
        isMoving = false;
    }
    
    private void Clear()
    {
        PlayAudio(clips[0]);
        score += 100 * multiply;
        scoreText.transform.DOScale(Vector3.one * 1.4f, .2f);
        scoreText.transform.DOScale(Vector3.one, 1.3f);
        GenerateTarget();
    }
    
    private Color[] GetRandomColors(int count)
    {
        Color[] randomColors = new Color[count];

        for (int i = 0; i < count; i++)
        {
            int randomIndex = Random.Range(0, colors.Length);
            randomColors[i] = colors[randomIndex];
        }

        return randomColors;
    }

    public void RefreshMold()
    {
        for(int i = 0; i < row; i++)
            for(int j = 0; j < col; j++)
                Destroy(objects[i, j]);
        
        InitMold();
    }

    private void InitMold()
    {
        int cnt = 0;
        objects = new GameObject[7, 7];

        float x = -405;
        float y = 405;
        float interval = 135;
        
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                //GameObject colorInst = Instantiate(colorObject, mold);

                Vector3 targetPos = new Vector3(x, y, 0);
                if (j < col - 1)
                    x += interval;
                else
                {
                    x = -405;
                    y -= interval;
                }
                
                GameObject colorInst = Instantiate(colorObject, mold);
                colorInst.transform.localPosition = targetPos;
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

                objects[i, j] = colorInst;
                cnt++;
            }
        }
        
        LoadAudio();
    }

    void Update()
    {
        if (!isGaming) return;
        time -= Time.deltaTime;
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        timeText.text = string.Format("{0:0}:{1:00}", minutes, seconds);
        scoreText.text = score.ToString();
        if (time <= 0f)
            TimeOver();

        OnInputHandler();
    }

    private void TimeOver()
    {
        isGaming = false;
        PlayAudio(clips[1]);
        timeText.text = "0:00";
        overScoreText.text = score.ToString() + "점";
        combinationPanel.SetActive(false);
        if(PlayerPrefs.GetInt("ChallengeBestScore") < score)
            PlayerPrefs.SetInt("ChallengeBestScore", score);
        PlayerPrefs.SetInt("Coin", PlayerPrefs.GetInt("Coin") + score * 3);

        overBackground.SetActive(true);
        overPanel.SetActive(true);
        overPanel.transform.localScale = Vector3.one * 0.1f;
        overPanel.transform.DOScale(Vector3.one, 1f);
    }

    private void OnInputHandler()
    {
        if (isMoving) return;
        
        if (inputManager.isTouchDown)
        {
            clickPos = inputManager.touchPosition;
            startBlock = CheckBlock(clickPos);
        } else if (inputManager.isTouchUp)
        {
            Swipe swipeDir = inputManager.EvalSwipeDir(clickPos, inputManager.touchPosition);
            ColorObject.OutlineType type = startBlock.GetComponent<ColorObject>().type;

            // SWIPE
            // GetEndBlock -> MoveAnimation -> Mix
            if (CheckMovable(startBlock, swipeDir, type))
            {
                endBlock = GetEndBlock(inputManager.touchPosition);
                if (endBlock.tag != "Block") return;
                if (Vector2.Distance(startBlock.transform.position, endBlock.transform.position) > 135f) return;
                MoveAnimation(startBlock, swipeDir);
                endBlock.GetComponent<AudioSource>().Play();
            }
        }
    }
    

    private void MoveAnimation(GameObject target, Swipe direction)
    {
        isMoving = true;
        Color startColor = startBlock.GetComponent<Image>().color;
        Color endColor = endBlock.GetComponent<Image>().color;

        Color targetColor = MixColor(startColor, endColor);
        recentColor = targetColor;
        Debug.Log("섞인 색 : " + targetColor);

        startBlock.GetComponent<Image>().DOColor(targetColor, 0.5f);
        endBlock.GetComponent<Image>().DOColor(targetColor, 0.5f);
        
        var seq = DOTween.Sequence();
        
        if (direction == Swipe.UP)
        {
            seq.Append(target.transform.DOMoveY(target.transform.position.y + 135f, animationSpeed));
        } else if (direction == Swipe.DOWN)
        {
            seq.Append(target.transform.DOMoveY(target.transform.position.y - 135f, animationSpeed));
            
        } else if (direction == Swipe.LEFT)
        {
            seq.Append(target.transform.DOMoveX(target.transform.position.x - 135f, animationSpeed));

        } else if (direction == Swipe.RIGHT)
        {
            seq.Append(target.transform.DOMoveX(target.transform.position.x + 135f, animationSpeed));
        }

        seq.InsertCallback(animationSpeed, () =>
        {
            CheckClear();
        });

        seq.Play();
    }
    
    private Color MixColor(Color color1, Color color2)
    {
        Debug.Log("첫번째 색 : " + color1);
        Debug.Log("두번째 색 : " + color2);
        

        float r = 255f - Mathf.Sqrt(((255f - color1.r * 255f) * (255f - color1.r * 255f) + (255f - color2.r * 255f) * (255f - color2.r * 255f)) / 2f);
        float g = 255f - Mathf.Sqrt(((255f - color1.g * 255f) * (255f - color1.g * 255f) + (255f - color2.g * 255f) * (255f - color2.g * 255f)) / 2f);
        float b = 255f - Mathf.Sqrt(((255f - color1.b * 255f) * (255f - color1.b * 255f) + (255f - color2.b * 255f) * (255f - color2.b * 255f)) / 2f);

        return new Color(r / 255f, g / 255f, b / 255f);
    }
    
    private GameObject GetEndBlock(Vector2 targetPos)
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = targetPos;
        
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, results);

        foreach (var result in results)
        {
            if (result.gameObject.tag == "Block" || result.gameObject != startBlock)
                return result.gameObject;
        }

        return null;
    }

    private bool CheckMovable(GameObject target, Swipe swipeDir, ColorObject.OutlineType type)
    {
        Vector2 position = target.transform.position;

        if (swipeDir == Swipe.NA)
        {
            return false;
        } else if (swipeDir == Swipe.UP)
        {
            if (type == ColorObject.OutlineType.Top || type == ColorObject.OutlineType.TopLeft ||
                type == ColorObject.OutlineType.TopRight)
            {
                return false;
            }

            if (!CheckBlock(position + Vector2.up * 130f))
                return false;
        } else if (swipeDir == Swipe.DOWN)
        {
            if (type == ColorObject.OutlineType.Bottom || type == ColorObject.OutlineType.BottomLeft ||
                type == ColorObject.OutlineType.BottomRight)
            {
                return false;
            }
            if (!CheckBlock(position + Vector2.down * 130f))
                return false;
        } else if (swipeDir == Swipe.LEFT)
        {
            if (type == ColorObject.OutlineType.Left || type == ColorObject.OutlineType.TopLeft ||
                type == ColorObject.OutlineType.BottomLeft)
            {
                return false;
            }
            if (!CheckBlock(position + Vector2.left * 130f))
                return false;
        } else if (swipeDir == Swipe.RIGHT)
        {
            if (type == ColorObject.OutlineType.Right || type == ColorObject.OutlineType.TopRight ||
                type == ColorObject.OutlineType.BottomRight)
            {
                return false;
            }
            if (!CheckBlock(position + Vector2.right * 130f))
                return false;
        }

        return true;
    }

    private GameObject CheckBlock(Vector2 targetPos)
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = targetPos;
        
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, results);

        foreach (var result in results)
        {
            if (result.gameObject.tag == "Block")
                return result.gameObject;
        }

        return null;
    }

    public void UseTimeItem()
    {
        if (PlayerPrefs.GetInt("TimeItem") < 1) return;
        
        PlayerPrefs.SetInt("TimeItem", PlayerPrefs.GetInt("TimeItem") - 1);
        time += 30f;
        timeEffect.color = Color.white;
        timeEffect.gameObject.SetActive(true);
        timeEffect.DOFade(0, 1f).OnComplete(() =>
        {
            timeEffect.gameObject.SetActive(false);
        });
        timeText.transform.DOScale(Vector3.one * 1.4f, .2f);
        timeText.transform.DOScale(Vector3.one, .8f);
        InitItems();
    }
    
    public void UsePassItem()
    {
        if (PlayerPrefs.GetInt("PassItem") < 1) return;
        
        PlayerPrefs.SetInt("PassItem", PlayerPrefs.GetInt("PassItem") - 1);
        GenerateTarget();
        
        InitItems();
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
    
    public void LoadAudio()
    {
        bool onoff = true;
        if (!PlayerPrefs.HasKey("Audio"))
        {
            PlayerPrefs.SetInt("Audio", 1);
        }
        else
        {
            onoff = (PlayerPrefs.GetInt("Audio") == 1) ? true : false;
        }

        OnOffAllAudio(onoff);
        SetAudioOnOff(!onoff);
    }
    
    public void SetAudio()
    {
        bool audioEnabled = true;
        if (PlayerPrefs.GetInt("Audio") == 1)
        {
            SetAudioOnOff(true);
            PlayerPrefs.SetInt("Audio", 0);
            audioEnabled = false;
        }
        else
        {
            SetAudioOnOff(false);
            PlayerPrefs.SetInt("Audio", 1);
            audioEnabled = true;
        }

        OnOffAllAudio(audioEnabled);
    }

    private void SetAudioOnOff(bool target)
    {
        audioOff.SetActive(target);
    }

    private void OnOffAllAudio(bool target)
    {
        AudioSource[] audios = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audio in audios)
        {
            audio.volume = target ? 1 : 0;
        }
    }

    public void GenerateCombinations()
    {
        for (int i = 0; i < colors.Length; i++)
        {
            for (int j = i; j < colors.Length; j++)
            {
                GameObject combination = Instantiate(combinationObject, combinationContent);
                Image start = combination.transform.GetChild(0).GetComponent<Image>();
                Image end = combination.transform.GetChild(1).GetComponent<Image>();
                Image mixed = combination.transform.GetChild(2).GetComponent<Image>();

                start.color = colors[i];
                end.color = colors[j];
                mixed.color = MixColor(start.color, end.color);
            }
        }
    }
}

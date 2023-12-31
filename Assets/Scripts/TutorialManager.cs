using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    public TMP_Text guideText;
    public string[] guides;
    public int idx = -1;
    public bool canGo = true;
    public GameObject guidePanel;
    
    public bool isMoving = false;
    
    public Transform mold;
    public GameObject colorObject;
    private int row = 7;
    private int col = 7;
    public GameObject[,] objects;
    public Color[] colors;
    
    private InputManager inputManager;
    private bool canMove = false;
    public Vector2 clickPos;
    public GameObject startBlock;
    public GameObject endBlock;
    
    public float animationSpeed = 0.5f;
    public Color recentColor;

    public GameObject refreshIcon;
    public GameObject combIcon;

    public GameObject item1;
    public GameObject item2;
    
    void Start()
    {
        inputManager = new InputManager();
        GetNextGuide();
    }

    public void GetNextGuide()
    {
        if (!canGo) return;
        
        idx++;
        if (idx >= guides.Length)
        {
            SceneManager.LoadScene(0);
        }
        
        guideText.color = new Color(1f, 1f, 1f, 0);
        guideText.text = guides[idx];
        guideText.DOFade(1f, 1);
        
        WatchIndex();
    }

    private void WatchIndex()
    {
        if (idx == 3)
        {
            canGo = false;
            guidePanel.SetActive(false);
            InitMold();
        } else if (idx == 5)
        { 
            for(int i = 0; i < row; i++)
                for(int j = 0; j < col; j++)
                    Destroy(objects[i, j]);
        } else if (idx == 8)
        {
            item1.SetActive(true);
            item2.SetActive(true);
        } else if (idx == 9)
        {
            item1.SetActive(false);
            item2.SetActive(false);
        }else if(idx == 10)
        {
            combIcon.SetActive(true);
        } else if (idx == 11)
        {
            combIcon.SetActive(false);
            refreshIcon.SetActive(true);
        } else if (idx == 12)
        {
            refreshIcon.SetActive(false);
        }
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
    }

    void Update()
    {
        OnInputHandler();
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
        canGo = true;
        guidePanel.SetActive(true);
        GetNextGuide();
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
    
    private void CheckClear()
    {
        Destroy(startBlock);
        isMoving = false;
    }
}
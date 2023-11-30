using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ColorObject : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public enum OutlineType { Top, Bottom, Left, Right, Inside, TopLeft, TopRight, BottomLeft, BottomRight }
    
    private Transform canvas;
    public RectTransform rect;
    public Vector3 prevRect;
    public bool isDragStarted = false;
    public bool direction = false;

    public OutlineType type;

    void Awake()
    {
        canvas = FindObjectOfType<Canvas>().transform;
        rect = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData data)
    {
        prevRect = rect.transform.position;
    }

    public void OnDrag(PointerEventData data)
    {   
        Vector2 delta = data.delta;

        bool direct = Mathf.Abs(delta.x) > Mathf.Abs(delta.y);
        if (!isDragStarted)
        {
            isDragStarted = true;
            direction = direct;
        }
        
        if (type == OutlineType.Top && delta.y > 0)
            return;
        if (type == OutlineType.Bottom && delta.y < 0)
            return;
        if (type == OutlineType.Left && delta.x < 0)
            return;
        if (type == OutlineType.Right && delta.x > 0)
            return;
        if (type == OutlineType.TopLeft && (delta.x < 0 || delta.y > 0))
            return;
        if (type == OutlineType.TopRight && (delta.x > 0 || delta.y > 0))
            return;
        if (type == OutlineType.BottomLeft && (delta.x < 0 || delta.y < 0))
            return;
        if (type == OutlineType.BottomRight && (delta.x > 0 || delta.y < 0))
            return;

        if (direct)
        {
            if(direction)
                rect.position += new Vector3(delta.x, 0, 0);
        }
        else
        {
            if(!direction)
                rect.position += new Vector3(0, delta.y, 0);
        }
        
        Vector3 dragDistance = rect.position - prevRect;
        
        if (dragDistance.magnitude >= 130f)
        {
            rect.position = prevRect + dragDistance.normalized * 130f;
        }
    }

    public void OnEndDrag(PointerEventData data)
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = rect.position;

        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, results);

        foreach (var result in results)
        {
            Debug.Log("Clicked UI Object: " + result.gameObject.name);
            if (result.gameObject == gameObject)
            {
                Debug.Log("SELF");
            }
        }
        isDragStarted = false;
        rect.position = prevRect;
    }
}
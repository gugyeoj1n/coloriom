using UnityEngine;
using UnityEngine.EventSystems;

public class ColorObject : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform canvas;
    public RectTransform rect;
    public Vector3 prevRect;

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
        rect.position = data.position;
    }

    public void OnEndDrag(PointerEventData data)
    {
        rect.position = prevRect;
    }
}
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ColorObject : MonoBehaviour
{
    public enum OutlineType { Top, Bottom, Left, Right, Inside, TopLeft, TopRight, BottomLeft, BottomRight }
    
    public OutlineType type;

    void Awake()
    {
        
    }
}
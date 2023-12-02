using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager
{
#if UNITY_EDITOR
    private IInputHandler inputHandler = new MouseHandler();
#else
    private IInputHandler inputHandler = new TouchHandler();
#endif
    public bool isTouchDown => inputHandler.isInputDown;
    public bool isTouchUp => inputHandler.isInputUp;
    public Vector2 touchPosition => inputHandler.inputPosition;
    
    public Swipe EvalSwipeDir(Vector2 vtStart, Vector2 vtEnd)
    {
        return TouchEvaluator.EvalSwipeDir(vtStart, vtEnd);
    }
}
    
public interface IInputHandler
{
    bool isInputDown { get; }
    bool isInputUp { get; }
    Vector2 inputPosition { get; }
}

public class TouchHandler : IInputHandler
{
    bool IInputHandler.isInputDown => Input.GetTouch(0).phase == TouchPhase.Began;
    bool IInputHandler.isInputUp => Input.GetTouch(0).phase == TouchPhase.Ended;
    Vector2 IInputHandler.inputPosition => Input.GetTouch(0).position;
}

public class MouseHandler : IInputHandler
{
    bool IInputHandler.isInputDown => Input.GetButtonDown("Fire1");
    bool IInputHandler.isInputUp => Input.GetButtonUp("Fire1");
 
    Vector2 IInputHandler.inputPosition => Input.mousePosition;
}
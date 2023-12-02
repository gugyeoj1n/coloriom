using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Swipe
{
    NA = -1,
    RIGHT = 0,
    UP = 1,
    LEFT = 2,
    DOWN = 3
}

public static class TouchEvaluator
{
    public static Swipe EvalSwipeDir(Vector2 start, Vector2 end)
    {
        if (Vector2.Distance(start, end) < 70f)
            return Swipe.NA;
        
        float angle = EvalDragAngle(start, end);
        if (angle < 0)
            return Swipe.NA;
 
        int swipe = (((int)angle + 45) % 360) / 90;
 
        switch (swipe)
        {
            case 0: return Swipe.RIGHT;
            case 1: return Swipe.UP;
            case 2: return Swipe.LEFT;
            case 3: return Swipe.DOWN;
        }
 
        return Swipe.NA;
    }

    public static float EvalDragAngle(Vector2 start, Vector2 end)
    {
        Vector2 direction = end - start;
        if (direction.magnitude <= 0.2f)
            return -1f;
 
        float aimAngle = Mathf.Atan2(direction.y, direction.x);
        if (aimAngle < 0f)
        {
            aimAngle = Mathf.PI * 2 + aimAngle;
        }
 
        return aimAngle * Mathf.Rad2Deg;
    }
}
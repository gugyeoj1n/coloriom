using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TutorialIconCircle : MonoBehaviour
{
    void Start()
    {
        DOTween.Sequence()
            .Append(transform.DOScale(Vector3.one * 0.9f, 0.3f))
            .Append(transform.DOScale(Vector3.one, 0.3f))
            .AppendInterval(1.0f)
            .SetLoops(-1);
    }
}

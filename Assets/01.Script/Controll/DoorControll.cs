using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DoorControll : ControllAbleObject
{
    [SerializeField] private Vector3 closePos;
    [SerializeField] private Vector3 openPos;
    public void Reset()
    {
        closePos = transform.localPosition;
        openPos = transform.localPosition;
    }
    public override void ControllNegative()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOLocalMove(closePos, time));
    }

    public override void ControllPositive()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOLocalMove(openPos, time));
    }
}

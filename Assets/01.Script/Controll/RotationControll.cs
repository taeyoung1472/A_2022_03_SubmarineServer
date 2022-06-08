using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RotationControll : ControllAbleObject
{
    [SerializeField] private Vector3 positiveAngle;
    [SerializeField] private Vector3 negativeAngle;
    public void Reset()
    {
        positiveAngle = transform.localEulerAngles;
        negativeAngle = transform.localEulerAngles;
    }
    public override void ControllNegative()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOLocalRotate(negativeAngle, time));
    }

    public override void ControllPositive()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOLocalRotate(positiveAngle, time));
    }
}

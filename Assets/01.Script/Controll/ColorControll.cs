using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ColorControll : ControllAbleObject
{
    [SerializeField] private Color positiveColor = Color.white;
    [SerializeField] private Color negativeColor = Color.white;
    Light light;
    private void Awake()
    {
        light = GetComponent<Light>();
    }
    public override void ControllNegative()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(light.DOColor(negativeColor, time));
    }

    public override void ControllPositive()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(light.DOColor(positiveColor, time));
    }
}

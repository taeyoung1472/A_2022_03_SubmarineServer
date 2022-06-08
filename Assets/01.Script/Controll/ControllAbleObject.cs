using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using UnityEngine.Events;

public abstract class ControllAbleObject : MonoBehaviour
{
    [SerializeField] protected float time = 0.5f;
    public abstract void ControllPositive();
    public abstract void ControllNegative();
}

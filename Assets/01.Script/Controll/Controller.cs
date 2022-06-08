using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
public class Controller : MonoBehaviour
{
    [SerializeField] private UnityEvent positiveControllAbleObjects;
    [SerializeField] private UnityEvent negativeControllAbleObjects;
    public UnityEvent PositiveControllAbleObjects { get { return positiveControllAbleObjects; } }
    public UnityEvent NegativeControllAbleObjects { get { return negativeControllAbleObjects; } }
    public void PositiveControll()
    {
        positiveControllAbleObjects?.Invoke();
    }
    public void NegativeControll()
    {
        NegativeControllAbleObjects?.Invoke();
    }
}

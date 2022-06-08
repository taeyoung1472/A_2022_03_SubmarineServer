using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ControllPannel : MonoBehaviour
{
    protected int postiveAction = 0, negativeAction = 0;
    [SerializeField] protected Controller controller;
    public abstract void ControllPositive();
    public abstract void ControllNegative();
    public abstract void ControllPositiveCallback();
    public abstract void ControllNegativeCallback();
    public abstract void OnControllPositiveCallback();
    public abstract void OnControllNegativeCallback();
}

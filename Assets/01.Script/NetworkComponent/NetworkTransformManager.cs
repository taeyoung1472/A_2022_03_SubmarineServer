using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkTransformManager : MonoBehaviour
{
    [SerializeField] private Dictionary<int, NetworkTransform> netTrans = new Dictionary<int, NetworkTransform>();
    public Dictionary<int, NetworkTransform> NetTrans { get { return netTrans; } }
    public static NetworkTransformManager instance;
    public void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
}

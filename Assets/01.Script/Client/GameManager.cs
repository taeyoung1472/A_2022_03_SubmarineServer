using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static Dictionary<int, Controller> controllPannels = new Dictionary<int, Controller>();
    [SerializeField] private List<Controller> controlls = new List<Controller>();
    public void Start()
    {
        int i = 0;
        foreach (Controller item in controlls)
        {
            controllPannels[i] = item;
            i++;
        }
        foreach (Controller item in controllPannels.Values)
        {
            print(item.gameObject.name);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantObjectManager : MonoBehaviour
{
    public static InstantObjectManager Instance;
    [SerializeField] private GameObject[] objects;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    private void FixedUpdate()
    {
        
    }
}
public enum ObjectEnum
{
    Test,
}
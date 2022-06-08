using UnityEngine;
using System;
[Serializable]
public class UserData
{
    public UserData(string _name)
    {
        userName = _name;
    }
    [SerializeField] private string userName = "";
    [SerializeField] private bool isReady = false;
    [SerializeField] private bool isLeader = false;
    public string UserName { get { return userName; } set { userName = value; } }
    public bool IsReady { get { return isReady; } set { isReady = value; } }
    public bool IsLeader { get { return isLeader; } set { isLeader = value; } }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    public static LobbyManager instance;
    Dictionary<string, UserData> users = new Dictionary<string, UserData>();
    public void AddUser(UserData data)
    {
        users.Add(data.UserName, data);
        UpdateUserData();
    }
    public void MinusUser(UserData data)
    {
        users.Remove(data.UserName);
        UpdateUserData();
    }
    public void Ready(string userName, bool isReady)
    {
        users[userName].IsReady = isReady;
        UpdateUserData();
    }
    public void UpdateUserData()
    {
        int length = users.Count;
        List<UserData> datas = new List<UserData>();
        foreach (UserData data in users.Values)
        {
            datas.Add(data);
        }
    }
    /*public static UserManager instance;
    Dictionary<string, UserData> users = new Dictionary<string, UserData>();

    public void Awake()
    {
        if(instance == null)
            instance = this;
    }
    public void JoinUser(UserData data)
    {
        users.Add(data.UserName, data);
        UpdateUserData();
    }
    public void Ready(string name, bool value)
    {
        users[name].IsReady = value;
        UpdateUserData();
    }
    public void UpdateUserData()
    {
        ServerSend.LobbyUpdate(users);
    }*/
}

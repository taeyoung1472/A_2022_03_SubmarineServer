using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager Instance;

    public GameObject playerPrefab;
    public GameObject projectilePrefab;
    public GameObject enemyProjectilePrefab;
    public GameObject enemyPrefab;
    [SerializeField] private GameObject submarine;
    public GameObject Submarine { get { return submarine; } }

    [SerializeField] private InputField inputField;
    [SerializeField] private Vector3 spawnPos;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }
    public void SetIpType(bool _isServer)
    {
        Server.isServer = _isServer;
    }
    public void StartServer()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 144;
        if (Server.isServer)
        {
            int port;
            try
            {
                port = Int32.Parse(inputField.text);
            }
            catch
            {
                port = 26950;
                //return;
            }
            Server.Start(50, port);
        }
        else
        {
            Server.Start(50, 26950);
        }
    }

    private void OnApplicationQuit()
    {
        Server.Stop();
    }
    public Player InstantiatePlayer()
    {
        GameObject obj = Instantiate(playerPrefab, spawnPos, Quaternion.identity);
        obj.transform.SetParent(submarine.transform);
        return obj.GetComponent<Player>();
    }
    public void InstantiateEnemy(Vector3 _position)
    {
        Instantiate(enemyPrefab, _position, Quaternion.identity);
    }
}

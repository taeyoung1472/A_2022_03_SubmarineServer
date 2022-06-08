using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SyncManager : MonoBehaviour
{
    public static SyncManager instance;
    int netTransCount;
    public int NetTransCount { get { return netTransCount; } }
    NetworkTransformManager netTransManager;
    public void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    public void Start()
    {
        netTransManager = NetworkTransformManager.instance;
        StartCoroutine(Syncing());
    }
    IEnumerator Syncing()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(1f);
            netTransCount = netTransManager.NetTrans.Count;
            ServerSend.Sync(this);
        }
    }
    public void GetSyncReturnData(int fromClient, Packet data)
    {
        bool isErrorByNetTrans = data.ReadBool();
        if (!isErrorByNetTrans)
        {
            foreach (NetworkTransform netTrans in netTransManager.NetTrans.Values)
            {
                var key = netTransManager.NetTrans.FirstOrDefault(x => x.Value == netTrans).Key;
                ServerSend.NetworkTransformInit(key, netTrans.name, fromClient);
            }
        }
    }
}

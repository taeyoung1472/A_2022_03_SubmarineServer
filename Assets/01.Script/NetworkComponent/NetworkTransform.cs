using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkTransform : MonoBehaviour
{
    [Header("통신 목록")]
    [SerializeField] private bool position; 
    [SerializeField] private bool rotation;
    //private int id;
    private bool isValueChanging = true;
    //public int Id { set { id = value; } }
    [SerializeField] private bool isStandardNetworkTransform;
    public bool IsStandardNetworkTransform { get { return isStandardNetworkTransform; } }
    static int staticId = 0;
    int instanceId = 0;
    private Vector3 prevPos;
    private Quaternion prevRot;
    public void Start()
    {
        NetworkTransformManager manager = NetworkTransformManager.instance;
        manager.NetTrans.Add(staticId, this);
        instanceId = staticId;
        staticId++;
        prevPos = transform.position;
        prevRot = transform.rotation;
        SendTransform(true);
    }
    public void InitInfos(int targetClient)
    {
        //ServerSend.NetworkTransformInit(instanceId, gameObject.name, targetClient);
    }
    public void FixedUpdate()
    {
        SendTransform();
    }
    public void SendTransform(bool isFirst = false)
    {
        if (position)
        {
            ServerSend.NetworkPosition(instanceId, transform.position);
        }
        if (rotation)
        {
            ServerSend.NetworkRotation(instanceId, transform.rotation);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerHandle
{
    /// <summary>
    /// ���� ���� ������ �÷��̾ �ִ� ��Ŷ
    /// </summary>
    /// <param name="_fromClient"></param>
    /// <param name="_packet"></param>
    public static void WelcomeReceived(int _fromClient, Packet _packet)
    {
        #region ���ʼ���
        int _clientIdCheck = _packet.ReadInt();
        string _userName = _packet.ReadString();
        ServerSend.TextSend(2, $"Player {_userName} has Connected.", true);

        Debug.Log($"{Server.clients[_fromClient].tcp.socket.Client.RemoteEndPoint} Has Connected, Now Players : {_fromClient}");
        if (_fromClient != _clientIdCheck)
        {
            Debug.Log($"Player \"{_userName}\" (ID : {_fromClient}) Has Send Erroed Client ID{_clientIdCheck}.");
        }
        Server.clients[_fromClient].SendIntoGame(_userName);
        UIManager.Instance.AddPlayer(_fromClient, _userName);
        #endregion
        foreach (NetworkTransform item in NetworkTransformManager.instance.NetTrans.Values)
        {
            if (item.IsStandardNetworkTransform)
            {
                item.InitInfos(_fromClient);
            }
        }
    }

    /// <summary>
    /// �÷��̾��� ������ ���ɿ��θ� �����ϴ� ��Ŷ
    /// </summary>
    /// <param name="_fromClient"></param>
    /// <param name="_packet"></param>
    public static void HandlePlayerMove(int _fromClient, Packet _packet)
    {
        bool isMove = _packet.ReadBool();

        Server.clients[_fromClient].player.IsCanMove = isMove;
    }

    /// <summary>
    /// �÷��̾��� Input�� �о���� ��Ŷ
    /// </summary>
    /// <param name="_fromClient"></param>
    /// <param name="_packet"></param>
    public static void PlayerMovement(int _fromClient, Packet _packet)
    {
        bool[] _inputs = new bool[_packet.ReadInt()];
        for (int i = 0; i < _inputs.Length; i++)
        {
            _inputs[i] = _packet.ReadBool();
        }
        Quaternion _rotation = _packet.ReadQuaternion();

        Server.clients[_fromClient].player.SetInput(_inputs, _rotation);
    }

    /// <summary>
    /// �÷��̾ ������ ���� �ʾƼ� �ٽ� ���� ��û�ϴ� ��Ŷ
    /// </summary>
    /// <param name="_fromClient"></param>
    /// <param name="_packet"></param>
    public static void PlayerSpawnAgain(int _fromClient, Packet _packet)
    {
        int id = _packet.ReadInt();
        ServerSend.SpawnPlayer(_fromClient, Server.clients[id].player);
    }

    /// <summary>
    /// ������� Input�� �о���� ��Ŷ
    /// </summary>
    /// <param name="_fromClient"></param>
    /// <param name="_packet"></param>
    public static void SubmarineMovement(int _fromClient, Packet _packet)
    {
        bool[] _inputs = new bool[_packet.ReadInt()];
        for (int i = 0; i < _inputs.Length; i++)
        {
            _inputs[i] = _packet.ReadBool();
        }

        Submarine.Instance.SetInput(_inputs);
    }

    /// <summary>
    /// �÷��̾ ģ ä�� ������ �������ִ� ��Ŷ
    /// </summary>
    /// <param name="fromClient"></param>
    /// <param name="packet"></param>
    public static void SendedText(int fromClient, Packet packet)
    {
        ServerSend.TextSend(fromClient, packet.ReadString(), false);
    }

    /// <summary>
    /// �÷��̾ ��û�� ������� ó���ϴ� ��Ŷ
    /// </summary>
    /// <param name="fromClient"></param>
    /// <param name="packet"></param>
    public static void SendedAudio(int fromClient, Packet packet)
    {
        int id = packet.ReadInt();
        Vector3 pos = packet.ReadVector3();
        ServerSend.AudioSend(id, pos, fromClient);
    }

    /// <summary>
    /// Contoll(��, ����)���� �����ϴ� ������ ���� ��Ŷ
    /// </summary>
    /// <param name="fromClient"></param>
    /// <param name="packet"></param>
    public static void Controll(int fromClient, Packet packet)
    {
        int controllId = packet.ReadInt();
        bool isPostive = packet.ReadBool();
        if (isPostive)
        {
            GameManager.controllPannels[controllId].PositiveControll();
        }
        else
        {
            GameManager.controllPannels[controllId].NegativeControll();
        }
    }

    /// <summary>
    /// NetworkObjects�� ����ȭ�� å������ ��Ŷ
    /// </summary>
    /// <param name="fromClient"></param>
    /// <param name="packet"></param>
    public static void Sync(int fromClient, Packet packet)
    {
        SyncManager.instance.GetSyncReturnData(fromClient, packet);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerHandle
{
    /// <summary>
    /// 서버 연결 성공시 플레이어가 주는 패킷
    /// </summary>
    /// <param name="_fromClient"></param>
    /// <param name="_packet"></param>
    public static void WelcomeReceived(int _fromClient, Packet _packet)
    {
        #region 기초설정
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
    /// 플레이어의 움직임 가능여부를 제어하는 패킷
    /// </summary>
    /// <param name="_fromClient"></param>
    /// <param name="_packet"></param>
    public static void HandlePlayerMove(int _fromClient, Packet _packet)
    {
        bool isMove = _packet.ReadBool();

        Server.clients[_fromClient].player.IsCanMove = isMove;
    }

    /// <summary>
    /// 플레이어의 Input을 읽어오는 패킷
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
    /// 플레이어가 정상스폰 되지 않아서 다시 스폰 요청하는 패킷
    /// </summary>
    /// <param name="_fromClient"></param>
    /// <param name="_packet"></param>
    public static void PlayerSpawnAgain(int _fromClient, Packet _packet)
    {
        int id = _packet.ReadInt();
        ServerSend.SpawnPlayer(_fromClient, Server.clients[id].player);
    }

    /// <summary>
    /// 잠수함의 Input을 읽어오는 패킷
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
    /// 플레이어가 친 채팅 정보를 전달해주는 패킷
    /// </summary>
    /// <param name="fromClient"></param>
    /// <param name="packet"></param>
    public static void SendedText(int fromClient, Packet packet)
    {
        ServerSend.TextSend(fromClient, packet.ReadString(), false);
    }

    /// <summary>
    /// 플레이어가 요청한 오디오를 처리하는 패킷
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
    /// Contoll(문, 전등)등을 제어하는 정보를 가진 패킷
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
    /// NetworkObjects의 동기화를 책임지는 패킷
    /// </summary>
    /// <param name="fromClient"></param>
    /// <param name="packet"></param>
    public static void Sync(int fromClient, Packet packet)
    {
        SyncManager.instance.GetSyncReturnData(fromClient, packet);
    }
}

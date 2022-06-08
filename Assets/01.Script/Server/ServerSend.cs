using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerSend
{
    #region 전달로직
    public static void SendTCPData(int _toClient, Packet _packet)
    {
        _packet.WriteLength();
        Server.clients[_toClient].tcp.SendData(_packet);
    }
    public static void SendUDPData(int _toClient, Packet _packet)
    {
        _packet.WriteLength();
        Server.clients[_toClient].udp.SendData(_packet);
    }
    public static void SendUDPDataToAll(Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            Server.clients[i].udp.SendData(_packet);
        }
    }
    public static void SendUDPDataToAll(int _exceptClient, Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            if (i != _exceptClient)
            {
                Server.clients[i].udp.SendData(_packet);
            }
        }
    }
    #endregion
    #region 패킷들
    
    /// <summary>
    /// 플레이어가 서버에 입장시 정보를 전달해주는 함수
    /// </summary>
    /// <param name="_toClient"></param>
    /// <param name="_msg"></param>
    public static void Welcome(int _toClient, string _msg)
    {
        using (Packet _packet = new Packet((int)ServerPackets.welcome))
        {
            _packet.Write(_msg);
            _packet.Write(_toClient);

            SendTCPData(_toClient, _packet);
        }
    }

    /// <summary>
    /// 로비의 전반적 정보를 동기화 시켜주는 패킷
    /// </summary>
    /// <param name="_toClient"></param>
    /// <param name="_msg"></param>
    public static void LobbyUpdate(UserData[] datas)
    {
        using (Packet _packet = new Packet((int)ServerPackets.lobby_Update))
        {
            _packet.Write(datas.Length);
            foreach (UserData data in datas)
            {
                _packet.Write(data.UserName);
                _packet.Write(data.IsReady);
                _packet.Write(data.IsLeader);
            }

            SendUDPDataToAll(_packet);
        }
    }

    /// <summary>
    /// 플레이어의 스폰을 명령하는 함수
    /// </summary>
    /// <param name="_toClient"></param>
    /// <param name="_player"></param>
    public static void SpawnPlayer(int _toClient, Player _player)
    {
        using (Packet _packet = new Packet((int)ServerPackets.spawnPlayer))
        {
            _packet.Write(_player.id);
            _packet.Write(_player.username);
            _packet.Write(_player.transform.position);
            _packet.Write(_player.transform.rotation);
            SendUDPData(_toClient, _packet);
        }
    }

    /// <summary>
    /// 모든 플레이어들의 위치/회전 정보를 동기화 시키는 함수
    /// </summary>
    /// <param name="_player"></param>
    /// <param name="_moveDir"></param>
    public static void PlayerPositionAndRotation(Player _player, Vector2 _moveDir = default)
    {
        using (Packet _packet = new Packet((int)ServerPackets.playerPositionAndRotation))
        {
            _packet.Write(_player.id);
            _packet.Write(_player.transform.position);
            _packet.Write(_player.transform.rotation);
            _packet.Write(_moveDir);
            SendUDPDataToAll(_packet);
        }
    }

    /// <summary>
    /// Map의 위치와 회전정보를 동기화 시키는 함수
    /// </summary>
    /// <param name="info"></param>
    public static void MapPositionAndRotation(Transform info)
    {
        using (Packet _packet = new Packet((int)ServerPackets.mapPositionAndRotation))
        {
            _packet.Write(info.transform.position);
            _packet.Write(info.transform.rotation);
            SendUDPDataToAll(_packet);
        }
    }

    /// <summary>
    /// 플레이어가 서버를 나갈시 다른 플레이어들에개 알리는 함수
    /// </summary>
    /// <param name="_playerId"></param>
    public static void PlayerDisconnected(int _playerId)
    {
        using (Packet _packet = new Packet((int)ServerPackets.playerDisconnected))
        {
            _packet.Write(_playerId);

            SendUDPDataToAll(_packet);
        }
    }

    /// <summary>
    /// 플레이어에개 전달받은 채팅 정보를 모두에개 전달하는 함수
    /// </summary>
    /// <param name="id"></param>
    /// <param name="text"></param>
    /// <param name="isServer"></param>
    public static void TextSend(int id, string text, bool isServer)
    {
        using (Packet _packet = new Packet((int)ServerPackets.textSended))
        {
            _packet.Write(id);
            _packet.Write(text);
            _packet.Write(isServer);

            SendUDPDataToAll(_packet);
        }
    }

    /// <summary>
    /// 플레이어에개 전달받은 오디오 정보를 모두에개 전달하는 함수
    /// </summary>
    /// <param name="id"></param>
    /// <param name="pos"></param>
    /// <param name="fromClient"></param>
    public static void AudioSend(int id, Vector3 pos, int fromClient)
    {
        using (Packet _packet = new Packet((int)ServerPackets.audioSended))
        {
            _packet.Write(id);
            _packet.Write(pos);

            SendUDPDataToAll(fromClient, _packet);
        }
    }

    /// <summary>
    /// NetworkTransform의 위치를 동기화 해주는 함수
    /// </summary>
    /// <param name="id"></param>
    /// <param name="pos"></param>
    public static void NetworkPosition(int id, Vector3 pos)
    {
        using (Packet _packet = new Packet((int)ServerPackets.networkPosition))
        {
            _packet.Write(id);
            _packet.Write(pos);

            SendUDPDataToAll(_packet);
        }
    }

    /// <summary>
    /// NetworkTransform의 회전을 동기화 해주는 함수
    /// </summary>
    /// <param name="id"></param>
    /// <param name="rot"></param>
    public static void NetworkRotation(int id, Quaternion rot)
    {
        using (Packet _packet = new Packet((int)ServerPackets.networkRotation))
        {
            _packet.Write(id);
            _packet.Write(rot);

            SendUDPDataToAll(_packet);
        }
    }

    /// <summary>
    /// NetworkTransform의 초기화를 시켜주는 함수???
    /// </summary>
    /// <param name="id"></param>
    /// <param name="name"></param>
    /// <param name="targetClient"></param>
    public static void NetworkTransformInit(int id, string name, int targetClient)
    {
        using (Packet _packet = new Packet((int)ServerPackets.networkTransformInit))
        {
            _packet.Write(id);
            _packet.Write(name);

            SendUDPData(targetClient, _packet);
        }
    }

    /// <summary>
    /// 게임의 전반적인 동기화 여부를 판단하는 함수
    /// </summary>
    /// <param name="data"></param>
    public static void Sync(SyncManager data)
    {
        using (Packet _packet = new Packet((int)ServerPackets.sync))
        {
            _packet.Write(data.NetTransCount);
            SendUDPDataToAll(_packet);
        }
    }
    #endregion
}

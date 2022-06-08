using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;

public class Client
{
    public static int dataBufferSize = 4097;
    public int id;
    public Player player;
    public TCP tcp;
    public UDP udp;
    public Client(int _clientId)
    {
        id = _clientId;
        tcp = new TCP(id);
        udp = new UDP(id);
    }
    public class TCP
    {
        public TcpClient socket;

        private readonly int id;
        private NetworkStream stream;
        private Packet receivedData;
        private byte[] receieveBuffer;

        public TCP(int _id)
        {
            id = _id;
        }
        public void Connect(TcpClient _socket)
        {
            socket = _socket;
            socket.ReceiveBufferSize = dataBufferSize;
            socket.SendBufferSize = dataBufferSize;

            stream = socket.GetStream();

            receivedData = new Packet();
            receieveBuffer = new byte[dataBufferSize];

            stream.BeginRead(receieveBuffer, 0, dataBufferSize, ReceiveCallback, null);

            ServerSend.Welcome(id, "Welcome To the Server!");
        }

        public void SendData(Packet _packet)
        {
            try
            {
                if (socket != null)
                {
                    stream.BeginWrite(_packet.ToArray(), 0, _packet.Length(), null, null);
                }
            }
            catch (Exception _ex)
            {
                Debug.Log($"Erroed To Sending Data To Player({id}) : VIA TCP {_ex}");
            }
        }

        private void ReceiveCallback(IAsyncResult _result)
        {
            try
            {
                int _byteLength = stream.EndRead(_result);
                if (_byteLength <= 0)
                {
                    Debug.Log($"Case 0 : 나감 : {_byteLength}");
                    Server.clients[id].Disconnect();
                    return;
                }
                byte[] _data = new byte[_byteLength];
                Array.Copy(receieveBuffer, _data, _byteLength);

                receivedData.Reset(HandledData(_data));
                stream.BeginRead(receieveBuffer, 0, dataBufferSize, ReceiveCallback, null);
            }
            catch (Exception ex)
            {
                Debug.Log("Case 1 : 나감");
                Debug.Log($"At TCP Data has errored : {ex}");
                Server.clients[id].Disconnect();
            }
        }
        private bool HandledData(byte[] _data)
        {
            int _packetLength = 0;

            receivedData.SetBytes(_data);

            if (receivedData.UnreadLength() >= 4)
            {
                _packetLength = receivedData.ReadInt();
                if (_packetLength <= 0)
                {
                    return true;
                }
            }
            while (_packetLength > 0 && _packetLength <= receivedData.UnreadLength())
            {
                byte[] _packetBytes = receivedData.ReadBytes(_packetLength);
                ThreadManager.ExecuteOnMainThread(() =>
                {
                    using (Packet _packet = new Packet(_packetBytes))
                    {
                        int _packetId = _packet.ReadInt();
                        Server.packetHandlers[_packetId](id, _packet);
                    }
                });

                _packetLength = 0;
                if (receivedData.UnreadLength() >= 4)
                {
                    _packetLength = receivedData.ReadInt();
                    if (_packetLength <= 0)
                    {
                        return true;
                    }
                }
            }
            if (_packetLength <= 1)
            {
                return true;
            }
            return false;
        }

        public void Disconnect()
        {
            socket.Close();
            stream = null;
            receivedData = null;
            receieveBuffer = null;
            socket = null;
        }
    }

    public class UDP
    {
        public IPEndPoint endPoint;

        private int id;

        public UDP(int _id)
        {
            id = _id;
        }

        public void Connect(IPEndPoint _endPoint)
        {
            endPoint = _endPoint;
        }
        public void SendData(Packet _packet)
        {
            Server.SendUDPData(endPoint, _packet);
        }
        public void HandleData(Packet _packetData)
        {
            int _packetLength = _packetData.ReadInt();
            byte[] _packetBytes = _packetData.ReadBytes(_packetLength);

            ThreadManager.ExecuteOnMainThread(() =>
            {
                using (Packet _packet = new Packet(_packetBytes))
                {
                    int _packetID = _packet.ReadInt();
                    Server.packetHandlers[_packetID](id, _packet);
                }
            });
        }

        public void Disconnect()
        {
            endPoint = null;
        }
    }

    public void SendIntoGame(string _playerName)
    {
        //이것들은 게임 시작부분으로 빼야할듯
        player = NetworkManager.Instance.InstantiatePlayer();
        player.Initialize(id, _playerName);
        LobbyManager.instance.AddUser(new UserData(_playerName));
        foreach (Client _client in Server.clients.Values)
        {
            if (_client.player != null)
            {
                if (_client.id != id)
                {
                    Debug.Log(id);
                    ServerSend.SpawnPlayer(id, _client.player);
                }
            }
        }

        foreach (Client _client in Server.clients.Values)
        {
            if (_client.player != null)
            {
                Debug.Log(id);
                ServerSend.SpawnPlayer(_client.id, player);
            }
        }

        //ItemSpawnere 없으니까 빼도 될듯
        /*foreach (ItemSpawner _itemSpawner in ItemSpawner.spawners.Values)
        {
            ServerSend.CreateItemSpawner(id, _itemSpawner.spawnerId, _itemSpawner.transform.position, _itemSpawner.hasItem);
        }*/
    }
    public void Disconnect()
    {
        Debug.Log($"{tcp.socket.Client.RemoteEndPoint} Has Disconnected.");

        ThreadManager.ExecuteOnMainThread(() =>
        {
            UnityEngine.Object.Destroy(player.gameObject);
            player = null;
        });

        tcp.Disconnect();
        udp.Disconnect();

        ServerSend.PlayerDisconnected(id);
        UIManager.Instance.MinusPlayer(id);
    }
}

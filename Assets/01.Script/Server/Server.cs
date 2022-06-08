using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class Server
{
    public static int MaxPlayers { get; private set; }
    public static int Port { get; private set; }
    public static Dictionary<int, Client> clients = new Dictionary<int, Client>();
    public delegate void PacketHandler(int _fromClient, Packet _packet);
    public static Dictionary<int, PacketHandler> packetHandlers;

    private static TcpListener tcpListener;
    private static UdpClient udpListener;

    public static bool isServer;
    public static void Start(int _maxPlayers, int _port)
    {
        MaxPlayers = _maxPlayers;
        Port = _port;

        Debug.Log("Server Stating....");
        InitializeServerData();

        tcpListener = new TcpListener(IPAddress.Any, Port);
        tcpListener.Start();
        tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectCallback), null);

        udpListener = new UdpClient(Port);
        udpListener.BeginReceive(UDPReceiveCallback, null);

        Debug.Log($"Server Opend, Port : {Port}");
        UIManager.Instance.IpText.text = GetMyIpAddress(isServer) + ":" + Port.ToString();
    }
    public static string GetMyIpAddress(bool isServer)
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        if (!isServer)
        {
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }

            throw new Exception("error");
        }
        else
        {
            var request = (HttpWebRequest)WebRequest.Create("http://ifconfig.me");
            request.UserAgent = "curl"; // this will tell the server to return the information as if the request was made by the linux "curl" command
            string publicIPAddress;
            request.Method = "GET";
            using (WebResponse response = request.GetResponse())
            {
                using (var reader = new System.IO.StreamReader(response.GetResponseStream()))
                {
                    publicIPAddress = reader.ReadToEnd();
                }
            }
            return publicIPAddress.Replace("\n", "");
        }
    }

    private static void TCPConnectCallback(IAsyncResult _result)
    {
        TcpClient _client = tcpListener.EndAcceptTcpClient(_result);
        tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectCallback), null);
        Debug.Log($"����� {_client.Client.RemoteEndPoint}...");

        for (int i = 1; i <= MaxPlayers; i++)
        {
            if (clients[i].tcp.socket == null)
            {
                clients[i].tcp.Connect(_client);
                return;
            }
        }
        Debug.Log($"{_client.Client.RemoteEndPoint} Has Faild TO Connect Server : Max Player");
    }

    private static void UDPReceiveCallback(IAsyncResult _result)
    {
        try
        {
            IPEndPoint _clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
            byte[] _data = udpListener.EndReceive(_result, ref _clientEndPoint);
            udpListener.BeginReceive(UDPReceiveCallback, null);

            if (_data.Length < 4)
            {
                return;
            }
            using (Packet _packet = new Packet(_data))
            {
                int _clientId = _packet.ReadInt();

                if (_clientId == 0)
                {
                    return;
                }
                if (clients[_clientId].udp.endPoint == null)
                {
                    clients[_clientId].udp.Connect(_clientEndPoint);
                    return;
                }
                if (clients[_clientId].udp.endPoint.ToString() == _clientEndPoint.ToString())
                {
                    clients[_clientId].udp.HandleData(_packet);
                }
            }
        }
        catch (Exception _ex)
        {
            Debug.Log($"Errored TO Sending UDP : {_ex}");
        }
    }

    public static void SendUDPData(IPEndPoint _clientEndPoint, Packet _packet)
    {
        try
        {
            if (_clientEndPoint != null)
            {
                udpListener.BeginSend(_packet.ToArray(), _packet.Length(), _clientEndPoint, null, null);
            }
        }
        catch (Exception _ex)
        {
            Debug.Log($"Erroed To Sending Data To {_clientEndPoint} VIA UDP : {_ex}");
        }
    }

    private static void InitializeServerData()
    {
        for (int i = 1; i <= MaxPlayers; i++)
        {
            clients.Add(i, new Client(i));
        }
        packetHandlers = new Dictionary<int, PacketHandler>()
            {
                { (int)ClientPackets.welcomeReceived, ServerHandle.WelcomeReceived },
                { (int)ClientPackets.lobby_Start, ServerHandle. },
                { (int)ClientPackets.lobby_Ready, ServerHandle. },
                { (int)ClientPackets.playerMovement, ServerHandle.PlayerMovement },
                { (int)ClientPackets.requestSpawnAgain, ServerHandle.PlayerSpawnAgain },
                { (int)ClientPackets.submarineMovement, ServerHandle.SubmarineMovement },
                { (int)ClientPackets.handlePlayerMove, ServerHandle.HandlePlayerMove },
                { (int)ClientPackets.textSend, ServerHandle.SendedText },
                { (int)ClientPackets.audioSend, ServerHandle.SendedAudio },
                { (int)ClientPackets.controll, ServerHandle.Controll },
                { (int)ClientPackets.sync, ServerHandle.Sync },
            };
        Debug.Log("Packet Initialize");
    }

    public static void Stop()
    {
        tcpListener.Stop();
        udpListener.Close();
    }
}

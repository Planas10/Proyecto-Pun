using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.IO;
using System;

public class Network_Manager : MonoBehaviour
{
    public static Network_Manager _NETWORK_MANAGER;
    public Room_UI_Manager room_manager;
    public void Awake()
    {
        _NETWORK_MANAGER = this;
    }

    private TcpClient socket;
    private NetworkStream stream;
    private StreamWriter writer;
    private StreamReader reader;
    const string host = "192.168.0.14";
    const int port = 6543;
    bool connected;
    public string playerName;
    public Stats selectedRaza;

    private void Update()
    {
        if (connected) 
        {
            if (stream.DataAvailable)
            {
                string data = reader.ReadLine();
                if (data != null)
                {
                    ManageData(data);
                }
            }
        }
    }
    public void ConnectToServer(string nick, string password)
    {
        try
        {
            socket = new TcpClient(host, port);
            stream = socket.GetStream();
            connected = true;
            writer = new StreamWriter(stream);
            reader = new StreamReader(stream);
            writer.WriteLine("0" + "/" + nick + "/" + password);
            writer.Flush();

        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }
    public void RegisterUser(string nick, string password,string race) 
    {
        try
        {
            socket = new TcpClient(host, port);
            stream = socket.GetStream();
            connected = true;
            writer = new StreamWriter(stream);
            reader = new StreamReader(stream);
            writer.WriteLine("2" + "/" + nick + "/" + password + "/" + race);
            writer.Flush();

        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }
    private void ManageData(string data)
    {
        List<string> parameters = new List<string>();
        parameters.AddRange(data.Split('/'));
        if (parameters[0] == "1")
        {
            Debug.Log("Recibo ping");
            writer.WriteLine("1");
            writer.Flush();
        }
        if (parameters[0] == "2") 
        {
            room_manager.SetMessageText(parameters[1]);
        }
        if (parameters[0] == "3")
        {
            room_manager.SetMessageText(parameters[1]);
            playerName = parameters[1];
            Photon_Manager._PHOTON_MANAGER.LoadMatchmaking();

        }
        if (parameters[0] == "4")
        {
            selectedRaza.maxHp = int.Parse(parameters[2]);
            selectedRaza.speed = int.Parse(parameters[3]);
            selectedRaza.jumpForce = int.Parse(parameters[4]);
            selectedRaza.damage = int.Parse(parameters[5]);
            selectedRaza.bulletSize = int.Parse(parameters[6]);

            Photon_Manager._PHOTON_MANAGER.LoadMatchmaking();

        }
    }


}

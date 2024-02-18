using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using UnityEngine.UI;
using System;
using System.Text;
using TMPro;
using System.Threading;

public class Client : MonoBehaviour
{
    // 连接按钮
    public Button connectButton;
    // 显示服务器信息
    public TextMeshProUGUI messageText;
    // 定义套接字
    private Socket clientSocket;
    // 接收缓冲区
    private byte[] buffer = new byte[1024];

    public string receivedMessage;
    // Start is called before the first frame update
    void Start()
    {
        // 添加 connectButton 的点击事件监听器
        connectButton.onClick.AddListener(ConnectToServer);
    }
    void ConnectToServer()
    {
        try
        {
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clientSocket.Connect("8.134.173.32", 3389); // 连接到服务器
            Debug.Log("Connected to server.");
            messageText.text = "Connect Success";
            // 开始接收数据
            Thread t = new Thread(ReceiveCallback);
            t.Start();
        }
        catch (Exception e)
        {
            Debug.Log("Error connecting to server: " + e.Message);
            messageText.text = "Connect Error";
        }
    }
    // 接受数据
    void ReceiveCallback()
    {
        while (true)
        {
            int bytesRead = clientSocket.Receive(buffer);
            if (bytesRead > 0)
            {
                receivedMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Debug.Log("Received message: " + receivedMessage);

                buffer = new byte[1024]; // 重置缓冲区
            }
            else
            {
                Debug.Log("Server disconnected.");
                break;
            }
        }
    }
    // 向服务器发送消息
    public void Send(string message)
    {
        byte[] data = Encoding.UTF8.GetBytes(message);
        clientSocket.Send(data);
    }
    void SendInstruct()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) || (Input.GetKeyDown(KeyCode.A)))
        {
            Send("Turn Left");
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || (Input.GetKeyDown(KeyCode.D)))
        {
            Send("Turn Right");
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) || (Input.GetKeyDown(KeyCode.W)))
        {
            Send("Move Forward");
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || (Input.GetKeyDown(KeyCode.S)))
        {
            Send("Move Backward");
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            Send("Brake");
        }
        else if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Send("Accelerate");
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            Send("Decelerate");
        }
    }
    // Update is called once per frame
    void Update()
    {
        SendInstruct();// 向服务器发送指令
    }
    void OnApplicationQuit()
    {
        clientSocket.Close();
    }
}

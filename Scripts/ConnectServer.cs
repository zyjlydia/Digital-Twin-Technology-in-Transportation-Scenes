using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using UnityEngine.UI;
using System;
using System.Text;
using TMPro;

public class ConnectServer : MonoBehaviour
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
    // 点击连接按钮
    public void ConnectToServer()
    {
        try
        {
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clientSocket.Connect("127.0.0.1", 8888); // 连接到服务器
            Debug.Log("Connected to server.");
            messageText.text = "Connect Success";
            StartListening();
        }
        catch (Exception e)
        {
            Debug.Log("Error connecting to server: " + e.Message);
            messageText.text = "Connect Error";
        }
    }

    // 接受数据
    void StartListening()
    {
        clientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceiveCallback, null);
    }

    // 接收数据的回调函数
    void ReceiveCallback(IAsyncResult ar)
    {
        int bytesRead = clientSocket.EndReceive(ar);
        if (bytesRead > 0)
        {
            receivedMessage = Encoding.ASCII.GetString(buffer, 0, bytesRead);
            Debug.Log("Received message: " + receivedMessage);

            buffer = new byte[1024]; // 重置缓冲区
            StartListening(); // 开始下一次接收
        }
        else
        {
            Debug.Log("Server disconnected.");
        }
    }
    // 向服务器发送消息
    public void Send(string message)
    {
        byte[] data = Encoding.ASCII.GetBytes(message);
        clientSocket.Send(data);
    }
    // 向服务器发送指令
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

    // 关闭客户端 socket
    void OnApplicationQuit()
    {
        clientSocket.Close();
    }
}

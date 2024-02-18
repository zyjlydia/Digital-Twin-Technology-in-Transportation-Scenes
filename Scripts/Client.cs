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
    // ���Ӱ�ť
    public Button connectButton;
    // ��ʾ��������Ϣ
    public TextMeshProUGUI messageText;
    // �����׽���
    private Socket clientSocket;
    // ���ջ�����
    private byte[] buffer = new byte[1024];

    public string receivedMessage;
    // Start is called before the first frame update
    void Start()
    {
        // ��� connectButton �ĵ���¼�������
        connectButton.onClick.AddListener(ConnectToServer);
    }
    void ConnectToServer()
    {
        try
        {
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clientSocket.Connect("8.134.173.32", 3389); // ���ӵ�������
            Debug.Log("Connected to server.");
            messageText.text = "Connect Success";
            // ��ʼ��������
            Thread t = new Thread(ReceiveCallback);
            t.Start();
        }
        catch (Exception e)
        {
            Debug.Log("Error connecting to server: " + e.Message);
            messageText.text = "Connect Error";
        }
    }
    // ��������
    void ReceiveCallback()
    {
        while (true)
        {
            int bytesRead = clientSocket.Receive(buffer);
            if (bytesRead > 0)
            {
                receivedMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Debug.Log("Received message: " + receivedMessage);

                buffer = new byte[1024]; // ���û�����
            }
            else
            {
                Debug.Log("Server disconnected.");
                break;
            }
        }
    }
    // �������������Ϣ
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
        SendInstruct();// �����������ָ��
    }
    void OnApplicationQuit()
    {
        clientSocket.Close();
    }
}

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
    // ������Ӱ�ť
    public void ConnectToServer()
    {
        try
        {
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clientSocket.Connect("127.0.0.1", 8888); // ���ӵ�������
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

    // ��������
    void StartListening()
    {
        clientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceiveCallback, null);
    }

    // �������ݵĻص�����
    void ReceiveCallback(IAsyncResult ar)
    {
        int bytesRead = clientSocket.EndReceive(ar);
        if (bytesRead > 0)
        {
            receivedMessage = Encoding.ASCII.GetString(buffer, 0, bytesRead);
            Debug.Log("Received message: " + receivedMessage);

            buffer = new byte[1024]; // ���û�����
            StartListening(); // ��ʼ��һ�ν���
        }
        else
        {
            Debug.Log("Server disconnected.");
        }
    }
    // �������������Ϣ
    public void Send(string message)
    {
        byte[] data = Encoding.ASCII.GetBytes(message);
        clientSocket.Send(data);
    }
    // �����������ָ��
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

    // �رտͻ��� socket
    void OnApplicationQuit()
    {
        clientSocket.Close();
    }
}

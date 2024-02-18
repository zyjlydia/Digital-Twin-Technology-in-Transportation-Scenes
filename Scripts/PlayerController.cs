using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";

    private float horizontalInput;
    private float verticalInput;
    private float currentSteerAngle;
    private float currentbreakForce;
    public bool isBraking = false;
    public bool isAccelerating = false;
    public bool isDecelerating = false;

    public float motorForce = 20f;//最大马达值
    public float brakeForce = 100f; //刹车力量
    public float maxSteerAngle = 45f; //车轮转向角度

    public Vector3 locaRv3;
    public float speed = 0;
    public float speedOrigin = 0;

    [SerializeField] private WheelCollider frontLeftWheelCollider;
    [SerializeField] private WheelCollider frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider;
    [SerializeField] private WheelCollider rearRightWheelCollider;


    [SerializeField] private Transform frontLeftWheelTransform;
    [SerializeField] private Transform frontRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform;
    [SerializeField] private Transform rearRightWheelTransform;

    public GameObject connectButton;
    private string message;//从服务端接受的消息
    // Start is called before the first frame update
    void Start()
    {
        message = connectButton.GetComponent<Client>().receivedMessage;
    }

    void Update()
    {
        message = connectButton.GetComponent<Client>().receivedMessage;
    }

    private void FixedUpdate()
    {
        GetInput();
        Move();
        Steer();
        if (message != null && message != "0")
        {
            ServerUpdate();
        }
        CalculateSpeed();
        UpdateWheels();   
    }

    void GetInput()
    {
        //方向控制
        horizontalInput = Input.GetAxis(HORIZONTAL);
        verticalInput = Input.GetAxis(VERTICAL);
        //空格刹车
        if (Input.GetKeyDown(KeyCode.Space))
            isBraking = !isBraking;
        //左shift加速
        if (Input.GetKeyDown(KeyCode.LeftShift))
            isAccelerating = !isAccelerating;
        //X键减速
        if (Input.GetKeyDown(KeyCode.X))
            isDecelerating = !isDecelerating;
    }

    //动力（车轮碰撞器）
    void Move()
    {
        frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
        frontRightWheelCollider.motorTorque = verticalInput * motorForce;

        if (isAccelerating)
        {
            frontLeftWheelCollider.motorTorque = 200.0f;
            frontRightWheelCollider.motorTorque = 200.0f;
        }

        if (isDecelerating)//减速
        {
            frontLeftWheelCollider.motorTorque = -200.0f;
            frontRightWheelCollider.motorTorque = -200.0f;
        }


        if (isBraking)
            currentbreakForce = brakeForce;
        else
            currentbreakForce = 0f;
        ApplyBrake();
    }
    //刹车
    void ApplyBrake()
    {
        frontRightWheelCollider.brakeTorque = currentbreakForce;
        frontLeftWheelCollider.brakeTorque = currentbreakForce;
        rearRightWheelCollider.brakeTorque = currentbreakForce;
        rearRightWheelCollider.brakeTorque = currentbreakForce;
    }
    //计算速度
    void CalculateSpeed()
    {
        //汽车速度，km/h
        speed = rearLeftWheelCollider.rpm * (rearLeftWheelCollider.radius * 2 * Mathf.PI) * 60 / 1000;
        //车速-去掉小数点
        speed = Mathf.Round(speed);
        speedOrigin = speed;
        //绝对值
        speed = Mathf.Abs(speed);
    }
    //转向（车轮碰撞器）
    void Steer()
    {
        currentSteerAngle = maxSteerAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    void SteerLeft()
    {
        currentSteerAngle = -maxSteerAngle;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    void SteerRight()
    {
        currentSteerAngle = maxSteerAngle;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheelTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
    }

    void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }

    void ServerUpdate()
    {
        if (message.ToLower() == "MoveForward".ToLower() || message == "1")
        {
            frontLeftWheelCollider.motorTorque = 20.0f;
            frontRightWheelCollider.motorTorque = 20.0f;
        }
        else if(message.ToLower() == "MoveBackward".ToLower() || message == "2")
        {
            frontLeftWheelCollider.motorTorque = -20.0f;
            frontRightWheelCollider.motorTorque = -20.0f;
        }
        else if(message.ToLower() == "accelerate" || message == "3")
        {
            frontLeftWheelCollider.motorTorque = 200.0f;
            frontRightWheelCollider.motorTorque = 200.0f;
        }
        else if(message.ToLower() == "decelerate" || message == "4")
        {
            frontLeftWheelCollider.motorTorque = -200.0f;
            frontRightWheelCollider.motorTorque = -200.0f;
        }
        else if (message.ToLower() == "brake" || message == "5")
        {
            frontRightWheelCollider.brakeTorque = brakeForce;
            frontLeftWheelCollider.brakeTorque = brakeForce;
            rearRightWheelCollider.brakeTorque = brakeForce;
            rearRightWheelCollider.brakeTorque = brakeForce;
        }
        else if (message.ToLower() == "TurnLeft".ToLower() || message == "6")
        {
            SteerLeft();
        }
        else if(message.ToLower() == "TurnRight".ToLower() || message == "7")
        {
            SteerRight();
        }
    }
}

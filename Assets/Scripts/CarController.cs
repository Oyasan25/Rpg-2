using NUnit.Framework;
using UnityEngine;
using System;
using System.Collections.Generic;
using Unity.Mathematics;

public class CarController : MonoBehaviour
{
    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";

    private float HorizontalInput;
    private float VerticalInput;
    private float currentSteerAngle;
    private float currentbreakForce;
    private bool isBreaking;

    [SerializeField] private float motorForce;
    [SerializeField] private float breakForce;
    [SerializeField] private float maxSteerAngle;

    [SerializeField] private WheelCollider frontLeftWheelCollider;
    [SerializeField] private WheelCollider frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider;
    [SerializeField] private WheelCollider rearRightWheelCollider;


    [SerializeField] private Transform frontLeftWheelTransform;
    [SerializeField] private Transform frontRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform;
    [SerializeField] private Transform rearRightWheelTransform;

    private void FixedUpdate()
    {
        GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();
    }


    private void GetInput()
    {
        HorizontalInput = Input.GetAxis(HORIZONTAL);
        VerticalInput = Input.GetAxis(VERTICAL);
        isBreaking = Input.GetKey(KeyCode.Space);

        
    }

    private void HandleMotor()
    {
        frontLeftWheelCollider.motorTorque = VerticalInput * motorForce;
        frontRightWheelCollider.motorTorque = VerticalInput * motorForce;
        currentbreakForce = isBreaking ? breakForce : 0f;
        if (isBreaking)
        {
            ApplyBreaking();
        }
    }

    private void ApplyBreaking()
    {
        frontRightWheelCollider.brakeTorque = currentbreakForce;
        frontLeftWheelCollider.brakeTorque = currentbreakForce;
        rearRightWheelCollider.brakeTorque = currentbreakForce;
        rearLeftWheelCollider.brakeTorque = currentbreakForce;
    }

    private void HandleSteering()
    {
        currentSteerAngle = maxSteerAngle * HorizontalInput;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;

        currentSteerAngle = maxSteerAngle * HorizontalInput;
        frontRightWheelCollider.steerAngle = currentSteerAngle;

    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheelTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
    }

    private void UpdateSingleWheel(WheelCollider WheelCollider, Transform WheelTransform)
    {
        Vector3 pos;
        Quaternion rot;

        WheelCollider.GetWorldPose(out pos, out rot);
        WheelTransform.rotation = rot;
        WheelTransform.position = pos;
    }


    private void AdjustWheelFriction(WheelCollider wheel, float forwardStiffness, float sidewaysStiffness)
    {
        // Configure forward friction
        WheelFrictionCurve forwardFriction = wheel.forwardFriction;
        forwardFriction.stiffness = forwardStiffness; // Increase this value to improve grip
        wheel.forwardFriction = forwardFriction;

        // Configure sideways friction
        WheelFrictionCurve sidewaysFriction = wheel.sidewaysFriction;
        sidewaysFriction.stiffness = sidewaysStiffness;
        wheel.sidewaysFriction = sidewaysFriction;
    }

    private void Start()
    {
        // Example adjustment to all wheels
        AdjustWheelFriction(frontLeftWheelCollider, 2.0f, 1.5f);
        AdjustWheelFriction(frontRightWheelCollider, 2.0f,1.5f);
        AdjustWheelFriction(rearLeftWheelCollider, 2.0f, 1.5f);
        AdjustWheelFriction(rearRightWheelCollider, 2.0f, 1.5f);


        Rigidbody rb = GetComponent<Rigidbody>();
        rb.mass = 500;
        rb.linearDamping = 0.1f;
        rb.angularDamping = 0.05f;
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";

    private float horizontalInput;
    private float verticalInput;
    private float currentSteerAngle;
    private float currentbreakForce;
    private bool isBreaking;

    [SerializeField] private float motorForce;
    [SerializeField] private float breakForce;
    [SerializeField] private float maxSteerAngle;

    [SerializeField] private WheelCollider wheelColliderLeftFront;
    [SerializeField] private WheelCollider wheelColliderRightFront;
    [SerializeField] private WheelCollider wheelColliderLeftBack;
    [SerializeField] private WheelCollider wheelColliderRightBack;

    [SerializeField] private Transform wheelLeftFront;
    [SerializeField] private Transform wheelRightFront;
    [SerializeField] private Transform wheelLeftBack;
    [SerializeField] private Transform wheelRightBack;

    private void FixedUpdate()
    {
        GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();
    }


    private void GetInput()
    {
        horizontalInput = Input.GetAxis(HORIZONTAL);
        verticalInput = Input.GetAxis(VERTICAL);
        isBreaking = Input.GetKey(KeyCode.LeftShift);
    }

    private void HandleMotor()
    {
        wheelColliderLeftFront.motorTorque = verticalInput * motorForce;
        wheelColliderRightFront.motorTorque = verticalInput * motorForce;
        currentbreakForce = isBreaking ? breakForce : 0f;
        ApplyBreaking();
    }

    private void ApplyBreaking()
    {
        wheelColliderRightFront.brakeTorque = currentbreakForce;
        wheelColliderLeftFront.brakeTorque = currentbreakForce;
        wheelColliderLeftBack.brakeTorque = currentbreakForce;
        wheelColliderRightBack.brakeTorque = currentbreakForce;
    }

    private void HandleSteering()
    {
        currentSteerAngle = maxSteerAngle * horizontalInput;
        wheelColliderLeftFront.steerAngle = currentSteerAngle;
        wheelColliderRightFront.steerAngle = currentSteerAngle;
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(wheelColliderLeftFront, wheelLeftFront);
        UpdateSingleWheel(wheelColliderRightFront, wheelRightFront);
        UpdateSingleWheel(wheelColliderRightBack, wheelRightBack);
        UpdateSingleWheel(wheelColliderLeftBack, wheelLeftBack);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot; 
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }
}
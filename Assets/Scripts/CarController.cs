using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public Transform centerOfMass;
    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";

    private float horizontalInput;
    private float verticalInput;
    private float currentSteerAngle;
    private float currentbreakForce;
    private bool isBreaking;
    private WheelFrictionCurve friction;

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
    private Rigidbody _rigidbody;
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.centerOfMass = centerOfMass.localPosition;
    }
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
        wheelColliderLeftBack.motorTorque = verticalInput * motorForce;
        wheelColliderRightBack.motorTorque = verticalInput * motorForce;
        currentbreakForce = isBreaking ? breakForce : 0f;
        ApplyBreaking();
    }

    private void ApplyBreaking()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        { 
        friction = wheelColliderRightBack.sidewaysFriction;
        friction.extremumSlip = 0.7f;
        wheelColliderRightBack.sidewaysFriction = friction;

        friction = wheelColliderLeftBack.sidewaysFriction;
        friction.extremumSlip = 0.7f;
        wheelColliderLeftBack.sidewaysFriction = friction;

        }
        else
        {
            friction = wheelColliderRightBack.sidewaysFriction;
            friction.extremumSlip = 0.05f;
            wheelColliderRightBack.sidewaysFriction = friction;

            friction = wheelColliderLeftBack.sidewaysFriction;
            friction.extremumSlip = 0.05f;
            wheelColliderLeftBack.sidewaysFriction = friction;
        }
        wheelColliderRightFront.brakeTorque = currentbreakForce/4f;
        wheelColliderLeftFront.brakeTorque = currentbreakForce/4f;
        wheelColliderLeftBack.brakeTorque = currentbreakForce/4f;
        wheelColliderRightBack.brakeTorque = currentbreakForce/4f;
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
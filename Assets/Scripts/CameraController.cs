using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour

{
    private Vector3 _cameraOffset;
    [SerializeField] private Transform target;
    [SerializeField] private float interpolationTime;
    private Vector3 _currentVelocity = Vector3.zero;

    [SerializeField] private float transitionSpeed = 2.0f;
    [SerializeField] private float rotationSpeed = 2.0f;

    private Transform _paintingTarget;
    private bool _isPaintingMode = false;
    
    void Start()
    {
        transform.position = target.position + new Vector3(0.0f, 20f, -30.0f);
        _cameraOffset = transform.position - target.position;
    }

    private void FixedUpdate()
    {
        if (!_isPaintingMode)
        {
            Vector3 targetPosition = target.position + _cameraOffset;
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _currentVelocity, interpolationTime);
        }
        else
        {
            if (_paintingTarget != null)
            {
                transform.position = Vector3.Lerp(transform.position, _paintingTarget.position, Time.deltaTime * transitionSpeed);
                transform.rotation = Quaternion.Slerp(transform.rotation, _paintingTarget.rotation, Time.deltaTime * rotationSpeed);
            }
        }
    }

    public void SwitchToPaintingMode(Transform targetPos)
    {
        _paintingTarget = targetPos;
        _isPaintingMode = true;
    }
    
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwerveController : MonoBehaviour
{
    [SerializeField] private float swerveSpeed = 0.2f;
    [SerializeField] private float maxX = 2.5f; 
    [SerializeField] private float maxY = 1.5f; 

    private Vector2 _lastFrameFingerPosition;
    private Vector2 _moveFactor;
    
    private float _centerX;
    private float _centerY;
    private float _fixedZ;
    
    private bool _isInputCalibrated = false;

    private void OnEnable()
    {
        _centerX = transform.position.x;
        _centerY = transform.position.y;
        _fixedZ = transform.position.z;
        
        _moveFactor = Vector2.zero;
        _isInputCalibrated = false;
        
        if (Input.GetMouseButton(0))
        {
            _lastFrameFingerPosition = Input.mousePosition;
            _isInputCalibrated = true;
        }
    }

    private void Update()
    {
        if (GameManager.Instance.State != GameState.Painting) return;
        
        if (Input.GetMouseButtonDown(0))
        {
            _lastFrameFingerPosition = Input.mousePosition;
            _isInputCalibrated = true; 
        }
        else if (Input.GetMouseButton(0))
        {
            if (!_isInputCalibrated)
            {
                _lastFrameFingerPosition = Input.mousePosition;
                _isInputCalibrated = true;
                return;
            }

            _moveFactor = (Vector2)Input.mousePosition - _lastFrameFingerPosition;
            _lastFrameFingerPosition = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _moveFactor = Vector2.zero;
            _isInputCalibrated = false;
        }
        
        if (_moveFactor != Vector2.zero && _isInputCalibrated)
        {
            float moveX = _moveFactor.x * swerveSpeed * Time.deltaTime;
            float moveY = _moveFactor.y * swerveSpeed * Time.deltaTime;

            Vector3 currentPos = transform.position;
            
            currentPos.x += moveX;
            currentPos.y += moveY;
            
            currentPos.x = Mathf.Clamp(currentPos.x, _centerX - maxX, _centerX + maxX);
            currentPos.y = Mathf.Clamp(currentPos.y, _centerY - maxY, _centerY + maxY);
            
            currentPos.z = _fixedZ;

            transform.position = currentPos;
        }
    }
}
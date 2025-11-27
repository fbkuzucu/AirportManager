using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 15f;
    [SerializeField] private DynamicJoystick joystick;
    
    private Rigidbody _rb;
    private PlayerAnimationManager _animManager;
    
    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _animManager = GetComponent<PlayerAnimationManager>();
        
        if (_animManager == null) _animManager = gameObject.AddComponent<PlayerAnimationManager>();
    }

    private void FixedUpdate()
    {
        MoveCharacter();
    }

    private void MoveCharacter()
    {
        _rb.velocity = new Vector3(joystick.Horizontal * moveSpeed, _rb.velocity.y, joystick.Vertical * moveSpeed);
        
        if (joystick.Horizontal != 0 || joystick.Vertical != 0)
        {
            Quaternion targetRotation = Quaternion.LookRotation(_rb.velocity);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 20f);
            _animManager.SetRunningState(true);
        }
        else
        {
            _rb.angularVelocity = Vector3.zero;
            _animManager.SetRunningState(false);
        }

       
    }
    
}

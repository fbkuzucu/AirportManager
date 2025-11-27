using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{
    private Animator _animator;

    private int _isRunningID;
    private int _workID;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        
        _isRunningID = Animator.StringToHash("IsRunning");
        _workID = Animator.StringToHash("WorkID");
    }

    public void UpdateMovementAnimation(bool isMoving)
    {
        if (_animator == null) return;
        
        
    }

    public void SetRunningState(bool isMoving)
    {
        if (!_animator) return;
        
        if (_animator.GetBool(_isRunningID) != isMoving) _animator.SetBool(_isRunningID, isMoving);
    }

    public void SetWorkID(int id)
    {
        if(!_animator) return;
        
        if(_animator.GetInteger(_workID) != id) _animator.SetInteger(_workID, id);
    }
    
}

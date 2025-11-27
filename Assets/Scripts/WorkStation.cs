using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class WorkStation : MonoBehaviour
{
    
    [SerializeField] private float processTime = 1.0f;
    [SerializeField] private int playerAnimationWorkID = 1;
    [SerializeField] private string customerAnimationTrigger;

    [SerializeField] private bool requiresPlayer = true;
    [SerializeField] private bool requiresCustomer = true;

    [SerializeField] private bool useJobQueue = false;
    [SerializeField] private bool autoRepeat = false;
    
    public UnityEvent OnProcessStart;
    public UnityEvent OnProcessComplete;
    
    private bool _isPlayerOnArea = false;
    private CustomerAI _currentCustomer = null;
    private bool _isProcessing = false;
    private PlayerAnimationManager _playerAnimManager;

    private int _pendingJobs = 0;

    private void Start()
    {
        if (useJobQueue && GameManager.Instance != null)
        {
            _pendingJobs = GameManager.Instance.GetTotalCustomer();
        }
    }

    private void OnTriggerEnter(Collider trigger)
    {
        if (trigger.CompareTag("Player"))
        {
            _isPlayerOnArea = true;
            _playerAnimManager = trigger.GetComponent<PlayerAnimationManager>();
            
            if(_playerAnimManager != null) _playerAnimManager.SetWorkID(playerAnimationWorkID);

            CheckAndStartProcess();
        }
    }

    private void OnTriggerExit(Collider trigger)
    {
        if (trigger.CompareTag("Player"))
        {
            _isPlayerOnArea = false;
            
            if (_playerAnimManager != null) _playerAnimManager.SetWorkID(0);
            
            _playerAnimManager = null;
        }
    }

    public void RegisterCustomer(CustomerAI customer)
    {
        _currentCustomer = customer;
        CheckAndStartProcess();
    }

    private void CheckAndStartProcess()
    {
       if(_isProcessing) return;
       if (requiresPlayer && !_isPlayerOnArea) return;

       if (useJobQueue)
       {
           if (_pendingJobs > 0)
           {
               StartCoroutine(ProcessRoutine());
           }
           else
           {
               Debug.Log("Done!");
           }
       }
       else
       {
           if (requiresCustomer &&  _currentCustomer == null) return;
           StartCoroutine(ProcessRoutine());
       }
    }

    private IEnumerator ProcessRoutine()
    {
        _isProcessing = true;

        OnProcessStart.Invoke();
        
        if (_currentCustomer != null && !string.IsNullOrEmpty(customerAnimationTrigger))
        {
            _currentCustomer.GetComponent<Animator>().SetTrigger(customerAnimationTrigger);
        }
        
        yield return new WaitForSeconds(processTime);

        CompleteProcess();
    }
    
    protected virtual void CompleteProcess()
    {
        OnProcessComplete.Invoke();

        if (GameManager.Instance != null)
        {
            GameManager.Instance.CompleteTask();
        }
        
        if (_currentCustomer != null)
        {
            _currentCustomer.ReleaseFromDesk();
            _currentCustomer = null;
        }

        if (useJobQueue)
        {
            _pendingJobs--;
        }
        
        _isProcessing = false;

        bool shouldRepeat = false;

        if (autoRepeat && _isPlayerOnArea)
        {
            if (useJobQueue && _pendingJobs > 0) shouldRepeat = true;
            else if (!useJobQueue && _currentCustomer != null) shouldRepeat = true;
        }

        if (shouldRepeat)
        {
            StartCoroutine(RestartProcess());
        }
    }

    private IEnumerator RestartProcess()
    {
        yield return new WaitForSeconds(0.2f);
        CheckAndStartProcess();
    }
    
}

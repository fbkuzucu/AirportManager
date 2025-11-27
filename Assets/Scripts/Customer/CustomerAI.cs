using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
public class CustomerAI : MonoBehaviour
{
    private NavMeshAgent _agent;
    private Animator _animator;
    
    private int _currentWaypointIndex = 0;
    private bool _isBusy = false;
    
    [SerializeField] public CustomerWaypointData currentWaypointData;
    [SerializeField] private float customerSpace = 1.0f;
    

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }

    void Start()
    {
        _agent.stoppingDistance = 0.1f;
        _agent.autoBraking = true;
        _agent.avoidancePriority = Random.Range(30, 70);
        
        if (currentWaypointData != null && currentWaypointData.customerWaypoints.Count > 0) 
        {
            MoveToWaypoint();
        }
    }

    void Update()
    {
        if (_isBusy) return;
        Vector3 radar = transform.position + Vector3.up * 1.0f;
        Ray ray = new Ray(radar, transform.forward);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, customerSpace))
        {
            if (hit.collider.gameObject.GetComponent<CustomerAI>() != null)
            {
                _agent.isStopped = true;
                _agent.velocity = Vector3.zero;
                _animator.SetBool("IsMoving", false);
                return;
            }
        }
        else
        {
            _agent.isStopped = false;
        }
        
        bool isMoving = _agent.velocity.sqrMagnitude > 0.1f;
        _animator.SetBool("IsMoving", isMoving);

        if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
        {
            if (!_agent.hasPath || _agent.velocity.sqrMagnitude == 0.0f)
            {
                StartCoroutine(WaypointArrival());
            }
        }
    }

    private IEnumerator WaypointArrival()
    {
        if (_isBusy) yield break;
        
        if (_currentWaypointIndex >= currentWaypointData.customerWaypoints.Count) yield break;
        
        _isBusy = true;
        _agent.isStopped = true;
        _agent.velocity = Vector3.zero;
        _animator.SetBool("IsMoving", false);
        
        WaypointStep currentStep = currentWaypointData.customerWaypoints[_currentWaypointIndex];
        
        if (!string.IsNullOrEmpty(currentStep.animationTrigger))
        {
            _animator.SetTrigger(currentStep.animationTrigger);

            Collider[] hits = Physics.OverlapSphere(transform.position, 3.0f);
            foreach (var hit in hits)
            {
                WorkStation station = hit.GetComponent<WorkStation>();
                if (station != null)
                {
                    station.RegisterCustomer(this);
                    yield break;
                }
            }
        }
        
        if (currentStep.waitTime > 0)
        {
            yield return new WaitForSeconds(currentStep.waitTime);
        }
        else if (!string.IsNullOrEmpty(currentStep.animationTrigger))
        {
            yield return new WaitForSeconds(1.5f); 
        }

        if (currentStep.requiredStateToLeave != GameState.Idle)
        {
           yield return new WaitUntil(() => GameManager.Instance.State == currentStep.requiredStateToLeave);
        }
       
        _isBusy = false;
        _agent.isStopped = false;
        GoToNextDestination();
    }

    private void GoToNextDestination()
    {
        _currentWaypointIndex++;

        if (_currentWaypointIndex >= currentWaypointData.customerWaypoints.Count)
        {
            if(GameManager.Instance != null) GameManager.Instance.SetMoney(50);
            Destroy(gameObject);
            return;
        }

        MoveToWaypoint();
    }

    private void MoveToWaypoint()
    {
        Vector3 targetPosition = currentWaypointData.customerWaypoints[_currentWaypointIndex].position;
        _agent.SetDestination(targetPosition);
    }

    public void ReleaseFromDesk()
    {
        _isBusy = false;
        if (_agent != null && _agent.isOnNavMesh)
        {
            _agent.isStopped = false;
        }
        GoToNextDestination();
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerTeleport : MonoBehaviour
{
    
    [SerializeField] private Transform destinationTeleport;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && destinationTeleport != null)
        {
            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
            rb.position = destinationTeleport.position;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}

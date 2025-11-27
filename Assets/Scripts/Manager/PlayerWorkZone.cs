using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWorkZone : MonoBehaviour
{

    [SerializeField] private int targetID = 1;

    private void OnTriggerEnter(Collider interactCollider)
    {
        if (interactCollider.gameObject.CompareTag("Player"))
        {
            var animationManager = interactCollider.GetComponent<PlayerAnimationManager>();
            if (animationManager != null)
            {
                animationManager.SetWorkID(targetID);
            }
        }
    }

    private void OnTriggerExit(Collider interactCollider)
    {
        if (interactCollider.gameObject.CompareTag("Player"))
        {
            var animationManager = interactCollider.GetComponent<PlayerAnimationManager>();
            if (animationManager != null)
            {
                animationManager.SetWorkID(0);
            }
        }
    }
    
    
    
   
}

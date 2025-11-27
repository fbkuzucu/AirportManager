using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintingPhaseManager : MonoBehaviour
{
    [SerializeField] private CameraController cameraController;
    [SerializeField] private Transform paintingCamPos;
    
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject wallBrushObject;
    
    
    void Start()
    {
        if(GameManager.Instance != null) GameManager.OnGameStateChanged += HandleStateChanged;
    }

    private void OnDestroy()
    {
        if(GameManager.Instance != null) GameManager.OnGameStateChanged -= HandleStateChanged;
    }

    private void HandleStateChanged(GameState currentState)
    {
        if (currentState == GameState.Painting)
        {
            StartPaintingPhase();
        }
    }

    private void StartPaintingPhase()
    {
       if (cameraController != null & paintingCamPos != null) cameraController.SwitchToPaintingMode(paintingCamPos);

       if (player != null)
       {
           var controller = player.GetComponent<PlayerController>();
           if (controller != null) controller.enabled = false;
           
           var rb = player.GetComponent<Rigidbody>();
           if (rb != null)
           {
               rb.velocity = Vector3.zero;
               rb.isKinematic = true;
           }
           
           var anim = player.GetComponent<Animator>();
           if (anim != null) anim.SetBool("IsRunning", false);
           
       }
       
       if (wallBrushObject != null) wallBrushObject.SetActive(true);
    }
}

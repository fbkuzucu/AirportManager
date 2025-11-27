using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UnlockEvent : MonoBehaviour
{

    [SerializeField] private int eventCost;
    [SerializeField] private GameObject newEventPlace;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private GameState nextGameState;
    
    private SpriteRenderer _spriteObject;

    private bool _isUnlocked = false;
    private int _remainingCost;
    
    //for pop effect
    private float _animationTimer;
    
    // Start is called before the first frame update
    void Start()
    {
        _remainingCost = eventCost;
        UpdateText();
        _spriteObject = GetComponent<SpriteRenderer>();
        
        if (newEventPlace != null) newEventPlace.SetActive(false);
    }

   
    private void OnTriggerStay(Collider other)
    {
        //just in case
        if (_isUnlocked) return;
        
        if (other.CompareTag("Player"))
        {
            CheckMoney();
        }
    }

    private void CheckMoney()
    {
        if (GameManager.Instance.GetMoney() >= eventCost)
        {
            Debug.Log("Money left");
            GameManager.Instance.SetMoney(-eventCost);
            _isUnlocked = true;
            Unlock();
        }
        else
        {
            _isUnlocked = false;
        }
    }

    private void Unlock()
    {
        if (_spriteObject != null && costText != null)
        {
            _spriteObject.enabled = false;
            costText.enabled = false;  
        }

        if (newEventPlace != null)
        {
            Debug.Log("Unlocked");
            newEventPlace.SetActive(true);
            StartCoroutine(PopUpAnimation(newEventPlace.transform));
        }
        
        GameManager.Instance.UpdateGameState(nextGameState);
    }
    
    private void UpdateText()
    {
       if(costText != null) costText.text = eventCost.ToString();
    }

   IEnumerator PopUpAnimation(Transform targetObj)
    {
        targetObj.gameObject.SetActive(true);
        targetObj.localScale = Vector3.zero;

        float timer = 0f;
        float duration = 0.5f;
        
        Vector3 targetScale = Vector3.one; 
        
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float progress = timer / duration;
            float curve = Mathf.SmoothStep(0, 1, progress);
            targetObj.localScale = Vector3.Lerp(Vector3.zero, targetScale, curve);
            yield return null;
        }
        targetObj.localScale = targetScale;
    }

}

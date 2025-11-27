using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Slider = UnityEngine.UI.Slider;

public class UIManager : MonoBehaviour
{

    [SerializeField] private GameObject gameplayUI;
    [SerializeField] private GameObject paintingUI;

    [SerializeField] private WallPainter wallPainter;
    [SerializeField] private Slider slider;
    
    [SerializeField] private Button yellowButton;
    [SerializeField] private Button redButton;
    [SerializeField] private Button blueButton;
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI stateText;
    
   
    
    void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.OnMoneyChanged += UpdateMoneyUI;
            GameManager.OnGameStateChanged += HandleStateChanged;
            
            UpdateMoneyUI(GameManager.Instance.GetMoney());
            HandleStateChanged(GameManager.Instance.State);
            
        }
        
        if (slider != null && wallPainter != null) slider.onValueChanged.AddListener(OnSliderChanged);
        
        SelectColorRed();
        
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.OnMoneyChanged -= UpdateMoneyUI;
            GameManager.OnGameStateChanged -= HandleStateChanged;
        }
    }

    private void UpdateMoneyUI(int moneyValue)
    {
        if(moneyText != null) moneyText.text = $"Money:{moneyValue}";
    }

    private void HandleStateChanged(GameState newState)
    {
        if (gameplayUI != null && paintingUI != null)
        {
            if (newState == GameState.Painting)
            {
                gameplayUI.SetActive(false);
                paintingUI.SetActive(true);
            }
            else
            {
                gameplayUI.SetActive(true);
                paintingUI.SetActive(false);
            }
        }
        
        stateText.text = $"State: {newState.ToString()}";
        
    }

    public void SelectColorRed()
    {
        wallPainter.SetBrushColor(Color.red);
        UpdateButtonScales(redButton);
    }

    public void SelectColorYellow()
    {
        wallPainter.SetBrushColor(Color.yellow);
        UpdateButtonScales(yellowButton);
    }
    public void SelectColorBlue()
    {
        wallPainter.SetBrushColor(Color.blue);
        UpdateButtonScales(blueButton);
    }
    
    private void UpdateButtonScales(Button selectedButton)
    {
        Vector3 bigScale = Vector3.one * 1.2f;
        Vector3 normalScale = Vector3.one;
        
        if (redButton != null)    redButton.transform.localScale    = (redButton == selectedButton)    ? bigScale : normalScale;
        if (yellowButton != null) yellowButton.transform.localScale = (yellowButton == selectedButton) ? bigScale : normalScale;
        if (blueButton != null)   blueButton.transform.localScale   = (blueButton == selectedButton)   ? bigScale : normalScale;
    }
    public void OnSliderChanged(float val)
    {
        wallPainter.SetBrushSize(val);
    }
}

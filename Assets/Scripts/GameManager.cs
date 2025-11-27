using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    public GameState State;
    
    [SerializeField] private int money = 100;
    [SerializeField] private int totalCustomer;

    private int _processedCount = 0;
    
    public static event Action<GameState> OnGameStateChanged;
    public static event Action<int> OnMoneyChanged;
    
    
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
    }

    private void Start()
    {
        Application.targetFrameRate = 60; 
        QualitySettings.vSyncCount = 0;
        
        UpdateGameState(GameState.Idle);
        OnMoneyChanged?.Invoke(money);
    }

    public void UpdateGameState(GameState newGameState)
    {
        State = newGameState;

        switch (newGameState)
        {
            case GameState.Idle:
                break;
            case GameState.CheckIn:
                break;
            case GameState.SecurityCheck:
                break;
            case GameState.LuggageTransfer:
                break;
            case GameState.Boarding:
                break;
            case GameState.Painting:
                break;
            case GameState.Finished:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newGameState), newGameState, null);
        }
        
        Debug.Log("Game state changed to: " + newGameState);
        OnGameStateChanged?.Invoke(newGameState);
    }
    
    public int GetMoney() => money;

    public void SetMoney(int amount)
    {
        money += amount;

        if (money <= 0) money = 0;
        OnMoneyChanged?.Invoke(money);
    }
    
    public int GetTotalCustomer() => totalCustomer;

    public void CompleteTask()
    {
        _processedCount++;
        Debug.Log($"İlerleme: {_processedCount}/{totalCustomer}");

        if (_processedCount >= totalCustomer)
        {
            AdvanceToNextState();
        }
    }

    private void AdvanceToNextState()
    {
        _processedCount = 0;

        switch (State)
        {
            case GameState.CheckIn:
                UpdateGameState(GameState.SecurityCheck);
                break;
            case GameState.SecurityCheck:
                UpdateGameState(GameState.LuggageTransfer);
                break;
            case GameState.LuggageTransfer:
                UpdateGameState(GameState.Boarding);
                break;
        }
    }
  
    
}

public enum GameState
{
    Idle,
    CheckIn,
    SecurityCheck,
    LuggageTransfer,
    Boarding,
    Painting,
    Finished
}

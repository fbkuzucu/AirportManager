using UnityEngine;

public class SpawnSystem : MonoBehaviour
{
    [SerializeField] private GameObject spawnPrefab;
    [SerializeField] private float spacing = 4.5f; 
    [SerializeField] private Vector3 spawnPosition; 
    [SerializeField] private Vector3 spawnRotation;

    private int _spawnCount;

    private void Start()
    {
        if (GameManager.Instance == null) return;

        _spawnCount = GameManager.Instance.GetTotalCustomer();

        GameManager.OnGameStateChanged += OnHandleGameStateChanged;
        
        if (GameManager.Instance.State == GameState.CheckIn)
        {
            Spawner();
        }
    }

    private void OnDestroy()
    {
        if(GameManager.Instance != null) GameManager.OnGameStateChanged -= OnHandleGameStateChanged;
    }

    private void OnHandleGameStateChanged(GameState newState)
    {
        if (newState == GameState.CheckIn)
        {
            Spawner();
        }
    }
    
    private void Spawner()
    {
        Quaternion startRot = Quaternion.Euler(spawnRotation);
        Vector3 forwardDir = startRot * Vector3.forward;

        for (int i = 0; i < _spawnCount; i++)
        {
            Vector3 offset = forwardDir * (i * spacing * -1); 
            Vector3 finalPos = spawnPosition + offset;
            Instantiate(spawnPrefab, finalPos, startRot);
        }
        
    }
}
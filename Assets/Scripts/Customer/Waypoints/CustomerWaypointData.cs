using UnityEngine;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif



[System.Serializable]
public class WaypointStep
{
    public Vector3 position;
    public string animationTrigger;
    public float waitTime;
    public GameState requiredStateToLeave = GameState.Idle;
}

[CreateAssetMenu(fileName = "New Coordinates Data", menuName = "Data/Coordinate List")]
public class CustomerWaypointData : ScriptableObject
{
    public List<WaypointStep> customerWaypoints = new List<WaypointStep>();
}
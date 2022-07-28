using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Party Ball/Stage Data", fileName = "Stage Data")]
public class StageData : ScriptableObject
{
    [SerializeField] GateSpawnData gateSpawnData;
    [SerializeField] List<ObstacleSpawnData> obstacleSpawnData;
    
    public GateSpawnData GateSpawnDatas { get => gateSpawnData; }
    public List<ObstacleSpawnData> ObstacleSpawnDatas { get => obstacleSpawnData; }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Party Ball/Level Data", fileName = "Level Data")]
public class LevelData : ScriptableObject
{
    [SerializeField] List<StageData> stageDatas;

    public List<StageData> StageDatas { get => stageDatas; }
}

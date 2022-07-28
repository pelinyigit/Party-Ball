using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

public class LevelManager : SingletonMonoBehaviour<LevelManager>
{
    public LevelData[] levels;
    public int currentLevelIndex;
    
    public LevelData currentLevelData;
    private ReferenceManager referenceManager;
    private int radius = 13;
    private Vector3 difference;

    public List<AIController> aICharacters;
    public CharacterProperty player;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        referenceManager = ReferenceManager.Instance;
        
        currentLevelIndex = (GameManager.instance.currentLevel - 1) % levels.Length;
        currentLevelData = levels[currentLevelIndex];

        FindAIPlayers();
        
        InitSpawnPlatformBalls();
        InitGates();
        InitObstacles();
    }
    
    private void InitSpawnPlatformBalls()
    {
        for (int i = 0; i < currentLevelData.StageDatas.Count; i++)
        {
            int gateValueOfCurrentStage = (int) currentLevelData.StageDatas[i].GateSpawnDatas.gateValue;
            int gateValueOfPreviousStage = 0;

            if (i > 0)
            {
                gateValueOfPreviousStage = (int) currentLevelData.StageDatas[i - 1].GateSpawnDatas.gateValue;
            }
            else
            {
                gateValueOfPreviousStage = 8; // Dummy stage -1
            }

            int decreaseValueOfPreviousStage = 0;

            if (i > 0)
            {
                decreaseValueOfPreviousStage = (int) currentLevelData.StageDatas[i - 1].GateSpawnDatas.decreaseMultiplier;
            }
            else
            {
                decreaseValueOfPreviousStage = 2;
            }
            
            int startvalue = FindPowerValueOfGateValue(gateValueOfPreviousStage) - decreaseValueOfPreviousStage;
            int endValue = FindPowerValueOfGateValue(gateValueOfCurrentStage);
            

            for (int j = startvalue; j < endValue; j++)
            {
                for (int k = 0; k < 3; k++) // Initial ball count
                {
                    var spawnedBall = SpawnBall(i, j);
                }
            }
        }
    }

    private GameObject SpawnBall(int stageNumber, int indexOfBallValue)
    {
        float angle = Random.Range(0, Mathf.PI * 2);
        var ballPosX = Random.Range(referenceManager.platforms[stageNumber].platformPosition.x,
            referenceManager.platforms[stageNumber].platformPosition.x - Mathf.Cos(angle) * radius);
        var ballPosY = 1.1f;
        var ballPosZ = Random.Range(referenceManager.platforms[stageNumber].platformPosition.z,
            referenceManager.platforms[stageNumber].platformPosition.z - Mathf.Sin(angle) * radius);
        var pos = new Vector3(ballPosX, ballPosY, ballPosZ);

        var spawnedBall = Instantiate(referenceManager.ballData.Balls[indexOfBallValue - 1].Prefab, pos,
            referenceManager.ballData.Balls[indexOfBallValue - 1].Prefab.transform.rotation);
        spawnedBall.GetComponent<CollectableBall>().stageNumber = stageNumber;
        return spawnedBall;
    }

    private int FindPowerValueOfGateValue(int gateValue)
    {
        float value = (float) (Math.Log(gateValue) / Math.Log(2));
        
        return Mathf.RoundToInt(value);
    }

    private void InitGates()
    {
        for (int i = 0; i < currentLevelData.StageDatas.Count; i++)
        {
            referenceManager.gates[i].UpdateGateValue((int) currentLevelData.StageDatas[i].GateSpawnDatas.gateValue);
        }
    }
    
    private void InitObstacles()
    {
        for (int i = 0; i < currentLevelData.StageDatas.Count; i++)
        {
            for (int k = 0; k < currentLevelData.StageDatas[i].ObstacleSpawnDatas.Count; k++)
            {
                Vector3 pos = referenceManager.platforms[i].transform.position + new Vector3(currentLevelData.StageDatas[i].ObstacleSpawnDatas[k].spawnPos.x, 1.3f,
                    currentLevelData.StageDatas[i].ObstacleSpawnDatas[k].spawnPos.z);
                GameObject obstacle = Instantiate(currentLevelData.StageDatas[i].ObstacleSpawnDatas[k].obstacleObj, pos,
                    currentLevelData.StageDatas[i].ObstacleSpawnDatas[k].obstacleObj.transform.rotation);
            }

        }
    }

    public void CompleteMissingBall(int stageNumber, int valueOfRemovedBall)
    {
        int indexOfRemovedBall = FindPowerValueOfGateValue(valueOfRemovedBall);

        SpawnBall(stageNumber, indexOfRemovedBall);
    }

    private void FindAIPlayers()
    {
        aICharacters = new List<AIController>();

        PlayerController[] allPlayers = FindObjectsOfType<PlayerController>();

        for (int i = 0; i < allPlayers.Length; i++)
        {
            if (allPlayers[i].characterType == PlayerController.CharacterType.AI)
            {
                aICharacters.Add(allPlayers[i].GetComponent<AIController>());
            }
            else
            {
                player = allPlayers[i].GetComponent<CharacterProperty>();
            }
        }
    }

    public void AdjustAIPlayersLevel()
    {
        for (int i = 0; i < aICharacters.Count; i++)
        {
            if (aICharacters[i].characterProperty.currentStageOfCharacter > player.currentStageOfCharacter)
            {
                aICharacters[i].ChangeCurrentAILevel(AIController.AILevel.Easy);
            }
            else if (aICharacters[i].characterProperty.currentStageOfCharacter == player.currentStageOfCharacter)
            {
                aICharacters[i].ChangeCurrentAILevel(AIController.AILevel.Medium);
            }
            else if (aICharacters[i].characterProperty.currentStageOfCharacter < player.currentStageOfCharacter)
            {
                aICharacters[i].ChangeCurrentAILevel(AIController.AILevel.Hard);
            }
            else
            {
                Debug.LogError("Invalid AI Level");
            }
        }
    }
    
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Random = UnityEngine.Random;
using DG.Tweening;

public class AIController : MonoBehaviour
{
    public AIData aIData;

    private LevelData currentLevel;
    public AIState currentState;
    public AILevel currentAILevel;
    public Transform attackTarget;

    private PlayerController playerController;
    public CharacterProperty characterProperty;
    private ReferenceManager referenceManager;

    public bool isAttack;
    public float horizontal;
    public float vertical;
    private int randomCharacters;
    private Vector3 attackDir;
    private Vector3 currentPos;
    public Transform targetBall;

    private float idleMultiplier;
    private float searchMultiplier;
    private float collisionMultiplier;
    private float attackMultiplier;
    private float idleChance;
    private float searchChance;
    private float moveChance;
    private float attackChance;

    public enum AILevel
    {
        Easy,
        Medium,
        Hard
    }

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        characterProperty = GetComponent<CharacterProperty>();
        referenceManager = ReferenceManager.Instance;
        currentLevel = LevelManager.Instance.currentLevelData;
        ChangeCurrentAILevel(AILevel.Medium);

        StartCoroutine(Decide());
        // ChangeCurrentState(startState);
    }

    public void ChangeCurrentState(AIState state)
    {
        currentState = state;
        StopAllCoroutines();
        
        switch (currentState)
        {
            case AIState.Idle:

                StartCoroutine(Idle()); // 10%
                
                break;

            case AIState.Search:
                
                StartCoroutine(Search()); // 30%

                break;
            
            case AIState.Collide:
                
                StartCoroutine(Collide()); // Only with collisions

                break;
            
            case AIState.Move:

                StartCoroutine(Move()); // 40%
                
                break;
            
            case AIState.Attack:
                
                StartCoroutine(Attack()); // 20%

                break;
            
            case AIState.Finish:
                
                StartCoroutine(Finish()); // Only if gate number is achieved

                break;

            default:
                
                Debug.LogError("Invalid AI State");
                
                break;
        }
        
        // Debug.Log(currentState);
    }

    public Vector3 AIJoystickMovement()
    {
        Vector3 dir = new Vector3(horizontal, 0f, vertical);

        return dir;
    }

    private IEnumerator Idle()
    {
        float decisionDuration = Random.Range(0.5f, 1f) * idleMultiplier;

        horizontal = 0f;
        vertical = 0f;

        yield return new WaitForSeconds(decisionDuration);

        StartCoroutine(Decide());
    }

    private IEnumerator Search()
    {
        float decisionDuration = Random.Range(0.5f, 2f) * searchMultiplier;
        
        horizontal = RandomDir();
        vertical = RandomDir();

        yield return new WaitForSeconds(decisionDuration);

        StartCoroutine(Decide());
    }
    
    private IEnumerator Collide()
    {
        float decisionDuration = Random.Range(0f, 0.5f) * collisionMultiplier;
        
        horizontal = RandomDir();
        vertical = RandomDir();

        while (currentState == AIState.Collide)
        {
            yield return new WaitForSeconds(decisionDuration);
            horizontal = RandomDir();
            vertical = RandomDir();
            StartCoroutine(Decide());

        }
    }
    
    private IEnumerator Move()
    {
        FindDirection(FindTargetBall());
        
        yield return null;
    }
    
    private IEnumerator Attack()
    {
        float decisionDuration = Random.Range(2f, 4f) * attackMultiplier;
        Transform opponent = FindOpponent();
        attackTarget = opponent;
        
        while (decisionDuration > 0f)
        {
            FindDirection(opponent);

            decisionDuration -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        
        yield return new WaitForSeconds(decisionDuration + 0.5f);
        StartCoroutine(Decide());
    }

    private IEnumerator Finish()
    {
        Transform currentActiveGate = characterProperty.currentActiveGate.transform; // To Do: Null Reference
        FindDirection(currentActiveGate);
               
        // gameObject.layer = 11;
        
        yield return null;
    }

    private void FindDirection(Transform target)
    {
        if (target == null)
        {
            ChangeCurrentState(AIState.Search);
        }
        else
        {
            Vector3 targetPos = target.transform.position;
            targetPos.y = 0f;

            currentPos = transform.position;
            currentPos.y = 0f;
        
            Vector3 dir = targetPos - currentPos;

            if (dir.magnitude == 0)
            {
                ChangeCurrentState(AIState.Search);
            }

            horizontal = dir.normalized.x;
            vertical = dir.normalized.z;
        }
    }

    private Transform FindOpponent()
    {
        List<PlayerController> allOpponents = FindObjectsOfType<PlayerController>().ToList();
        allOpponents.Remove(playerController);
        int targetOpponentIndex = Random.Range(0, allOpponents.Count);
        PlayerController targetOpponent = null;
        
        for (int i = 0; i < allOpponents.Count; i++)
        {
            if (characterProperty.currentStageOfCharacter == allOpponents[targetOpponentIndex].characterProperty.currentStageOfCharacter)
            {
                targetOpponent = allOpponents[targetOpponentIndex];

                return targetOpponent.transform;
            }
        }

        return null;
    }

    private Transform FindTargetBall()
    {
        targetBall = null;
        CollectableBall[] allBalls = FindObjectsOfType<CollectableBall>();

        if (allBalls.Length == 0)
            return null;

        for (int i = 0; i < allBalls.Length; i++)
        {
            if ((characterProperty.ballValue == allBalls[i].ballValue) && (characterProperty.currentStageOfCharacter == allBalls[i].stageNumber))  // To Do: Null reference
            {
                targetBall = allBalls[i].transform;

                return targetBall;
            }
        }

        return null;
    }
    
    public IEnumerator Decide()
    {
        AIState decision = AIState.Idle;
        
        float idleChance = this.idleChance;
        float searchChance = this.searchChance;
        float collideChance = 0f;
        float moveChance = this.moveChance;
        float attackChance = this.attackChance;
        float finishChance = 0f;

        float chance = Random.value;

        if (chance < idleChance)
        {
            decision = AIState.Idle;  // Idle
        }
        else if (chance < idleChance + searchChance)
        {
            decision = AIState.Search; // Search
        }
        else if (chance < idleChance + searchChance + collideChance)
        {
            decision = AIState.Collide; // Collide
        }
        else if (chance < idleChance + searchChance + collideChance + moveChance)
        {
            decision = AIState.Move; // Move
        }
        else if (chance < idleChance + searchChance + collideChance + moveChance + attackChance)
        {
            decision = AIState.Attack; // Attack
        }
        else if (chance < idleChance + searchChance + collideChance + moveChance + attackChance + finishChance)
        {
            decision = AIState.Finish; // Finish
        }
        else
        {
            decision = AIState.Idle; // Idle
            Debug.LogError("Chance is 1 or greater");
        }
        
        if (characterProperty.ballValue == characterProperty.currentActiveGate.gateValue) // To Do: Null reference
        {
            characterProperty.ChangeCharacterLayer(17);
            decision = AIState.Finish;
        }

        ChangeCurrentState(decision);
        
        // To Do: Finish decision
        // To Do: AI difficulty level
        // if (characterProperty.ballValue != referenceManager.gates[referenceManager.ActiveGateIndex].GetComponent<GateController>().gateValue)
        
        yield return null;
    }
    
    private float RandomDir()
    {
        return Random.Range(-1f, 1f);
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (!enabled)
            return;
        
        if ((collision.transform.CompareTag(Tags.BORDER) || collision.transform.CompareTag(Tags.GATECOLLIDER))) // Environment collision
        {
            ChangeCurrentState(AIState.Collide);
        }
        else if (collision.transform.CompareTag(Tags.AI) || collision.transform.CompareTag(Tags.PLAYER)) // Player collision
        {
            ChangeCurrentState(AIState.Collide);
        }
        else if (collision.transform.CompareTag(Tags.OBSTACLE)) // Obstacle collision
        {
            ChangeCurrentState(AIState.Collide);
        }
    }
    
    public void ChangeCurrentAILevel(AILevel level)
    {
        currentAILevel = level;

        switch (currentAILevel)
        {
            case AILevel.Easy:
                
                idleMultiplier = 1.5f;
                searchMultiplier = 1.5f;
                collisionMultiplier = 1f;
                attackMultiplier = 1.5f;
                
                idleChance = 0.2f;
                searchChance = 0.2f;
                moveChance = 0.4f;
                attackChance = 0.2f;
                
                playerController.speed = 9f;

                break;

            case AILevel.Medium:

                idleMultiplier = 1f;
                searchMultiplier = 1f;
                collisionMultiplier = 1f;
                attackMultiplier = 1f;

                idleChance = 0.1f;
                searchChance = 0.1f;
                moveChance = 0.6f;
                attackChance = 0.2f;
                
                playerController.speed = 9f;

                break;
            
            case AILevel.Hard:

                idleMultiplier = 0.5f;
                searchMultiplier = 0.5f;
                collisionMultiplier = 1f;
                attackMultiplier = 0.5f;
                
                idleChance = 0f;
                searchChance = 0.1f;
                moveChance = 0.9f;
                attackChance = 0f;
                
                DOTween.To(()=> playerController.speed, x=> playerController.speed = x, 15f, 1f);

                break;

            default:
                
                Debug.LogError("Invalid AI Level");
                
                break;
        }
    }
}
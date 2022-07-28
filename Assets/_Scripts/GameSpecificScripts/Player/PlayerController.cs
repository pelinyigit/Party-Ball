using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
    public enum CharacterType { Player, AI }
    public CharacterType characterType;
    
    [Header("DEPENDENCIES"), Space(5f)]
    public Joystick joystick;
    public GameObject fatman;
    public GameObject directionIndicator;
    public Nameplate nameplate;
    public string name;
    
    [Header("VALUES"), Space(5f)]
    public float speed;
    public Color color;
    public bool isControlDisabled;


    private AIController aIController;
    private enum AnimationState { Idle, Run }
    private AnimationState currentAnimationState;

    [HideInInspector]
    public Animator fatmanAnimator;
    [HideInInspector]
    public bool isGameStarted = false;
    [HideInInspector]
    public Rigidbody rb;
    [HideInInspector]
    public bool directionActived;

    public CharacterProperty characterProperty;
    private ReferenceManager referenceManager;  

    private void Start()
    {
        characterProperty = GetComponent<CharacterProperty>();
        aIController = GetComponent<AIController>();
        referenceManager = ReferenceManager.Instance;
        rb = GetComponent<Rigidbody>();
        fatmanAnimator = fatman.GetComponent<Animator>();
        
        ChangeCharacterType();
        //ChangeCharacterColor();

        // if (characterType == CharacterType.AI)
        // {
        //     aIController = GetComponent<AIController>();
        //     directionIndicator = null;
        // }
        // else
        //     directionIndicator.SetActive(false);
        // {
        // }
    }

    private void FixedUpdate()
    {
        if (GameManager.instance.isLevelStarted)
        {
            StartGame();
            if (characterType == CharacterType.Player)
            {
                Movement(PlayerJoystickMovement());
            }
            else if (characterType == CharacterType.AI)
            {
                Movement(aIController.AIJoystickMovement());
            }
        }
    }

    public void StartGame()
    {
        PlayerController[] allPlayers = FindObjectsOfType<PlayerController>();
        for (int i = 0; i < allPlayers.Length; i++)
        {
            allPlayers[i].GetComponent<Rigidbody>().constraints =
                RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
        }
        
        isGameStarted = true;
    }

    public void ChangeCharacterType()
    {
        if (characterType == CharacterType.Player)
        {
            gameObject.tag = Tags.PLAYER;
            aIController.enabled = false;
            name = "You";
            nameplate.UpdateCharacterName(name);
        }
        else if (characterType == CharacterType.AI)
        {
            gameObject.tag = Tags.AI;
            aIController.enabled = true;
            name = SelectRandomName();
            nameplate.UpdateCharacterName(name);
        }
    }

    public void ChangeCharacterColor()
    {
        fatman.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().material.color = ReferenceManager.Instance
            .ballData.Balls[characterProperty.ballIndex].Prefab.GetComponent<CollectableBall>().color;
    }

    private string SelectRandomName()
    {
        int randomIndex = Random.Range(0, ReferenceManager.Instance.aINames.Count);
        string randomName = ReferenceManager.Instance.aINames[randomIndex];
        ReferenceManager.Instance.aINames.RemoveAt(randomIndex);

        return randomName;
    }

    private Vector3 PlayerJoystickMovement()
    {
        var horizontal = joystick.Horizontal;
        var vertical = joystick.Vertical;
        Vector3 dir = new Vector3(horizontal, 0f, vertical);

        return dir;
    }

    private void Movement(Vector3 dir)
    {
        if (isControlDisabled)
        {
            rb.velocity = Vector3.zero;
            AnimationIdle();
            return;
        }
        
        if (dir == Vector3.zero)
        {
            rb.velocity = Vector3.zero;
            AnimationIdle();
        }
        else
        {
            rb.velocity = dir.normalized * speed;
            AnimationRun();
        }

        MainBallRotation(dir.x, dir.z);
        CharacterRotation(dir);
    }

    public void MainBallRotation(float horizontal, float vertical)
    {
        Vector3 moveDir = new Vector3(horizontal, 0f, vertical);
        if (characterType == CharacterType.Player)
            characterProperty.mainBall.transform.RotateAround(characterProperty.mainBall.transform.position, fatman.transform.right, moveDir.magnitude * speed);
        else
            characterProperty.mainBall.transform.RotateAround(characterProperty.mainBall.transform.position, fatman.transform.right, moveDir.magnitude * 5);
    }

    public void CharacterRotation(Vector3 dir)
    {
        if (dir == Vector3.zero)
            return;

        fatman.transform.rotation = Quaternion.Lerp(fatman.transform.rotation, Quaternion.LookRotation(dir), speed * Time.deltaTime);
    }

    private void AnimationIdle()
    {
        if (currentAnimationState == AnimationState.Idle || characterProperty.isRanked)
            return;
        
        currentAnimationState = AnimationState.Idle;
        fatmanAnimator.SetTrigger("Idle");
    }
    
    private void AnimationRun()
    {
        if (currentAnimationState == AnimationState.Run)
            return;
        
        currentAnimationState = AnimationState.Run;
        fatmanAnimator.SetTrigger("Run");
    }
    
    public void AnimationVictory()
    {
        fatmanAnimator.SetTrigger("Victory");
    }
    
    public void AnimationDefeat()
    {
        fatmanAnimator.SetTrigger("Defeat");
    }

    public void DisableControl()
    {
        isControlDisabled = true;
    }
    
    public void EnableControl()
    {
        if (characterProperty.isRanked)
            return;
        
        isControlDisabled = false;
    }

}





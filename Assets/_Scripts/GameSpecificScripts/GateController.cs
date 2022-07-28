using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using DG.Tweening;

public class GateController : MonoBehaviour
{
    [Header("DEPENDENCIES"), Space(5f)] 
    public Transform startPos;
    public Transform endPos;

    public GameObject gateCollider;

    private GameObject joystick;
    private bool doesCharacterPassed;
    private GameObject[] gateColliders;
    private ReferenceManager referenceManager;
    
    public int gateValue;
    public int gateOrder;

    public TextMeshProUGUI gateValueText;

    private void Start()
    {
        referenceManager = ReferenceManager.Instance;
        joystick = FindObjectOfType<VariableJoystick>().gameObject;
        gateColliders = GameObject.FindGameObjectsWithTag("GateCollider");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tags.PLAYER) || other.CompareTag(Tags.AI))
        {
            CharacterProperty characterProperty = other.gameObject.GetComponent<CharacterProperty>();
            
            if (characterProperty.ballValue == characterProperty.currentActiveGate.gateValue)
            {
                if (characterProperty.isOnGate)
                    return;

                characterProperty.currentStageOfCharacter++;

                if (characterProperty.currentStageOfCharacter >= referenceManager.gates.Count)
                {
                    characterProperty.currentStageOfCharacter = referenceManager.gates.Count - 1;
                }
                
                LevelManager.Instance.AdjustAIPlayersLevel();
                
                characterProperty.SelectActiveGate();
                characterProperty.isOnGate = true;
                characterProperty.playerController.DisableControl();
                PassTheGate(characterProperty);
            }
        }
    }
    
    private void PassTheGate(CharacterProperty character)
    {
        character.playerController.directionIndicator.SetActive(false);
        character.transform.GetComponent<Rigidbody>().isKinematic = true;
        
        character.transform.DOMove(startPos.position, 0.25f).SetEase(Ease.Linear).OnComplete(() =>
        {
            if (gateOrder >= 2) // endgame
            {
                Ranking.Instance.StartRanking(character);
                
                character.mainBall.transform.DOLocalRotate(new Vector3(30f, 0f, 0f), 1f);
                character.playerController.fatman.transform.DOLocalRotate(new Vector3(0f, 180f, 0f), 1f);
                
                character.transform.DOMove(Ranking.Instance.targetPositions[character.finalRank - 1].position, 1.5f).SetEase(Ease.OutSine).OnComplete(() =>
                {
                    character.playerController.AnimationVictory();
                    DecideLevelSuccessOrFail();
                });
            }
            else
            {
                StartCoroutine(PrepareForNextStage(character));
            
                character.transform.DOMove(endPos.position, 1.5f).SetEase(Ease.OutSine).OnComplete(() =>
                {
                    character.transform.GetComponent<Rigidbody>().isKinematic = false;
                    character.isOnGate = false;
                    character.playerController.EnableControl();
                    character.ChangeCharacterLayer(6);
                });
            }
            
            

        });
    }

    private IEnumerator PrepareForNextStage(CharacterProperty character)
    {
        int decreaseCount = (int) LevelManager.Instance.currentLevelData.StageDatas[gateOrder].GateSpawnDatas
            .decreaseMultiplier;

        for (int i = 0; i < decreaseCount; i++)
        {
            yield return new WaitForSeconds(0.1f + (i * 0.025f));
            character.DecreaseBall(true);
        }
    }

    private void CloseTheGate()
    {
        foreach (GameObject gateCollider in gateColliders)
        {
            gateCollider.GetComponent<MeshCollider>().enabled = true;
        }
    }

    private void OpenTheGate()
    {
        foreach (GameObject gateCollider in gateColliders)
        {
            gateCollider.GetComponent<MeshCollider>().enabled = false;
        }
    }

    // private void ChangeToGreen()
    // {
    //     for (int i = 0; i < referenceManager.ballData.Balls.Length; i++)
    //     {
    //         if (player.GetComponent<CharacterProperty>().ballValue == referenceManager.ballData.Balls[i].BallValue)
    //         {
    //             player.transform.GetChild(0).GetComponent<MeshRenderer>().material = referenceManager.ballData.Balls[i].GreenMaterial;
    //         }
    //     }
    // }

    public void ChangeGateColor(CharacterProperty characterProperty)
    {
        if (characterProperty.ballValue == characterProperty.currentActiveGate.gateValue)
        {
            characterProperty.currentActiveGate.transform.GetChild(0).GetComponent<MeshRenderer>().material.DOColor(Color.green, .8f);
        }
        else
        {
            characterProperty.currentActiveGate.transform.GetChild(0).GetComponent<MeshRenderer>().material.DOColor(Color.red, .8f);
        }
    }
    
    private void PlaceSecondAndThirdCharacters()
    {
        CharacterProperty secondCharacter = Ranking.Instance.rankedCharacters[1];
        MoveCharacterToRankPosition(secondCharacter, Ranking.Instance.targetPositions[1].position);
        
        CharacterProperty thirdCharacter = Ranking.Instance.rankedCharacters[2];
        MoveCharacterToRankPosition(thirdCharacter, Ranking.Instance.targetPositions[2].position);
    }
    
    private void PlaceThirdCharacter()
    {
        CharacterProperty thirdCharacter = Ranking.Instance.rankedCharacters[2];
        MoveCharacterToRankPosition(thirdCharacter, Ranking.Instance.targetPositions[2].position);
    }

    private void MoveCharacterToRankPosition(CharacterProperty character, Vector3 targetPos)
    {
        DOTween.Kill(character.transform);
        character.isOnGate = true;
        character.playerController.DisableControl();
        
        character.playerController.directionIndicator.SetActive(false);
        character.transform.GetComponent<Rigidbody>().isKinematic = true;
        character.transform.GetComponent<SphereCollider>().enabled = false;
        
        character.playerController.AnimationVictory();

        character.transform.DOMove(targetPos, 1f);
        character.mainBall.transform.DOLocalRotate(new Vector3(30f, 0f, 0f), 1f);
        character.playerController.fatman.transform.DOLocalRotate(new Vector3(0f, 180f, 0f), 1f);
    }
    
    
    private void StopRemainingCharacters(int index)
    {
        for (int i = index; i < Ranking.Instance.rankedCharacters.Count; i++)
        {
            Ranking.Instance.rankedCharacters[i].playerController.AnimationDefeat();
            Ranking.Instance.rankedCharacters[i].playerController.DisableControl();
            Ranking.Instance.rankedCharacters[i].playerController.directionIndicator.SetActive(false);
        }
    }

    private void DecideLevelSuccessOrFail()
    {
        int count = Ranking.Instance.rankedCharacters.Count;
        
        for (int i = 0; i < count; i++)
        {
            if (Ranking.Instance.rankedCharacters[0].playerController.characterType == // First
                PlayerController.CharacterType.Player)
            {
                GameManager.instance.LevelComplete();
                TestCameraFollow.Instance.PositionCameraToRanking();
                PlaceSecondAndThirdCharacters();
                StopRemainingCharacters(1);
                return;
            }
            
            if (Ranking.Instance.rankedCharacters[1].playerController.characterType == // Second
                     PlayerController.CharacterType.Player)
            {
                GameManager.instance.LevelComplete();
                TestCameraFollow.Instance.PositionCameraToRanking();
                PlaceThirdCharacter();
                StopRemainingCharacters(2);
                return;
            }
            
            if (Ranking.Instance.rankedCharacters[2].playerController.characterType == // Third
                     PlayerController.CharacterType.Player)
            {
                GameManager.instance.LevelComplete();
                TestCameraFollow.Instance.PositionCameraToRanking();
                StopRemainingCharacters(3);
                return;
            }
        }

        if (count >= 3)
        {
            GameManager.instance.LevelFail();
        }
    }

    public void UpdateGateValue(int value)
    {
        gateValue = value;
        gateValueText.text = value.ToString();
    }
    
}

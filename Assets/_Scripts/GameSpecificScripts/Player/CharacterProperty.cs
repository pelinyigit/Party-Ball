using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class CharacterProperty : MonoBehaviour
{

    private GameObject fatman;
    public GameObject mainBall;
    private MeshRenderer ballMaterial;
    public PlayerController playerController;
    private AIController aIController;
    private SphereCollider playerCollider;
    private GameObject cloneBall;
    public bool isOnGate;
    public bool isRanked;
    public int finalRank;

    [HideInInspector]
    public bool changeNewBallDone = false;


    public int currentStageOfCharacter;
    public int ballValue;
    public int ballIndex;


    [HideInInspector]
    public float yPos;

    private bool collisionOnce = false;
    private ReferenceManager referenceManager;
    public GateController currentActiveGate;


    void Start()
    {
        playerCollider = transform.GetComponent<SphereCollider>();
        playerController = GetComponent<PlayerController>();
        aIController = GetComponent<AIController>();
        fatman = transform.GetChild(1).gameObject;

        referenceManager = ReferenceManager.Instance;
        yPos = transform.position.y;
        SelectActiveGate();
    }

    public void IncreaseBall()
    {
        if (!GameManager.instance.isLevelStarted)
            return;

        LevelManager.Instance.CompleteMissingBall(currentStageOfCharacter, ballValue);
        
        UpdateBallValue(1);
        
        Ball[] balls = ReferenceManager.Instance.ballData.Balls;

        for (int i = 0; i < balls.Length; i++)
        {
            CollectableBall collectableBall = balls[i].Prefab.GetComponent<CollectableBall>();
            if (ballValue == collectableBall.ballValue)
            {
                playerCollider.radius += 0.05f;
                playerCollider.center += Vector3.up * 0.05f;
                ChangeMainBallMaterial();

                Vector3 collectableBallLocalScale = collectableBall.transform.localScale;

                ChangeMainBallSize(collectableBallLocalScale);
                ChangeMainBallYPos(collectableBallLocalScale.y * 0.5f);
                ChangeFatmanYPos(collectableBallLocalScale.y);
                //playerController.ChangeCharacterColor();

                if (playerController.characterType == PlayerController.CharacterType.AI)
                {
                    aIController.StartCoroutine(aIController.Decide());
                }
                else
                {
                    if (ballValue == currentActiveGate.gateValue)
                    {
                        playerController.directionIndicator.SetActive(true);
                        ChangeCharacterLayer(17);
                        currentActiveGate.GetComponent<GateController>().ChangeGateColor(this);
                    }

                }
            }
        }
    }
    
    public void DecreaseBall(bool destroy)
    {
        if (!GameManager.instance.isLevelStarted)
            return;
        
        if (ballValue <= 2)
            return;
        
        UpdateBallValue(-1);

        Ball[] balls = ReferenceManager.Instance.ballData.Balls;

        for (int i = 0; i < balls.Length; i++)
        {
            CollectableBall collectableBall = balls[i].Prefab.GetComponent<CollectableBall>();
            if (ballValue == collectableBall.ballValue)
            {
                playerCollider.radius += -0.05f;
                playerCollider.center += Vector3.up * -0.05f;
                ChangeMainBallMaterial();

                Vector3 collectableBallLocalScale = collectableBall.transform.localScale;
                if (!isOnGate)
                {
                    ChangeMainBallSize(collectableBallLocalScale);
                    ChangeFatmanYPos(collectableBallLocalScale.y);
                }
                else
                {
                    ChangeMainBallSizeDirectly(collectableBallLocalScale);
                    ChangeFatmanYPosDirectly(collectableBallLocalScale.y);
                }
                
                ChangeMainBallYPos(collectableBallLocalScale.y * 0.5f);
                //playerController.ChangeCharacterColor();
                ThrowDecreasedBall(collectableBall, destroy);

                if (playerController.characterType == PlayerController.CharacterType.AI)
                {
                    aIController.StartCoroutine(aIController.Decide());
                }
                
                // Throw a ball
            }
        }
    }

    private void ThrowDecreasedBall(CollectableBall decreasedBall, bool destroy)
    {
        Vector3 spawnPos = transform.position;
        Vector3 forceDir = Vector3.zero;
        GameObject decreasedBallGO = Instantiate(decreasedBall.gameObject, spawnPos, Quaternion.identity);
        
        CollectableBall collectableBall = decreasedBallGO.GetComponent<CollectableBall>();

        collectableBall.StartCoroutine(collectableBall.EnableTrail());
        collectableBall.StartCoroutine(collectableBall.MakeUncollectable());
        
        
        if (destroy)
        {
            forceDir = new Vector3(Random.Range(-1f, 1f), 1f, 0f);
            Destroy(collectableBall);
            Destroy(decreasedBallGO, 1.5f);
        }
        else
        {
            forceDir = new Vector3(Random.Range(-1f, 1f), 1f, Random.Range(-1f, 1f));
        }
        
        decreasedBallGO.GetComponent<Rigidbody>().AddForce(forceDir * 15f, ForceMode.Impulse);
        




    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(Tags.COLLECTIBLEBALL))
        {
            if (ballValue == collision.gameObject.GetComponent<CollectableBall>().ballValue)
            {
                IncreaseBall();
                Destroy(collision.gameObject);
                
                // if (referenceManager.ActiveGateIndex != referenceManager.gates.Count)
                // {
                //     // referenceManager.gates[referenceManager.ActiveGateIndex].GetComponent<GateController>().ChangeGateColor();
                // }
            }
        }
        else if (collision.gameObject.CompareTag(Tags.AI) || collision.gameObject.CompareTag(Tags.PLAYER))
        {
            CharacterProperty opponent = collision.gameObject.GetComponent<CharacterProperty>();
            
            if (ballValue == opponent.ballValue)
            {
                // Do nothing
            }
            else if (ballValue < opponent.ballValue)
            {
                DecreaseBall(false);
            }
            else if (ballValue > opponent.ballValue)
            {
                // Do nothing
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "AI")
        {
            changeNewBallDone = false;
        }
    }

    IEnumerator RestartCollision()
    {
        yield return new WaitForSeconds(1f);
        collisionOnce = false;
    }

    private void SetCloneBallProperties(GameObject cloneBall, Vector3 force)
    {
        cloneBall.GetComponent<Rigidbody>().AddForce(new Vector3(3, 5, 3), ForceMode.Impulse);
        cloneBall.GetComponent<Rigidbody>().AddTorque(new Vector3(3, 5, 3), ForceMode.Impulse);
        // cloneBall.tag = "Untagged";
        //  cloneBall.layer = 10;
    }

    public void ChangeFatmanYPos(float fatmanPosY)
    {
        fatman.transform.DOLocalMoveY(fatmanPosY + 0.75f, .2f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            fatman.transform.DOLocalMoveY(fatmanPosY, .1f).SetEase(Ease.Linear);
        });
    }
    
    public void ChangeFatmanYPosDirectly(float fatmanPosY)
    {
        fatman.transform.DOLocalMoveY(fatmanPosY, .2f).SetEase(Ease.Linear);
    }

    public void ChangeFatmanAnimation()
    {
        fatman.GetComponent<Animator>().SetTrigger("Level");
    }

    public void ChangeMainBallYPos(float yPos)
    {
        mainBall.transform.localPosition = new Vector3(0f, yPos, 0f);
    }

    public void ChangeMainBallSize(Vector3 scale)
    {
        mainBall.transform.DOScale(scale * 1.4f, .2f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            mainBall.transform.DOScale(scale, .1f).SetEase(Ease.Linear);
        });
    }
    
    public void ChangeMainBallSizeDirectly(Vector3 scale)
    {
        mainBall.transform.DOScale(scale, .2f).SetEase(Ease.Linear);
    }

    public void ChangeMainBallMaterial()
    {
        mainBall.GetComponent<MeshRenderer>().material =
            ReferenceManager.Instance.ballData.Balls[ballIndex].NormalMaterial;
    }

    public void UpdateBallValue(int change)
    {
        if (change == 1)
        {
            ballValue *= 2;
            ballIndex += 1;
        }
        else if (change == -1)
        {
            ballValue /= 2;
            ballIndex -= 1;
        }
        else
        {
            Debug.LogError("Invalid change value");
        }
    }

    public void ChangeCharacterLayer(int layerIndex)  // 17 for pass, 6 for not pass
    {
        gameObject.layer = layerIndex;
    }

    public void SelectActiveGate()
    {
        currentActiveGate = ReferenceManager.Instance.gates[currentStageOfCharacter];
    }
}

using UnityEngine;
using System;

[DefaultExecutionOrder(-100)]
public class EventManager : SingletonMonoBehaviour<EventManager>
{
    protected override void Awake()
    {
        base.Awake();
    }

    //A Character trigger the cube
    public event Action<CollectableBall, GameObject> OnCharacterTriggerEnterBall;
    public void CharacterTriggerEnterBall(CollectableBall collectableBall, GameObject characterGO)
    {
        OnCharacterTriggerEnterBall(collectableBall, characterGO);
    }

    public event Action<CollectableBall, CharacterController> OnObstacleTriggerEnterBall;
    public void ObstacleTriggerEnterBall(CollectableBall collectableBall, CharacterController characterController)
    {
        OnObstacleTriggerEnterBall(collectableBall, characterController);
    }
}

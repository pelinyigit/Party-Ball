using UnityEngine;

[CreateAssetMenu(menuName = "Party Ball/AI Data", fileName = "AI Data")]
public class AIData : CharacterData
{
    [SerializeField, Range(0f, 10f)] public float decideDuration = 5f;
    [SerializeField, Range(1f, 20f)] float moveSpeed = 5f;

    public float MoveSpeed { get => moveSpeed; }
    public float DecideDuration { get => decideDuration; }

}
using UnityEngine;

public class CharacterData : ScriptableObject
{
    [SerializeField] Material material;

    public Material Material { get => material; }
}

using System;
using UnityEngine;

[Serializable]
public class Ball
{
    [SerializeField] GameObject prefab;
    [SerializeField] int ballValue;
    [SerializeField] Material normalMaterial;
    [SerializeField] Material greenMaterial;

    public GameObject Prefab { get => prefab; }
    public int BallValue { get => ballValue; }
    public Material NormalMaterial { get => normalMaterial; }
    public Material GreenMaterial { get => greenMaterial; }
}

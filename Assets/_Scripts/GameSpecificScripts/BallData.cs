using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Party Ball/Ball Data", fileName = "BallData")]
public class BallData : ScriptableObject
{
    [SerializeField] Ball[] balls;
    public Ball[] Balls { get => balls; }
}
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GateSpawnData
{
    public BallValue gateValue;
    public DecreaseValue decreaseMultiplier;
    
    public enum BallValue
    {
        Ball2 = 2,
        Ball4 = 4,
        Ball8 = 8,
        Ball16 = 16,
        Ball32 = 32,
        Ball64 = 64,
        Ball128 = 128,
        Ball256 = 256,
        Ball512 = 512,
        Ball1024 = 1024,
        Ball2048 = 2048
    }
    
    public enum DecreaseValue
    {
        Decrease1 = 1,
        Decrease2 = 2,
        Decrease3 = 3,
        Decrease4 = 4,
        Decrease5 = 5,
    }
}
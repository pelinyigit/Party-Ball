using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferenceManager : SingletonMonoBehaviour<ReferenceManager>
{
    [Header("BALLS"), Space(5f)]
    public BallData ballData;

    [Header("PLATFORMS"), Space(5f)]
    public PlatformController[] platforms;

    [Header("GATES"), Space(5f)]
    public List<GateController> gates;
    
    [Header("AI NAMES"), Space(5f)]
    public List<string> aINames;

    public int activeGateIndex;

    // public GateController activeGate;

    protected override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        
    }
    
}

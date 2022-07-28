using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    [HideInInspector]
    public Vector3 platformPosition;

    public int stageNumber;

    void Awake()
    {
        platformPosition = transform.localPosition;
    }
}

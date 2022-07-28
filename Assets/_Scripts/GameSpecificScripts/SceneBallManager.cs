using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneBallManager : MonoBehaviour
{
    [HideInInspector]
    public Vector3 size;

    [Header("COMPONENTS"), Space(5f)]
    [SerializeField]
    public MeshRenderer meshRenderer;
    [HideInInspector]
    public Material material;
    [HideInInspector]
    public float collectableYPos;

    [Header("BALL DATA "), Space(5f)]
    public int collectableBallValue;

    void Start()
    {
        size = transform.localScale;
        meshRenderer = GetComponent<MeshRenderer>();
        material = meshRenderer.material;
        collectableYPos = transform.localPosition.y;
    }
}

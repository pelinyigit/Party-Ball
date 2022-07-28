using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateFinder : MonoBehaviour
{
    public CharacterProperty characterProperty;
    void Start()
    {
        
    }
    
    void Update()
    {
        Vector3 pos = characterProperty.currentActiveGate.transform.position;
        pos.y = transform.position.y;
        transform.LookAt(pos);
    }
}

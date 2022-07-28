using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveGateController : MonoBehaviour
{
    private ReferenceManager referenceManager;

    private void Start()
    {
        referenceManager = ReferenceManager.Instance;    
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // referenceManager.SetActiveGate(referenceManager.activeGateIndex++);
        }
    }
}

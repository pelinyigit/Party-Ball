using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag(Tags.PLAYER) || other.gameObject.CompareTag(Tags.AI))
        {
            CharacterProperty characterProperty = other.gameObject.GetComponent<CharacterProperty>();

            characterProperty.DecreaseBall(false);
        }
    }
}

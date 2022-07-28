using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallTriggerArea : MonoBehaviour
{
    [SerializeField] CollectableBall collectableBall;
    private void Start()
    {
        collectableBall = transform.parent.GetComponent<CollectableBall>();
    }
    private void OnTriggerEnter(Collider other)
    {
       // if (other.CompareTag("Character"))
       // {
       //     EventManager.Instance.CharacterTriggerEnterBall(collectableBall, other.gameObject);
       // }
       //else
       if (other.CompareTag("Player"))
        {
            //Debug.LogError(" player test trigger");
        //    EventManager.Instance.CharacterTriggerEnterBall(collectableBall, other.gameObject);
        }
        //if (other.CompareTag("Obstacle"))
        //{
        //    EventManager.Instance.ObstacleTriggerEnterBall(collectableBall, CollectableBall .GetCollecter());
        //}
    }
}


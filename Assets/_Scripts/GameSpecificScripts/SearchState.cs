using System.Collections;
using UnityEngine;

public class SearchState : AIBaseState
{
    public override IEnumerator Enter(AIController ai)
    {
        //ai.state = AIState.Search;

        //var ball = FindClosestBall();

        //if (ball != null)
        //{

        //    ai.SwitchState(new MoveState(ball));
        //}
        //else
        //{
        //    ai.SwitchState(new IdleState());
        //}


        //CollectableBall FindClosestBall()
        //{
        //    CollectableBall closestBall = null;

        //    var balls = GameObject.FindObjectsOfType<CollectableBall>();

        //    float maxDist = Mathf.Infinity;

        //    foreach (var ball in balls)
        //    {
        //        float dist = (ball.transform.position - ai.transform.position).sqrMagnitude;
        //        if (ball.collectableBallValue == ai.GetComponent<CharacterProperty>().ballvalue && 
        //            ball.CanCollectable() && dist < maxDist && ball.CanCollectable() &&
        //            ball.stageNumber == 1)
        //            if (dist < maxDist && ball.CanCollectable())
        //            {
        //                closestBall = ball;
        //                maxDist = dist;
        //            }
           
        //    }

        //    return closestBall;
        //}

        yield return null;
    }

    public override void Update(AIController ai)
    {

    }
}

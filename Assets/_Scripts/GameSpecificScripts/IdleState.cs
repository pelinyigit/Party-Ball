using System.Collections;
using UnityEngine;

public class IdleState : AIBaseState
{
    public override IEnumerator Enter(AIController ai)
    {
       // ai.state = AIState.Idle;

       //ai.SwitchAnimation("Idle");

        yield return null;
    }

    public override void Update(AIController ai)
    {

    }
}
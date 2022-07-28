using System.Collections;
using UnityEngine;

public class DecideState : AIBaseState
{
    int rnd;
    public override IEnumerator Enter(AIController ai)
    {
        //ai.state = AIState.Decide;

        //yield return new WaitForSeconds(ai.aIData.DecideDuration);

        //Decide();
        //void Decide()
        //{
        //    rnd = Random.Range(0, 5);

        //    if (rnd == 4)
        //    {

        //        ai.SwitchState(new SearchState());
        //    }
        //    else if (rnd == 3)
        //    {

        //        ai.SwitchState(new RandomMove());
        //    }
        //    else
        //    {
        //        ai.SwitchAnimation("Idle");
        //        Decide();
        //    }
        //}


        yield return null;
    }

    public override void Update(AIController ai)
    {

    }
}

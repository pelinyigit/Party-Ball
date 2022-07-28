using System.Collections;
using DG.Tweening;
using UnityEngine;
using TMPro;
using DG.Tweening;
public class RandomMove : AIBaseState
{
    bool canUpdate = false;
    float randX;
    float randY;

    public override IEnumerator Enter(AIController ai)
    {
        //if (ai != null)
        //{
        //    ai.state = AIState.Move;

        //    Tween rotateTween;
        //    yield return GetRotateTween().Play().WaitForCompletion();

        //    canUpdate = true;



        //    Tween GetRotateTween()
        //    {



        //        Vector3 target = new Vector3(randX, 0, randY);
        //        var dir = (target - ai.transform.position).normalized;


        //        var lookRot = Quaternion.LookRotation(dir);

        //        float dot = Mathf.Abs(Vector3.Dot(ai.transform.position.normalized, dir));

        //        float duration = 1 - dot;

        //        duration = Mathf.Clamp(duration, 1 - dot, .15f);
        //        /*Tween*/
        //        rotateTween = ai.transform.DORotate(lookRot.eulerAngles, duration)
       
        //      .SetAutoKill(true);
        //        //.SetRecyclable(true)
        //        //.OnStart(delegate
        //        //{
        //        //    if (ai.GetStackCount() == 0)
        //        //    {
        //        //        ai.SwitchAnimation("Run");
        //        //    }
        //        //});

        //        return rotateTween;

        //    }
        //}
        //    canUpdate = true;
        //randX = Random.Range(2, 10);
        //randY = Random.Range(2, 10);
        yield return null;
    }

    public override void Update(AIController ai)
    {
        //if (!canUpdate) return;
        //MoveSpaceShip(ai);

        //void MoveSpaceShip(AIController ai)
        //{

        //    Vector3 target = new Vector3(randX, 0, randY);
        //    //Option 1
        //    //small_star.transform.Translate(target * Time.deltaTime);
        //    //Option 2

        //    ai.transform.DOMove(new Vector3(target.x, ai.transform.position.y, target.z), 1);
        //    //ai.transform.Translate(new Vector3(target.x, ai.transform.position.y, target.z) * Time.deltaTime *
        //    //    ai.aIData.MoveSpeed, Space.World);


        //    //ai.SwitchState(new DecideState());

        //}
    }
}

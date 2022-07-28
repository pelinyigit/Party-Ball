using System.Collections;
using DG.Tweening;
using UnityEngine;
using TMPro;

public class MoveState : AIBaseState
{
    private CollectableBall targetCollectableBall;

    private CharacterProperty characterProperty;
    public MoveState(CollectableBall cube)
    {

        targetCollectableBall = cube;

    }


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



        //        var dir = (targetCollectableBall.transform.position - ai.transform.position).normalized;


        //        var lookRot = Quaternion.LookRotation(dir);

        //        float dot = Mathf.Abs(Vector3.Dot(ai.transform.position.normalized, dir));

        //        float duration = 1 - dot;

        //        duration = Mathf.Clamp(duration, 1 - dot, .15f);
        //        /*Tween*/
        //        rotateTween = ai.transform.DORotate(lookRot.eulerAngles, duration)
        //      .SetLink(targetCollectableBall.gameObject)
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
        yield return null;

    }
    public override void Update(AIController ai)
    {
        //if (!canUpdate) return;

        //CheckTarget();

        //Move();

        //void CheckTarget()
        //{
        //    if (!targetCollectableBall.CanCollectable() || targetCollectableBall == null)
        //    {
        //       int va= StageManager.Instance.GateList();
        //        if (ai.GetComponent<CharacterProperty>().ballvalue ==
        //        StageManager.Instance.gateValue)
        //        {
        //            var dir = (StageManager.Instance.gates[StageManager.Instance.GateList()]
        //                .transform.position - ai.transform.position).normalized;

        //            //ai.transform.Translate(new Vector3(dir.x, ai.transform.position.y, dir.z) * Time.deltaTime * ai.aIData.MoveSpeed, Space.World);
        //            MainBallRotation(ai, -dir.x, dir.y);
        //            Debug.LogError(" test kapi");
        //        }
        //        else
        //        {
        //            Debug.LogError("  test baska ball");
        //            //ai.SwitchAnimation("Surf");
        //            //     if (ai.collect)
        //            //ai.SwitchState(new DecideState());
        //        }
        //    }
        //}

        //void Move()
        //{

        //    if (targetCollectableBall.collectableBallValue == ai.GetComponent<CharacterProperty>().ballvalue)
        //    {
        //        var dir = (targetCollectableBall.transform.position - ai.transform.position).normalized;

        //        //ai.transform.Translate(new Vector3(dir.x, ai.transform.position.y, dir.z) * Time.deltaTime * ai.aIData.MoveSpeed, Space.World);
        //        MainBallRotation(ai, -dir.x, dir.y);
        //        Debug.LogError(" test");
        //    }



        //}




    }


    private void MainBallRotation(AIController ai, float horizontal, float vertical)
    {
        ai.transform.GetChild(1).transform.Rotate(-vertical * 5f, 0, horizontal * 5f, Space.World);
    }

}

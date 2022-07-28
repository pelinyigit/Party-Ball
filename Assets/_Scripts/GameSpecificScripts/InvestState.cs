using System.Collections;
using DG.Tweening;
using UnityEngine;

public class InvestState : AIBaseState
{
    private InvestPoint closestInvestPoint;

    public override IEnumerator Enter(AIController ai)
    {
        //ai.state = AIState.Invest;

        yield return RotateTween().Play().WaitForCompletion();

        closestInvestPoint = FindClosestInvestPoint();

        canUpdate = true;

        Tween RotateTween()
        {
            var investPoint = FindClosestInvestPoint();

            var dir = (investPoint.transform.position - ai.transform.position).normalized;

            var lookRot = Quaternion.LookRotation(dir);

            Tween rotateTween = ai.transform.DORotate(lookRot.eulerAngles, .25f)
                .OnStart(delegate
                {

                });

            return rotateTween;
        }

        InvestPoint FindClosestInvestPoint()
        {
            InvestPoint closestInvestPoint = null;

            var investPoints = GameObject.FindObjectsOfType<InvestPoint>();

            float maxDist = Mathf.Infinity;

            foreach (var investPoint in investPoints)
            {
                float dist = (investPoint.transform.position - ai.transform.position).sqrMagnitude;

                if (dist < maxDist)
                {
                    closestInvestPoint = investPoint;
                    maxDist = dist;
                }
            }

            return closestInvestPoint;
        }

        yield return null;
    }

    public override void Update(AIController ai)
    {
        if (!canUpdate) return;

        Move();

        void Move()
        {
            var threshold = 2f;

            var dist = (closestInvestPoint.transform.position - ai.transform.position).sqrMagnitude;

            if (dist <= threshold)
            {
                //ai.SwitchState(new SearchState());
            }

            var dir = (closestInvestPoint.transform.position - ai.transform.position).normalized;

            //ai.transform.Translate(dir * Time.deltaTime * ai.aIData.MoveSpeed, Space.World);
        }
    }
}

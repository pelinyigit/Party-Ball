using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using DG.Tweening;
public abstract class CharacterController : MonoBehaviour
{
    protected const float YOFFSET =.5F;

    //Model transform 
    [SerializeField] protected Transform modelTransform;
    [SerializeField] protected Animator animator;
    
    protected Vector3 lastStackPos = Vector3.zero;

    protected List<CollectableBall> balls = new List<CollectableBall>();

    [HideInInspector]
    public Animator Animator => animator;

    #region Event Subscribe
    virtual protected void OnEnable()
    {
        EventManager.Instance.OnCharacterTriggerEnterBall += CollectableBall;
        EventManager.Instance.OnObstacleTriggerEnterBall += DropBall;
    }

    virtual protected void OnDisable()
    {
        EventManager.Instance.OnCharacterTriggerEnterBall -= CollectableBall;
        EventManager.Instance.OnObstacleTriggerEnterBall -= DropBall;
    }
    #endregion

    #region Cube Collect and Drop
    virtual public void CollectableBall(CollectableBall ball, GameObject go)
    {
        if (ball.CanCollectable())
        {
           //s ball.Collect(this, CalculateStackPos(), GetMaterial());
           //s balls.Insert(0, ball);
         //   ModelLocalJump( ball);
            //Debug.LogError(" player test");

        }
    }

    virtual public void DropBall(CollectableBall ball, CharacterController characterController)
    {
        if (ball.CanDropable() && characterController == this)
        {
            if (ball != balls[balls.Count - 1])
            {
                var ballIndex = balls.IndexOf(ball);
                var underCube = balls[ballIndex + 1];
                DropBall(underCube, this);
            }

            CalculateStackPos();
            ball.Drop();
          //  cubes.Remove(ball);
        }
    }

    virtual protected Vector3 CalculateStackPos()
    {
       
            lastStackPos = modelTransform.localPosition;
      
        return lastStackPos;
    }

    virtual protected void ModelLocalJump(CollectableBall ball)
    {
         /// modelTransform.localPosition += Vector3.up *(ball.transform.position.y+ YOFFSET)   * 1.5f;
        // modelTransform.position = new Vector3(modelTransform.transform.position.x, YOFFSET*1.5f, modelTransform.transform.position.z);
    }
    #endregion

    protected abstract Material GetMaterial();

    public virtual int GetStackCount()
    {
        return balls.Count;
    }
}


using System.Collections;
using DG.Tweening;
using UnityEngine;

[SelectionBase]
public class CollectableBall : MonoBehaviour
{
    public int ballValue;
    public int stageNumber;
    public Color color;
    public TrailRenderer trailRenderer;
    public bool isCollected = false;

    private Rigidbody rb;
    private CharacterController character;

    void Start()
    {
        SpawnAnimation();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag(Tags.BORDER))
        {
            rb = GetComponent<Rigidbody>(); 
            var force = (collision.transform.position - transform.position).normalized;
            rb.AddForce(force * 10f, ForceMode.Impulse);
        }
    }

    public IEnumerator EnableTrail()
    {
        trailRenderer.material.color = color;
        trailRenderer.emitting = true;
        yield return new WaitForSeconds(1.5f);
        trailRenderer.emitting = false;
        
    }
    
    public IEnumerator MakeUncollectable()
    {
        gameObject.layer = 16;
        yield return new WaitForSeconds(0.5f);
        gameObject.layer = 10;
    }

    public void Collect(CharacterController character, Vector3 stackPos, Material material)
    {
        if (!isCollected)
        {
            isCollected = true;
            RbCollectSetUp();
            LocalRotationReset();
            this.character = character;
        }
    }
    public void Drop()
    {
        if (isCollected)
        {
            isCollected = false;
            RbDropSetUp();
            character = null;
        }
    }

    private void SpawnAnimation()
    {
        Vector3 startScale = transform.localScale;

        transform.localScale = Vector3.zero;
        transform.DOScale(startScale, 0.25f);
    }

    private void GoToLocalStackPosition(Vector3 localPos)
    {
        transform.localPosition = localPos;
    }
    
    private void LocalRotationReset()
    {
        transform.localRotation = Quaternion.identity;
    }

    #region Rb SetUp
    private void RbCollectSetUp()
    {
        gameObject.SetActive(false);
        SwitchRbConstraints();
        
    }

    private void RbDropSetUp()
    {
        gameObject.SetActive(true); 
    }

    private void SwitchRbConstraints()
    {
        switch (isCollected)
        {
            case true:
                gameObject.SetActive(false);
                rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionX
                   | RigidbodyConstraints.FreezeRotation;
                break;

            case false:
                rb.constraints = RigidbodyConstraints.None;
                break;
        }
    }
    

    public CharacterController GetCollecter()
    {
        return character;
    }

    #endregion
    #region Interactable
    public bool CanCollectable()
    {
        return !isCollected;
    }

    public bool CanDropable()
    {
        return isCollected;
    }
    #endregion
}

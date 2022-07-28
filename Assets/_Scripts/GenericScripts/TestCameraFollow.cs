using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TestCameraFollow : SingletonMonoBehaviour<TestCameraFollow>
{
    public Transform player;
    public Vector3 offset;
    private bool letFollow;

    protected override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        letFollow = true;
        offset = transform.position - player.transform.position;
    }
    
    void Update()
    {
        if(!letFollow)
            return;
        
        transform.position = Vector3.Lerp(transform.position, player.position + offset, Time.deltaTime * 10f);
    }

    public void PositionCameraToRanking()
    {
        letFollow = false;
        transform.DOMove(Ranking.Instance.cameraPosition.position, 1f);
        transform.DORotate(Ranking.Instance.cameraPosition.rotation.eulerAngles, 1f);
    }
}

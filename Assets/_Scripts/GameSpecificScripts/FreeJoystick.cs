using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class FreeJoystick : Joystick
{
    private GameObject fatman;

    protected override void Start()
    {
        base.Start();
        background.gameObject.SetActive(false);
    
        fatman = FindObjectOfType<PlayerController>().transform.GetChild(0).gameObject;


    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        fatman.transform.GetComponent<Animator>().SetTrigger("Run");
        background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
        background.gameObject.SetActive(true);
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        fatman.transform.GetComponent<Animator>().SetTrigger("Idle");
        background.gameObject.SetActive(false);
        base.OnPointerUp(eventData);
    }
}

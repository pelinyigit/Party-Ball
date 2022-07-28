using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Nameplate : MonoBehaviour
{
    private Transform mainCamera;
    public TextMeshProUGUI text;

    void Start()
    {
        mainCamera = Camera.main.transform;
    }

    void Update()
    {
        transform.LookAt(mainCamera);
    }

    public void UpdateCharacterName(string name)
    {
        text.text = name;
    }
}

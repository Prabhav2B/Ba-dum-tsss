using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardSprite : MonoBehaviour
{
    [SerializeField] private bool freezeXZAxis = false;

    private Camera _mainCam;

    private void Start()
    {
        _mainCam = Camera.main;
    }

    void Update()
    {
        transform.rotation = freezeXZAxis ?
            Quaternion.Euler(0f, _mainCam.transform.localEulerAngles.y, 0f) :
            _mainCam.transform.rotation;
    }
}

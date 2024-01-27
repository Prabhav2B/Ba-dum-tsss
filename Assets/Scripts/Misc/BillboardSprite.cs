using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardSprite : MonoBehaviour
{
    [SerializeField] private bool freezeXZAxis = false;

    public bool IsFocusingOnAct { get; set; }
        
    private Camera _mainCam;
    private float _targetAngle;

    private void Start()
    {
        _mainCam = Camera.main;
    }

    void Update()
    {
        if (!IsFocusingOnAct)
        {
            transform.rotation = freezeXZAxis
                ? Quaternion.Euler(0f, _mainCam.transform.eulerAngles.y, 0f)
                : _mainCam.transform.rotation;
            return;
        }

    }

    public void SetFocusAngle(Vector3 focusPoint)
    {
        IsFocusingOnAct = true;
        
        transform.LookAt(focusPoint);
        transform.rotation = Quaternion.Euler(0f, transform.localEulerAngles.y + 90f, 0f);
    }
}

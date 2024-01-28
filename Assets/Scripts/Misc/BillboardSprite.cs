using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BillboardSprite : MonoBehaviour
{
    [SerializeField] private bool freezeXZAxis = false;

    public bool IsFocusingOnAct { get; set; }
        
    private Camera _mainCam;
    private float _targetAngle;

    private Vector3 _focusPoint;

    private void Start()
    {
        _mainCam = Camera.main;
    }

    void Update()
    {
        if (IsFocusingOnAct) return;

        transform.rotation = freezeXZAxis
            ?  Quaternion.Euler(0f, _mainCam.transform.eulerAngles.y, 0f)
            : _mainCam.transform.rotation;
    }

    public void SetFocusAngle(Vector3 focusPoint)
    {
        IsFocusingOnAct = true;
        _focusPoint = focusPoint;
            
        //transform.LookAt(focusPoint);
        //transform.rotation = Quaternion.Euler(0f, transform.localEulerAngles.y + 90f, 0f);
        
        var target_rot = Quaternion.LookRotation (focusPoint - transform.position);
        
        transform.DORotateQuaternion(target_rot, 0.5f);
    }

    public void JokeFizzle()
    {
        StartCoroutine(WaitAndStare());
    }
    
    IEnumerator WaitAndStare()
    {
        var targetRotation = freezeXZAxis
            ?  Quaternion.Euler(0f, _mainCam.transform.eulerAngles.y, 0f)
            : _mainCam.transform.rotation;
        transform.DORotateQuaternion(targetRotation, 0.5f);
        yield return new WaitForSeconds(5f);
        SetFocusAngle(_focusPoint);
    }
}

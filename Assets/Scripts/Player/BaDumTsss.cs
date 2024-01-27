using System;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class BaDumTsss : MonoBehaviour
{

    [SerializeField] private List<AudioClip> BaClips;
    [SerializeField] private List<AudioClip> DumClips;
    [SerializeField] private List<AudioClip> TssClips;

    [SerializeField] private AudioSource _audioSource;

    private void Start()
    {
        //_audioSource.GetComponentInChildren<AudioSource>();
    }

    public void OnBa(InputValue value)
    {
        //JumpInput(value.isPressed);
        if(value.isPressed) Debug.Log("Ba");
        _audioSource.PlayOneShot(BaClips[0]);
    }
    
    public void OnDum(InputValue value)
    {
        //JumpInput(value.isPressed);
        if(value.isPressed) Debug.Log("Dum");
        _audioSource.PlayOneShot(DumClips[0]);
    }
    
    public void OnTsss(InputValue value)
    {
        //JumpInput(value.isPressed);
        if(value.isPressed) Debug.Log("Tsss");
        _audioSource.PlayOneShot(TssClips[0]);
    }
    
}

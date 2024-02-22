using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using DG.Tweening;

public class BaDumTsss : MonoBehaviour
{
    [SerializeField] private AudioClip startAudio;
    
    [SerializeField] private float popAmount = 1.5f;
    [SerializeField] private float popTime = 0.1f;
    [SerializeField] private Color popColor = Color.white;
    
    [Space (10)]
    [SerializeField] private SpriteRenderer drumSpriteBa;
    [SerializeField] private SpriteRenderer drumSpriteTss;
    [SerializeField] private Animator ba;
    [SerializeField] private Animator dum;
    [SerializeField] private Animator tss;

    [Space (10)]
    [SerializeField] private List<AudioClip> BaClips;
    [SerializeField] private List<AudioClip> DumClips;
    [SerializeField] private List<AudioClip> TssClips;

    private AudioSource _audioSource;
    
    private Transform _transformBa;
    private Transform _transformTss;
    
    private Vector3 _originalScaleBa;
    private Vector3 _originalScaleTss;
    
    private Color _originalColorBa;
    private Color _originalColorTss;

    private JokeDeliveryManager _jokeDeliveryManager;

    private bool _ba = false;
    private bool _dum = false;


    private void Awake()
    {
        _audioSource = GetComponentInChildren<AudioSource>();
        _jokeDeliveryManager = FindObjectOfType<JokeDeliveryManager>();
        
        _transformBa = drumSpriteBa.transform;
        _transformTss = drumSpriteTss.transform;

        _originalScaleBa = _transformBa.localScale;
        _originalScaleTss = _transformTss.localScale;

        _originalColorBa = drumSpriteBa.color;
        _originalColorTss = drumSpriteTss.color;
    }  
    #region BaDumTss
    
    public void OnBa(InputValue value)
    {
        if(!value.isPressed) return;

        _ba = true;
        _dum = false;
        
        _audioSource.PlayOneShot(BaClips[0]);
        drumSpriteBa.DOColor(popColor, popTime);
        _transformBa.DOScale(_originalScaleBa * popAmount, popTime).OnComplete(ResetBa);
        ba.Play("Play");
    }

    private void ResetBa()
    {
        _transformBa.DOScale(_originalScaleBa, popTime);
        drumSpriteBa.DOColor(_originalColorBa, popTime);
    }

    public void OnDum(InputValue value)
    {
        if(!value.isPressed) return;
            
        _dum = true;
        
        _audioSource.PlayOneShot(DumClips[0]);
        drumSpriteBa.DOColor(popColor, popTime);
        _transformBa.DOScale(_originalScaleBa * popAmount, popTime).OnComplete(ResetDum);
        dum.Play("Play");
    }
    
    private void ResetDum()
    {
        _transformBa.DOScale(_originalScaleBa, popTime);
        drumSpriteBa.DOColor(_originalColorBa, popTime);
    }
    
    public void OnTsss(InputValue value)
    {
        if(!value.isPressed) return;

        if (_ba && _dum)
            _jokeDeliveryManager.Tss(); //MAKE THIS INTO AN EVENT
        
        _audioSource.PlayOneShot(TssClips[0]);
        drumSpriteTss.DOColor(popColor, popTime);
        _transformTss.DOScale(_originalScaleTss * popAmount, popTime).OnComplete(ResetTss);
        tss.Play("Play");
    }
    
    private void ResetTss()
    {
        drumSpriteTss.DOColor(_originalColorTss, popTime);
        _transformTss.DOScale(_originalScaleTss, popTime);
    }
    
    #endregion

    
}

using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using DG.Tweening;

public class BaDumTsss : MonoBehaviour
{

    [SerializeField] private float popAmount = 1.5f;
    [SerializeField] private float popTime = 0.1f;
    
    [SerializeField] private SpriteRenderer drumSpriteBa;
    [SerializeField] private SpriteRenderer drumSpriteDum;
    [SerializeField] private SpriteRenderer drumSpriteTss;

    [SerializeField] private List<AudioClip> BaClips;
    [SerializeField] private List<AudioClip> DumClips;
    [SerializeField] private List<AudioClip> TssClips;

    private AudioSource _audioSource;
    
    private Transform transformBa;
    private Transform transformDum;
    private Transform transformTss;
    
    private Vector3 originalScaleBa;
    private Vector3 originalScaleDum;
    private Vector3 originalScaleTss;

    private void Start()
    {
        _audioSource = GetComponentInChildren<AudioSource>();
        
        transformBa = drumSpriteBa.transform;
        transformDum = drumSpriteDum.transform;
        transformTss = drumSpriteTss.transform;

        originalScaleBa = transformBa.localScale;
        originalScaleDum = transformDum.localScale;
        originalScaleTss = transformTss.localScale;
    }

    public void OnBa(InputValue value)
    {
        if(!value.isPressed) return;
        
        _audioSource.PlayOneShot(BaClips[0]);
        transformBa.DOScale(originalScaleBa * popAmount, popTime).OnComplete(ResetBaScale);
    }

    private void ResetBaScale()
    {
        transformBa.DOScale(originalScaleBa, popTime);
    }

    public void OnDum(InputValue value)
    {
        if(!value.isPressed) return;
            
        _audioSource.PlayOneShot(DumClips[0]);
        transformDum.DOScale(originalScaleDum * popAmount, popTime).OnComplete(ResetDumScale);
    }
    
    private void ResetDumScale()
    {
        transformDum.DOScale(originalScaleDum, popTime);
    }
    
    public void OnTsss(InputValue value)
    {
        if(!value.isPressed) return;
        
        _audioSource.PlayOneShot(TssClips[0]);
        transformTss.DOScale(originalScaleTss * popAmount, popTime).OnComplete(ResetTssScale);
    }
    
    private void ResetTssScale()
    {
        transformTss.DOScale(originalScaleTss, popTime);
    }
    
}

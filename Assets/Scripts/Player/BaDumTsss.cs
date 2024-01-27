using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using DG.Tweening;

public class BaDumTsss : MonoBehaviour
{

    [SerializeField] private float popAmount = 1.5f;
    [SerializeField] private float popTime = 0.1f;
    [SerializeField] private Color popColor = Color.white;
    
    [Space (10)]
    [SerializeField] private SpriteRenderer drumSpriteBa;
    [SerializeField] private SpriteRenderer drumSpriteDum;
    [SerializeField] private SpriteRenderer drumSpriteTss;
    
    [Space (10)]
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
    
    private Color originalColorBa;
    private Color originalColorDum;
    private Color originalColorTss;

    private void Start()
    {
        _audioSource = GetComponentInChildren<AudioSource>();
        
        transformBa = drumSpriteBa.transform;
        transformDum = drumSpriteDum.transform;
        transformTss = drumSpriteTss.transform;

        originalScaleBa = transformBa.localScale;
        originalScaleDum = transformDum.localScale;
        originalScaleTss = transformTss.localScale;

        originalColorBa = drumSpriteBa.color;
        originalColorDum = drumSpriteDum.color;
        originalColorTss = drumSpriteTss.color;
        
    }

    public void OnBa(InputValue value)
    {
        if(!value.isPressed) return;
        
        _audioSource.PlayOneShot(BaClips[0]);
        drumSpriteBa.DOColor(popColor, popTime);
        transformBa.DOScale(originalScaleBa * popAmount, popTime).OnComplete(ResetBa);
    }

    private void ResetBa()
    {
        transformBa.DOScale(originalScaleBa, popTime);
        drumSpriteBa.DOColor(originalColorBa, popTime);
    }

    public void OnDum(InputValue value)
    {
        if(!value.isPressed) return;
            
        _audioSource.PlayOneShot(DumClips[0]);
        drumSpriteDum.DOColor(popColor, popTime);
        transformDum.DOScale(originalScaleDum * popAmount, popTime).OnComplete(ResetDum);
    }
    
    private void ResetDum()
    {
        drumSpriteDum.DOColor(originalColorDum, popTime);
        transformDum.DOScale(originalScaleDum, popTime);
    }
    
    public void OnTsss(InputValue value)
    {
        if(!value.isPressed) return;
        
        _audioSource.PlayOneShot(TssClips[0]);
        drumSpriteTss.DOColor(popColor, popTime);
        transformTss.DOScale(originalScaleTss * popAmount, popTime).OnComplete(ResetTss);
    }
    
    private void ResetTss()
    {
        drumSpriteTss.DOColor(originalColorTss, popTime);
        transformTss.DOScale(originalScaleTss, popTime);
    }
    
}

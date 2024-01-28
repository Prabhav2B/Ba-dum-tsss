using System;
using System.Collections;
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
    private bool _waiting = false;

    private Queue<AudioClip> _audioQueue;

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

        _audioQueue = new Queue<AudioClip>();

    }

    private void OnEnable()
    {
        _jokeDeliveryManager.OnJokeQueue += PlayHandlerJokeQueueLine;
        _jokeDeliveryManager.OnJokeHit += PlayHandlerSuccessLine;
        _jokeDeliveryManager.OnFizzle += PlayHandlerFailureLine;
    }
    
    private void OnDisable()
    {
        _jokeDeliveryManager.OnJokeQueue -= PlayHandlerJokeQueueLine;
        _jokeDeliveryManager.OnJokeHit -= PlayHandlerSuccessLine;
        _jokeDeliveryManager.OnFizzle -= PlayHandlerFailureLine;
    }

    private void PlayHandlerJokeQueueLine(AudioClip handlerJokeQueueLine)
    {
        _audioQueue.Enqueue(handlerJokeQueueLine);
    }
    
    private void PlayHandlerSuccessLine(AudioClip handlerJokeHit)
    {
        _audioQueue.Enqueue(handlerJokeHit);
    }
    
    public void PlayHandlerEnteringJokeZone(AudioClip handlerJokeZone)
    {
        _audioSource.PlayOneShot(handlerJokeZone);
    }
    
    private void PlayHandlerFailureLine(AudioClip handlerJokeFail)
    {
        _audioQueue.Enqueue(handlerJokeFail);
    }

    private void Update()
    {
        if(_audioQueue.Count == 0 || _audioSource.isPlaying) return;
        if(_waiting) return;
        _waiting = true;
        StartCoroutine(WaitForABit());
    }

    private IEnumerator WaitForABit()
    {
        yield return new WaitForSeconds(3f);
        _audioSource.clip = _audioQueue.Dequeue();
        _audioSource.Play();
        _waiting = false;
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
        _transformBa.DOScale(_originalScaleBa * popAmount, popTime).OnComplete(ResetBa);
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
    }
    
    private void ResetTss()
    {
        drumSpriteTss.DOColor(_originalColorTss, popTime);
        _transformTss.DOScale(_originalScaleTss, popTime);
    }
    
    #endregion
    
}

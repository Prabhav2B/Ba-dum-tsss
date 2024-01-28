using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Comedian : MonoBehaviour
{
    [SerializeField] private float awkwardSilenceTime = 5f;
    
    private AudioSource _comedianAudioSource;
    private bool _isPlayingJoke = false;

    private ComedianCircle _myComedianCircle;
    [Space(5)]
    [SerializeField] MaleComedian maleSprites;
    [SerializeField] FemaleComedian femaleSprites;
    Sprite idle;
    Sprite joking;
    Sprite awkward;
    Sprite laugh;
    Sprite dead;
    SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        RandomizeSex();
    }
    private void Start()
    {
        _comedianAudioSource = GetComponentInChildren<AudioSource>();
        _myComedianCircle = GetComponentInParent<ComedianCircle>();
        Idle();
    }
    void RandomizeSex()
    {
        int randomizeSex = UnityEngine.Random.Range(0, 2);
        if (randomizeSex == 0)
        {
            idle = femaleSprites.idle;
            joking = femaleSprites.joking;
            awkward = femaleSprites.awkward;
            laugh = femaleSprites.laugh;
            dead = femaleSprites.dead;
        }
        else
        {
            idle = maleSprites.idle;
            joking = maleSprites.joking;
            awkward = maleSprites.awkward;
            laugh = maleSprites.laugh;
            dead = maleSprites.dead;
        }
    }

    private void Update()
    {
        if (!_isPlayingJoke) return;
        if (_comedianAudioSource.isPlaying) return;
        _isPlayingJoke = false;
        _myComedianCircle.FinishedJoke();
        _comedianAudioSource.clip = null;
    }

    public void PlayComedianJoke(AudioClip jokeClip)
    {
        _comedianAudioSource.clip = jokeClip;
        _comedianAudioSource.Play();
        _isPlayingJoke = true;
        Joking();
    }

    public void JokeFizzleInterrupted()
    {
        _comedianAudioSource.Pause();
        StartCoroutine(AwkwardSilence());
    }

    public float GetJokeTime()
    {
        return _comedianAudioSource.time;
    }

    private IEnumerator AwkwardSilence()
    {
        yield return new WaitForSeconds(awkwardSilenceTime);
        _comedianAudioSource.Stop();
        Awkward();
    }
    public void Idle()
    {
        sr.sprite = idle;
    }
    public void Joking()
    {
        sr.sprite = joking;
    }
    public void Laugh()
    {
        sr.sprite = laugh;
    }
    public void Awkward()
    {
        sr.sprite = awkward;
    }
    public void Dead()
    {
        sr.sprite = dead;
    }
}

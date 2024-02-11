using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Comedian : MonoBehaviour
{
    [SerializeField] private float awkwardSilenceTime = 5f;
    [SerializeField] private float laughTime = 3f;
    
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
    Material mat;
    GameManager gm => GameManager.Instance;

    void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        mat = sr.material;
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
        if (gm.IsPaused)
        {
            if(_comedianAudioSource.isPlaying)
                _comedianAudioSource.Pause();
            return;
        }
        else
        {
            if(!_comedianAudioSource.isPlaying)
                _comedianAudioSource.UnPause();
        }
        //if (_comedianAudioSource.isPlaying) return;
        //_isPlayingJoke = false;
        //_myComedianCircle.FinishedJoke();
        //_comedianAudioSource.clip = null;
    }

    public void PlayComedianJoke(AudioClip jokeClip)
    {
        _comedianAudioSource.clip = jokeClip;
        _comedianAudioSource.Play();
        _isPlayingJoke = true;
        Joking();
        AudioManager.Instance.JokeOngoing();
    }

    public void OnJokeFail()
    {
        _comedianAudioSource.Stop();
        StartCoroutine(AwkwardSilence());
    }
    
    public void OnJokeSuccess()
    {
        _comedianAudioSource.Stop();
        StartCoroutine(WaitTillDie());
    }

    public float GetJokeTime()
    {
        return _comedianAudioSource.time;
    }

    private IEnumerator AwkwardSilence()
    {
        Awkward();
        foreach (var audience in _myComedianCircle.AudienceMembers)
        {
            audience.Awkward();
        }
        yield return new WaitForSeconds(awkwardSilenceTime);
        _comedianAudioSource.Stop();
        
        Idle();
        foreach (var audience in _myComedianCircle.AudienceMembers)
        {
            audience.Idle();
        }
        AudioManager.Instance.JokeOff();
    }
    
    private IEnumerator WaitTillDie()
    {
        Laugh();
        foreach (var audience in _myComedianCircle.AudienceMembers)
        {
            audience.Laugh();
        }
        yield return new WaitForSeconds(laughTime);
        Dead();
        foreach (var audience in _myComedianCircle.AudienceMembers)
        {
            audience.Dead();
        }

        var deathfx = transform.parent.GetComponentInChildren<ParticleSystem>(true);
        deathfx.gameObject.SetActive(true);
        //deathfx.Play();
        
        _isPlayingJoke = false;
        _comedianAudioSource.clip = null;
        _myComedianCircle.Dissolve();
        AudioManager.Instance.JokeOff();
    }
    public void Idle()
    {
        sr.sprite = idle;
        mat.SetTexture("_EmissionMap", idle.texture);
    }
    public void Joking()
    {
        sr.sprite = joking;
        mat.SetTexture("_EmissionMap", joking.texture);
    }
    public void Laugh()
    {
        sr.sprite = laugh;
        mat.SetTexture("_EmissionMap", laugh.texture);
    }
    public void Awkward()
    {
        sr.sprite = awkward;
        mat.SetTexture("_EmissionMap", awkward.texture);
    }
    public void Dead()
    {
        sr.sprite = dead;
        mat.SetTexture("_EmissionMap", dead.texture);
    }

    
}

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

    private void Start()
    {
        _comedianAudioSource = GetComponentInChildren<AudioSource>();
        _myComedianCircle = GetComponentInParent<ComedianCircle>();
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
    }
}

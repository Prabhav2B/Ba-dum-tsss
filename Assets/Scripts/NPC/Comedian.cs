using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Comedian : MonoBehaviour
{
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

    public float GetJokeTime()
    {
        return _comedianAudioSource.time;
    }
}

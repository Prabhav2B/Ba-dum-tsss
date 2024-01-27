using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Comedian : MonoBehaviour
{
    private AudioSource _comedianAudioSource;

    private void Start()
    {
        _comedianAudioSource = GetComponentInChildren<AudioSource>();
    }
    
    public void PlayComedianOneShot(AudioClip jokeClip)
    {
        _comedianAudioSource.PlayOneShot(jokeClip);
    }
}

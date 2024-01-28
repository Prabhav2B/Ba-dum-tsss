using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    AudioSource audioSource;
    [SerializeField] float regularVolume;
    [SerializeField] float jokeVolume;
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(this.gameObject);
        JokeOff();
    }

    public void JokeOngoing()
    {
        audioSource.volume = jokeVolume;
    }
    public void JokeOff()
    {
        audioSource.volume = regularVolume;
    }
}

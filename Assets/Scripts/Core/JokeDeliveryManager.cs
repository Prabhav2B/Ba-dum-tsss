using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.SocialPlatforms;

public class JokeDeliveryManager : MonoBehaviour
{
    
    [Range(0f, 2f)]
    [SerializeField] private float jokeHitTolerance = 1f;
    [SerializeField] private AudioClip cricketChirp;
    
    [Space(10)]
    [SerializeField] private List<JokeAndPunchline> jokesAndPunchLines;
    
    [Space(5)]
    [SerializeField] private List<CustomDictionary> handlerLocationLines;
    
    [Space(10)]
    [SerializeField] private List<AudioClip> handlerSuccessLines;
    
    [Space(10)]
    [SerializeField] private List<AudioClip> handlerFailureLines;
    
    
    private List<ComedianCircle> _comedianCircles;

    public delegate void FizzleEvent(AudioClip handlerSuccessAudio); 
    public FizzleEvent OnFizzle;
    
    public delegate void JokeHitEvent(AudioClip handlerFailureAudio); 
    public JokeHitEvent OnJokeHit;
    
    public delegate void JokeQueueEvent(AudioClip handlerAudio); 
    public JokeQueueEvent OnJokeQueue;
    
    public bool PlayerInComedyCircle { get; set; }
    public ComedianCircle CurrentComedyCircle { get; set; }

    private ComedianCircle ComedyCirclePlayingJoke { get; set; }
    
    // Start is called before the first frame update
    void Start()
    {
        _comedianCircles = FindObjectsOfType<ComedianCircle>().ToList();
    }

    public void Tss()
    {
        if (!PlayerInComedyCircle) return;
        if (ComedyCirclePlayingJoke == null) return;
        if (CurrentComedyCircle != ComedyCirclePlayingJoke) return;

        var punchLineTime = CurrentComedyCircle.CurrentJoke.PunchlineTimeStampInSeconds;
        var jokeTime = ComedyCirclePlayingJoke.GetJokeTime();

        if (jokeTime - punchLineTime > 0.0f && jokeTime - punchLineTime < jokeHitTolerance)
        {
            Debug.Log("Hit");
            OnJokeHit?.Invoke(handlerSuccessLines[Random.Range(0, handlerSuccessLines.Count)]);
        }
        else
        {
            Debug.Log("Miss");
            OnFizzle?.Invoke(handlerFailureLines[Random.Range(0, handlerFailureLines.Count)]);
        }

    }

    public bool TryGetJoke(ComedianCircle circle)
    {
        if (ComedyCirclePlayingJoke != null) return false;

        ComedyCirclePlayingJoke = circle;
        ComedyCirclePlayingJoke.CurrentJoke = jokesAndPunchLines[Random.Range(0, jokesAndPunchLines.Count)];
        
        var locationKey = (int)ComedyCirclePlayingJoke.ComedianCircleLocation;
        var currentLocationLines = this.handlerLocationLines[locationKey];
        var locationLine = currentLocationLines.AudioClips[Random.Range(0, currentLocationLines.AudioClips.Count)];
        
        OnJokeQueue?.Invoke(locationLine);
        ComedyCirclePlayingJoke.QueueJoke();
        return true;
    }

    public void JokeFinishedPlaying()
    {
        ComedyCirclePlayingJoke = null;
    }
}

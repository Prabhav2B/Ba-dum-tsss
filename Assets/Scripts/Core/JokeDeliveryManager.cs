using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.SocialPlatforms;
using Random = UnityEngine.Random;

public class JokeDeliveryManager : MonoBehaviour
{

    [SerializeField] private int missesForLose = 4;
    [SerializeField] private int totalTargets = 15;
    
    [Space(10)]
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

    public ComedianCircle ComedyCirclePlayingJoke { get; set; }
    
    public bool PreventHitSpam { get; set; }

    private GameManager _gameManager;
    
    private int _misses;
    private int _targetsEliminated;
    GameManager gm => GameManager.Instance;
    [SerializeField]AgentManager agentManager;

    private void Awake()
    {
        _gameManager = FindObjectOfType<GameManager>();
    }

    void Start()
    {
        _comedianCircles = FindObjectsOfType<ComedianCircle>().ToList();
    }

    public void Tss()
    {
        if (!PlayerInComedyCircle) return;
        if (ComedyCirclePlayingJoke == null) return;
        if (CurrentComedyCircle != ComedyCirclePlayingJoke) return;
        if (gm.IsPaused) return;

        var punchLineTime = CurrentComedyCircle.CurrentJoke.PunchlineTimeStampInSeconds;
        var jokeTime = ComedyCirclePlayingJoke.GetJokeTime();

        if (jokeTime - punchLineTime > 0.0f && jokeTime - punchLineTime < jokeHitTolerance)
        {
            if(PreventHitSpam) return;
            
            PreventHitSpam = true;
            OnJokeHit?.Invoke(handlerSuccessLines[Random.Range(0, handlerSuccessLines.Count)]);
            _targetsEliminated++;
            if (_targetsEliminated == totalTargets)
            {
                _gameManager.Win();
            }
        }
        else
        {
            OnFizzle?.Invoke(handlerFailureLines[Random.Range(0, handlerFailureLines.Count)]);
            _misses++;
            if (_misses == missesForLose)
            {
                _gameManager.Lose();
            }
        }

    }

    public bool TryGetJoke(ComedianCircle circle)
    {
        if (ComedyCirclePlayingJoke != null) return false;

        ComedyCirclePlayingJoke = circle;
        ComedyCirclePlayingJoke.CurrentJoke = jokesAndPunchLines[Random.Range(0, jokesAndPunchLines.Count)];
        
        var locationKey = (int)ComedyCirclePlayingJoke.ComedianCircleLocation;
        //var currentLocationLines = this.handlerLocationLines[locationKey];
        //var locationLine = currentLocationLines.AudioClips[Random.Range(0, currentLocationLines.AudioClips.Count)];

        agentManager.PlayLocationLine(locationKey);
        //OnJokeQueue?.Invoke(locationLine);
        //QueueJoke();
        return true;
    }

    public void JokeFinishedPlaying()
    {
        ComedyCirclePlayingJoke = null;
    }
}

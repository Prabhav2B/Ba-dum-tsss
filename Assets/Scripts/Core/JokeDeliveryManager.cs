using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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
    [SerializeField] private float pauseBeforeFindingComedianCircle = 2f;
    [SerializeField] private AudioClip cricketChirp;
    
    [Space(10)]
    [SerializeField] private List<JokeAndPunchline> jokesAndPunchLines;
    
    [Space(5)]
    [SerializeField] private List<CustomDictionary> handlerLocationLines;
    
    //[Space(10)]
    //[SerializeField] private List<AudioClip> handlerSuccessLines;
    
    //[Space(10)]
    //[SerializeField] private List<AudioClip> handlerFailureLines;
    
    
    private List<ComedianCircle> _comedianCircles;

    public delegate void FizzleEvent(/*AudioClip handlerSuccessAudio*/); 
    public FizzleEvent OnJokeFail;
    
    public delegate void JokeHitEvent(/*AudioClip handlerFailureAudio*/); 
    public JokeHitEvent OnJokeSuccess;
    
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
        if (gm.IsPaused) return;
        if (!PlayerInComedyCircle) return;
        if (ComedyCirclePlayingJoke == null) return;
        if (CurrentComedyCircle != ComedyCirclePlayingJoke) return;

        if (!ComedyCirclePlayingJoke.JokeDelivered)
        {
            JokeFail();
            return;
        } 
        
        if (ComedyCirclePlayingJoke.JokeSuccessPeriod < jokeHitTolerance)
        {
            if(PreventHitSpam) return;
            
            PreventHitSpam = true;
            OnJokeSuccess?.Invoke();
            _targetsEliminated++;
            if (_targetsEliminated == totalTargets)
            {
                _gameManager.Win();
            }
        }
        else
        {
            JokeFail();
        }
    }
    private void Update()
    {
        if (gm.IsPaused) return;
        if (!PlayerInComedyCircle) return;
        if (ComedyCirclePlayingJoke == null) return;
        if (CurrentComedyCircle != ComedyCirclePlayingJoke) return;

        if (!ComedyCirclePlayingJoke.JokeDelivered) return;
        if (ComedyCirclePlayingJoke.JokeSuccessPeriod < jokeHitTolerance) return;
        JokeFail();
    }
    public void JokeSuccess()
    {
        OnJokeSuccess?.Invoke();
        _targetsEliminated++;
        if (_targetsEliminated == totalTargets)
            _gameManager.Win();
        else
            LookForComedians();
    }
    public void JokeFail()
    {
        ComedyCirclePlayingJoke = null;
        OnJokeFail?.Invoke();
        _misses++;
        if (_misses == missesForLose)
            _gameManager.Lose();
        else
            LookForComedians();
    }

    public void LookForComedians()
    {
        StartCoroutine(FindComedianCircle());
    }
    IEnumerator FindComedianCircle()
    {
        if (ComedyCirclePlayingJoke != null) yield break;
        yield return new WaitForSeconds(pauseBeforeFindingComedianCircle);
        ComedyCirclePlayingJoke = _comedianCircles[Random.Range(0, _comedianCircles.Count)];
        ComedyCirclePlayingJoke.InitializeJoke(jokesAndPunchLines[Random.Range(0, jokesAndPunchLines.Count)]);
        agentManager.PlayLocationLine(ComedyCirclePlayingJoke.LocationLines);
    }
    void RepeatLocationLine()
    {
        agentManager.PlayLocationLine(ComedyCirclePlayingJoke.LocationLines);
    }
    //public bool TryGetJoke(ComedianCircle circle)
    //{
    //    if (ComedyCirclePlayingJoke != null) return false;

    //    ComedyCirclePlayingJoke = circle;
    //    ComedyCirclePlayingJoke.InitializeJoke(jokesAndPunchLines[Random.Range(0, jokesAndPunchLines.Count)]);
        
    //    //var locationKey = (int)ComedyCirclePlayingJoke.ComedianCircleLocation;
    //    //var currentLocationLines = this.handlerLocationLines[locationKey];
    //    //var locationLine = currentLocationLines.AudioClips[Random.Range(0, currentLocationLines.AudioClips.Count)];

    //    agentManager.PlayLocationLine(circle.LocationLines);
    //    //OnJokeQueue?.Invoke(locationLine);
    //    //QueueJoke();
    //    return true;
    //}

    //public void JokeFinishedPlaying()
    //{
    //    ComedyCirclePlayingJoke = null;
    //}

}

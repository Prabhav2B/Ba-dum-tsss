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
    [SerializeField] int currentMisses;
    [SerializeField] int currentSuccess;

    [Space(10)]
    [Range(0f, 2f)]
    [SerializeField] private float jokeHitTolerance = 1f;
    [SerializeField] private float pauseBeforeFindingComedianCircle = 2f;
    [SerializeField] private AudioClip cricketChirp;
    
    [Space(10)]
    [SerializeField] private List<JokeAndPunchline> jokesAndPunchLines;
    
    [Space(5)]
    [SerializeField] private List<CustomDictionary> handlerLocationLines;      
    
    private List<ComedianCircle> _comedianCircles;
    
    public bool PlayerInComedyCircle { get; set; }
    public ComedianCircle CurrentComedyCircle { get; set; }

    public ComedianCircle ComedyCirclePlayingJoke { get; set; }
    
    public bool PreventHitSpam { get; set; }
    
    GameManager gm => GameManager.Instance;
    [SerializeField]AgentManager agentManager;
    [SerializeField]JokeLocationUI locationUi;

   
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

        if (!ComedyCirclePlayingJoke.JokeDelivered || ComedyCirclePlayingJoke.JokeSuccessPeriod > jokeHitTolerance)
        {
            JokeFail();
            return;
        }
        if (PreventHitSpam) return;

        PreventHitSpam = true;
        JokeSuccess();
        currentSuccess++;
        if (currentSuccess == totalTargets)
        {
            gm.Win();
        }
    }
    private void Update()
    {
        if (gm.IsPaused) return;
        if (ComedyCirclePlayingJoke == null) return;
        if (CurrentComedyCircle != ComedyCirclePlayingJoke) return;

        if (!ComedyCirclePlayingJoke.JokeDelivered) return;
        if (ComedyCirclePlayingJoke.JokeSuccessPeriod < jokeHitTolerance) return;
        JokeFail();
    }
    private void JokeSuccess()
    {
        locationUi.LocationBlank();
        ComedyCirclePlayingJoke.JokeSuccess();
        agentManager.JokeSuccess();
        RemoveComedyCircleFromList();
        currentSuccess++;
        if (currentSuccess == totalTargets)
        {
            agentManager.PlayWin();
            gm.Win();
        }
    }
    private void JokeFail()
    {
        locationUi.LocationBlank();
        ComedyCirclePlayingJoke.JokeFail();
        agentManager.JokeFail();
        ComedyCirclePlayingJoke = null;
        currentMisses++;
        if (currentMisses == missesForLose)
        { 
            agentManager.PlayLose();
            gm.Lose();
        }
    }
    void RemoveComedyCircleFromList()
    {
        _comedianCircles.Remove(ComedyCirclePlayingJoke);
        ComedyCirclePlayingJoke = null;
    }
    public void LookForComedians()
    {
        if (gm.IsGameOver) return;
        StartCoroutine(FindComedianCircle());
    }
    IEnumerator FindComedianCircle()
    {
        if (ComedyCirclePlayingJoke != null) yield break;
        yield return new WaitForSeconds(pauseBeforeFindingComedianCircle);
        ComedyCirclePlayingJoke = _comedianCircles[Random.Range(0, _comedianCircles.Count)];
        ComedyCirclePlayingJoke.InitializeJoke(jokesAndPunchLines[Random.Range(0, jokesAndPunchLines.Count)]);
        agentManager.PlayLocationLine(ComedyCirclePlayingJoke.LocationLines);
        locationUi.LocationUpdate(ComedyCirclePlayingJoke.LocationInfo);
    }
    void RepeatLocationLine()
    {
        agentManager.PlayLocationLine(ComedyCirclePlayingJoke.LocationLines);
    }
}

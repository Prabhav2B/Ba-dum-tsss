using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class JokeDeliveryManager : MonoBehaviour
{
    [SerializeField] private List<JokeAndPunchline> jokesAndPunchLines;
    [SerializeField] private AudioClip cricketChirp;
    
    [Range(0f, 2f)]
    [SerializeField] private float jokeHitTolerance = 1f;
    
    private List<ComedianCircle> _comedianCircles;
    private JokeAndPunchline _currentJoke;
    
    public bool PlayerInComedyCircle { get; set; }
    public ComedianCircle CurrentComedyCircle { get; set; }
    
    public ComedianCircle ComedyCirclePlayingJoke { get; set; }
    
    // Start is called before the first frame update
    void Start()
    {
        _comedianCircles = FindObjectsOfType<ComedianCircle>().ToList();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Tss()
    {
        if (!PlayerInComedyCircle) return;
        if (ComedyCirclePlayingJoke == null) return;
        if (CurrentComedyCircle != ComedyCirclePlayingJoke) return;

        var punchLineTime = _currentJoke.PunchlineTimeStampInSeconds;
        var jokeTime = ComedyCirclePlayingJoke.GetJokeTime();

        if (jokeTime - punchLineTime > 0.0f && jokeTime - punchLineTime < jokeHitTolerance)
        {
            Debug.Log("Hit");
        }
        else
            Debug.Log("Miss");

    }

    public bool TryGetJoke(ComedianCircle circle)
    {
        if (ComedyCirclePlayingJoke != null) return false;

        ComedyCirclePlayingJoke = circle;
        _currentJoke = jokesAndPunchLines[Random.Range(0, jokesAndPunchLines.Count)];
        ComedyCirclePlayingJoke.PlayJoke(_currentJoke.Joke);
        return true;
    }

    public void JokeFinishedPlaying()
    {
        ComedyCirclePlayingJoke = null;
        _currentJoke = null;
    }
}

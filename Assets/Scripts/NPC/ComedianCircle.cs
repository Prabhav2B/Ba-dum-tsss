using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class ComedianCircle : MonoBehaviour
{

    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float minWaitTime = 1.0f;
    [SerializeField] private float maxWaitTime = 20.0f;

    [SerializeField] private AudioClip chirpSound;
    
    [Space(10)]
    [SerializeField] private List<AudioClip> crowdFailureLines;
    //[SerializeField] private List<AudioClip> EnteringJokeZoneLines;
    
    [Space(10)]
    [SerializeField] private List<AudioClip> laughTracks;
    
    [Space(10)]
    [SerializeField] private LocationScriptableObject location;
    public LocationScriptableObject LocationInfo=> location;
    public AudioClip[] LocationLines => location.LocationLines;
    
    public JokeAndPunchline CurrentJoke { get; set; }
    //public ComedianLocation ComedianCircleLocation => comedianLocation;
    public List<AudienceMember> AudienceMembers => _audienceMembers;

    private List<AudienceMember> _audienceMembers;
    private Comedian _comedian;

    private Vector3 _comedianFocus;
    private Vector3 _audienceFocus;

    private JokeDeliveryManager _jokeDeliveryManager;
    private AudioSource _comedyCircleAudioSource;

    private bool _jokeWaiting;
    private AgentManager _agentManager;

    float jokePositionInTime=>_comedian.GetJokeTime();
    float punchLineTime;
    public bool JokePlaying => jokePlaying;
    bool jokePlaying;
    public bool JokeDelivered =>jokePositionInTime - punchLineTime > 0.0f;
    public float JokeSuccessPeriod => jokePositionInTime - punchLineTime;


    void Start()
    {
        _jokeDeliveryManager = FindObjectOfType<JokeDeliveryManager>();
        _agentManager = FindObjectOfType<AgentManager>();
        _comedyCircleAudioSource = GetComponent<AudioSource>();
        
        _audienceMembers = GetComponentsInChildren<AudienceMember>().ToList();
        _comedian = GetComponentInChildren<Comedian>();

        var audienceCentrePosition = Vector3.zero;
        
        foreach (var member in _audienceMembers)
        {
            var memBillboard = member.GetComponentInChildren<BillboardSprite>();
            memBillboard.SetFocusAngle(_comedian.transform.position);
            audienceCentrePosition += member.transform.position;
        }
        
        audienceCentrePosition /= 4.0f;
        var comBillboard = _comedian.GetComponentInChildren<BillboardSprite>();
        comBillboard.SetFocusAngle(audienceCentrePosition);
    }
    public void InitializeJoke(JokeAndPunchline joke)
    { 
        CurrentJoke = joke;
    }
    IEnumerator StartJoking()
    {
        if (jokePlaying) yield break;
        jokePlaying = true;
        _comedian.Joking();
        yield return _agentManager.PlayEnteringJokeZone();
        _comedian.PlayComedianJoke(CurrentJoke.Joke);
        punchLineTime = CurrentJoke.PunchlineTimeStampInSeconds;
    } 
   
    public void JokeSuccess()
    {
        jokePlaying = false;
        PlayLaugh(laughTracks[Random.Range(0, laughTracks.Count)]);
        _comedian.OnJokeSuccess();
    }
    public void JokeFail()
    {
        jokePlaying = false;
        _comedian.OnJokeFail();
        _comedyCircleAudioSource.PlayOneShot(chirpSound);
        var awkwardClip = crowdFailureLines[Random.Range(0, crowdFailureLines.Count)];
        _comedyCircleAudioSource.PlayOneShot(awkwardClip);
        var billboards = GetComponentsInChildren<BillboardSprite>();
        foreach (var billboard in billboards)
        {
            billboard.OnJokeFail();
        }
    }   

    private void PlayLaugh(AudioClip laughTrack)
    {
        _comedyCircleAudioSource.PlayOneShot(laughTrack);
    }
    public void Dissolve()
    {
        StopAllCoroutines();
        enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((layerMask.value & (1 << other.transform.gameObject.layer)) <= 0) return;
        _jokeDeliveryManager.PlayerInComedyCircle = true;
        _jokeDeliveryManager.CurrentComedyCircle = this;

        _jokeDeliveryManager.PreventHitSpam = false;

        if (_jokeDeliveryManager.ComedyCirclePlayingJoke == this)
            StartCoroutine(StartJoking());
    }

    private void OnTriggerExit(Collider other)
    {
        if ((layerMask.value & (1 << other.transform.gameObject.layer)) <= 0) return;
        _jokeDeliveryManager.PlayerInComedyCircle = false;
        _jokeDeliveryManager.CurrentComedyCircle = null;
    }
}

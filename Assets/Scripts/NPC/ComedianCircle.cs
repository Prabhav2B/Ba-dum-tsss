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
    [SerializeField] private List<AudioClip> EnteringJokeZoneLines;
    
    [Space(10)]
    [SerializeField] private List<AudioClip> laughTracks;
    
    [Space(10)]
    //[SerializeField] private ComedianLocation comedianLocation;
    [SerializeField] private LocationScriptableObject location;
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
    
    // Start is called before the first frame update
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

        StartCoroutine(WaitForRandomTime());
    }

    private void OnDisable()
    {
        _jokeDeliveryManager.OnJokeHit -= KillAllNPCS;
        _jokeDeliveryManager.OnFizzle -= NPCDeathGlare;
    }
    

    public void PlayerInComedyCircle()
    {
        _jokeDeliveryManager.OnJokeHit += KillAllNPCS;
        _jokeDeliveryManager.OnFizzle += NPCDeathGlare;
    }
    
    public void PlayerLeftComedyCircle()
    {
        _jokeDeliveryManager.OnJokeHit -= KillAllNPCS;
        _jokeDeliveryManager.OnFizzle -= NPCDeathGlare;
    }

    private void NPCDeathGlare(AudioClip handlersuccessaudio)
    {
        PlayChirping();
        PlayAwkwardAudience(crowdFailureLines[Random.Range(0, crowdFailureLines.Count)]);
        _comedian.JokeFizzleInterrupted();
        var billboards = GetComponentsInChildren<BillboardSprite>();
        foreach (var billboard in billboards)
        {
            billboard.JokeFizzle();
        }
    }
    
    private void KillAllNPCS(AudioClip handlerfailaudio)
    {
        PlayLaugh(laughTracks[Random.Range(0, laughTracks.Count)]);
        _comedian.JokeHitInterrupted();
    }


    private void PlayChirping()
    {
        _comedyCircleAudioSource.PlayOneShot(chirpSound);
    }

    private void PlayAwkwardAudience(AudioClip awkwardAudience)
    {
        _comedyCircleAudioSource.PlayOneShot(awkwardAudience);
    }
    
    private void PlayLaugh(AudioClip laughTrack)
    {
        _comedyCircleAudioSource.PlayOneShot(laughTrack);
    }

    private void PlayJoke()
    {
        _comedian.PlayComedianJoke(CurrentJoke.Joke);
    }

    IEnumerator WaitForRandomTime()
    {
        yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));

        if (!_jokeDeliveryManager.TryGetJoke(this))
        {
            _jokeWaiting = false;
            StartCoroutine(WaitForRandomTime());
        }
    }
    
    public void QueueJoke()
    {
        if(_jokeWaiting) return;
        _jokeWaiting = true;
        StartCoroutine(JokeWaitingInQueue());
    }
    
    IEnumerator JokeWaitingInQueue()
    {
        yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));
        PlayJoke();
    }

    public void FinishedJoke()
    {
        _jokeDeliveryManager.JokeFinishedPlaying();
        StartCoroutine(WaitForRandomTime());
    }

    public float GetJokeTime()
    {
        return _comedian.GetJokeTime();
    }
    
    
    private void OnTriggerEnter(Collider other)
    {
        if ((layerMask.value & (1 << other.transform.gameObject.layer)) <= 0) return;
        _jokeDeliveryManager.PlayerInComedyCircle = true;
        _jokeDeliveryManager.CurrentComedyCircle = this;

        _jokeDeliveryManager.PreventHitSpam = false;

        if (_jokeDeliveryManager.ComedyCirclePlayingJoke == this)
        {
            _agentManager.PlayEnteringJokeZone();
            QueueJoke();
        }

        PlayerInComedyCircle();
    }
    
    private void OnTriggerExit(Collider other)
    {
        if ((layerMask.value & (1 << other.transform.gameObject.layer)) <= 0) return;
        _jokeDeliveryManager.PlayerInComedyCircle = false;
        _jokeDeliveryManager.CurrentComedyCircle = null;
        PlayerLeftComedyCircle();
    }

    public enum ComedianLocation
    {
        WaterCooler = 0,
        MonetPainting = 1,
        WomenRestroom = 2,
        Tables = 3,
        BarCounter = 4,
        Stage = 5,
        Kitchen = 6,
        SmallChandelier = 7, 
        BiiigChandelier = 8,
        BackOfTheRoom = 9,
        Pillar = 10,
        Tutorial = 99
    }

    public void Dissolve()
    {
        _jokeDeliveryManager.JokeFinishedPlaying();
        StopAllCoroutines();
        enabled = false;
    }
}

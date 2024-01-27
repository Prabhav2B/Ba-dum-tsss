using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ComedianCircle : MonoBehaviour
{

    [SerializeField] public LayerMask layerMask;
    [SerializeField] public float minWaitTime = 1.0f;
    [SerializeField] public float maxWaitTime = 20.0f;
    
    private List<AudienceMember> audienceMembers;
    private Comedian _comedian;

    private Vector3 _comedianFocus;
    private Vector3 _audienceFocus;

    private JokeDeliveryManager _jokeDeliveryManager;
    private AudioSource _comedyCircleAudioSource;
    
    
    // Start is called before the first frame update
    void Start()
    {
        _jokeDeliveryManager = FindObjectOfType<JokeDeliveryManager>();
        _comedyCircleAudioSource = GetComponent<AudioSource>();
        
        audienceMembers = GetComponentsInChildren<AudienceMember>().ToList();
        _comedian = GetComponentInChildren<Comedian>();

        var audienceCentrePosition = Vector3.zero;
        
        foreach (var member in audienceMembers)
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

    private void OnTriggerEnter(Collider other)
    {
        if ((layerMask.value & (1 << other.transform.gameObject.layer)) <= 0) return;
        _jokeDeliveryManager.PlayerInComedyCircle = true;
        _jokeDeliveryManager.CurrentComedyCircle = this;
    }
    
    private void OnTriggerExit(Collider other)
    {
        if ((layerMask.value & (1 << other.transform.gameObject.layer)) <= 0) return;
        _jokeDeliveryManager.PlayerInComedyCircle = false;
        _jokeDeliveryManager.CurrentComedyCircle = null;
    }
    
    public void PlayChirping(AudioClip chirpSound)
    {
        _comedyCircleAudioSource.PlayOneShot(chirpSound);
    }

    public void PlayJoke(AudioClip joke)
    {
        _comedian.PlayComedianJoke(joke);
    }

    IEnumerator WaitForRandomTime()
    {
        yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));

        if (!_jokeDeliveryManager.TryGetJoke(this))
        {
            StartCoroutine(WaitForRandomTime());
        }
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

    public void FizzleJoke(AudioClip chirp)
    {
        PlayChirping(chirp);
        var billboards = GetComponentsInChildren<BillboardSprite>();
        foreach (var billboard in billboards)
        {
            billboard.JokeFizzle();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentManager : MonoBehaviour
{
    [SerializeField] private float introDelay;
    [SerializeField] private float pauseBetweenQueues;
    float pauseTimer;
    [SerializeField] private AudioClip introLines;
    [SerializeField] JokeDeliveryManager _jokeDeliveryManager;
    
    [SerializeField] private List<AudioClip> EnteringJokeZoneLines;

    [Space(5)]
    [SerializeField] private List<CustomDictionary> handlerLocationLines;

    [Space(10)]
    [SerializeField] private List<AudioClip> handlerSuccessLines;

    [Space(10)]
    [SerializeField] private List<AudioClip> handlerFailureLines;


    private Queue<AudioClip> _audioQueue;
    private AudioSource _audioSource;
    private bool _waiting = false;
    GameManager gm => GameManager.Instance;


    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioQueue = new Queue<AudioClip>();
        //_audioQueue.Enqueue(introLines);
        Invoke(nameof(PlayIntro), introDelay);
    }
    private void OnEnable()
    {
        //_jokeDeliveryManager.OnJokeQueue += PlayLocationLine;
        _jokeDeliveryManager.OnJokeHit += PlayJokeHit;
        _jokeDeliveryManager.OnFizzle += PlayJokeFizzle;
    }

    private void OnDisable()
    {
        //_jokeDeliveryManager.OnJokeQueue -= PlayLocationLine;
        _jokeDeliveryManager.OnJokeHit -= PlayJokeHit;
        _jokeDeliveryManager.OnFizzle -= PlayJokeFizzle;
    }
    private void Update()
    {
        if (gm.IsPaused)
        {
            if (_audioSource.isPlaying)
                _audioSource.Pause();
            return;
        }

        if (!_audioSource.isPlaying)
            _audioSource.UnPause();
        
        if (_audioQueue.Count == 0 || _audioSource.isPlaying) return;
        if (PausingBetweenQueues()) return;
        _audioSource.clip = _audioQueue.Dequeue();
        _audioSource.Play();        
    }
    bool PausingBetweenQueues()
    {
        pauseTimer += Time.deltaTime;
        if (pauseTimer < pauseBetweenQueues) return true;
        pauseTimer = 0;
        return false;
    }
    void PlayIntro()
    {
        _audioSource.clip = introLines;
        _audioSource.Play();
    }
   
    public void PlayLocationLine(int key)
    {
        var currentLocationLines = handlerLocationLines[key];
        var locationLine = currentLocationLines.AudioClips[Random.Range(0, currentLocationLines.AudioClips.Count)];
        _audioQueue.Enqueue(locationLine);
    }

    private void PlayJokeHit(AudioClip handlerJokeHit)
    {
        StartCoroutine(SuccessDelay(handlerJokeHit));
        //_audioQueue.Enqueue(handlerJokeHit);
    }
    private IEnumerator SuccessDelay(AudioClip handlerJokeHit)
    {
        yield return new WaitForSeconds(3f);
        _audioQueue.Enqueue(handlerJokeHit);
    }
    public void PlayEnteringJokeZone()
    {
        AudioClip handlerJokeZone = EnteringJokeZoneLines[Random.Range(0, EnteringJokeZoneLines.Count)];
        _audioSource.PlayOneShot(handlerJokeZone);
    }

    private void PlayJokeFizzle(AudioClip handlerJokeFail)
    {
        _audioQueue.Enqueue(handlerJokeFail);
    }
    public void PlayWin(AudioClip winAudio)
    {
        _audioQueue.Clear();
        _audioSource.clip = winAudio;
        _audioSource.Play();
    }

    public void PlayLose(AudioClip loseAudio)
    {
        _audioQueue.Clear();
        _audioSource.clip = loseAudio;
        _audioSource.Play();
    }

    //private IEnumerator WaitForABit()
    //{
    //    yield return new WaitForSeconds(3f);
    //    _audioSource.clip = _audioQueue.Dequeue();
    //    _audioSource.Play();
    //    _waiting = false;
    //}
}

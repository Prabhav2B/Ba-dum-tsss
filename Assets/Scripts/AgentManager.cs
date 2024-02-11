using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentManager : MonoBehaviour
{
    [SerializeField] private bool skipIntro;
    [SerializeField] private float introDelay;
    [SerializeField] private float pauseBetweenQueues;
    float pauseTimer;
    [SerializeField] private AudioClip introLines;
    [SerializeField] JokeDeliveryManager _jokeDeliveryManager;
    
    [SerializeField] private List<AudioClip> EnteringJokeZoneLines;

    //[Space(5)]
    //[SerializeField] private List<CustomDictionary> handlerLocationLines;

    [Space(10)]
    [SerializeField] private List<AudioClip> handlerSuccessLines;

    [Space(10)]
    [SerializeField] private List<AudioClip> handlerFailureLines;


    private Queue<AudioClip> _locationAudioQueue;
    private AudioSource _audioSource;
    private bool _waiting = false;
    GameManager gm => GameManager.Instance;

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _locationAudioQueue = new Queue<AudioClip>();
        //_audioQueue.Enqueue(introLines);
        StartCoroutine(PlayIntro());
    }
    private void OnEnable()
    {
        //_jokeDeliveryManager.OnJokeQueue += PlayLocationLine;
        _jokeDeliveryManager.OnJokeSuccess += PlayJokeSuccess;
        _jokeDeliveryManager.OnJokeFail += PlayJokeFail;
    }

    private void OnDisable()
    {
        //_jokeDeliveryManager.OnJokeQueue -= PlayLocationLine;
        _jokeDeliveryManager.OnJokeSuccess -= PlayJokeSuccess;
        _jokeDeliveryManager.OnJokeFail -= PlayJokeFail;
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

        //if(_jokeDeliveryManager.ComedyCirclePlayingJoke.JokePlaying)
        //    _audioSource.Stop();

        //if (_locationAudioQueue.Count == 0 || _audioSource.isPlaying) return;
        //if (PausingBetweenQueues()) return;
        //_audioSource.clip = _locationAudioQueue.Dequeue();
        //_audioSource.Play();
    }
    //bool PausingBetweenQueues()
    //{
    //    pauseTimer += Time.deltaTime;
    //    if (pauseTimer < pauseBetweenQueues) return true;
    //    pauseTimer = 0;
    //    return false;
    //}
    IEnumerator PlayIntro()
    {
        if (!skipIntro)
        {
            yield return new WaitForSeconds(introDelay);
            _audioSource.clip = introLines;
            _audioSource.Play();
            yield return new WaitForSeconds(introLines.length);
        }       
        _jokeDeliveryManager.LookForComedians();
    }
    public void PlayLocationLine(AudioClip[] clips)
    {
        var locationLine = clips[Random.Range(0, clips.Length)];
        _locationAudioQueue.Enqueue(locationLine);        
    }
    public IEnumerator PlayEnteringJokeZone()
    {
        var handlerJokeZone = EnteringJokeZoneLines[Random.Range(0, EnteringJokeZoneLines.Count)];
        _audioSource.PlayOneShot(handlerJokeZone);
        yield return new WaitForSeconds(handlerJokeZone.length - 0.5f);
    }
    private void PlayJokeSuccess()
    {
        var line = handlerSuccessLines[Random.Range(0, handlerSuccessLines.Count)];
        StartCoroutine(SuccessDelay(line));
        //_audioQueue.Enqueue(handlerJokeHit);
    }
    private IEnumerator SuccessDelay(AudioClip handlerJokeHit)
    {
        yield return new WaitForSeconds(3f);
        _audioSource.PlayOneShot(handlerJokeHit);
    }

    private void PlayJokeFail()
    {
        var line = handlerFailureLines[Random.Range(0, handlerFailureLines.Count)];
        _audioSource.PlayOneShot(line);
    }
    public void PlayWin(AudioClip winAudio)
    {
        _locationAudioQueue.Clear();
        _audioSource.clip = winAudio;
        _audioSource.Play();
    }

    public void PlayLose(AudioClip loseAudio)
    {
        _locationAudioQueue.Clear();
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

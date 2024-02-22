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

    [Space(10)]
    [SerializeField] private List<AudioClip> handlerSuccessLines;

    [Space(10)]
    [SerializeField] private List<AudioClip> handlerFailureLines;
    [SerializeField] private AudioClip winAudio;
    [SerializeField] private AudioClip loseAudio;

    private AudioSource _audioSource;
    GameManager gm => GameManager.Instance;

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        StartCoroutine(PlayIntro());
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
    }   
    IEnumerator PlayIntro()
    {
        if (!skipIntro)
        {
            yield return new WaitForSeconds(introDelay);
            PlayAudio(introLines);
            yield return new WaitForSeconds(introLines.length);
        }       
        StartCoroutine(LookingForNewComedians());
    }
    public void PlayLocationLine(AudioClip[] clips)
    {
        var locationLine = clips[Random.Range(0, clips.Length)];
        PlayAudio(locationLine);
    }
    public IEnumerator PlayEnteringJokeZone()
    {
        var handlerJokeZone = EnteringJokeZoneLines[Random.Range(0, EnteringJokeZoneLines.Count)];
        PlayAudio(handlerJokeZone);
        yield return new WaitForSeconds(handlerJokeZone.length - 0.5f);
    }
    public void JokeSuccess()
    {        
        StartCoroutine(SuccessDelay());
    }
    private IEnumerator SuccessDelay()
    {
        yield return new WaitForSeconds(4.5f);
        var line = handlerSuccessLines[Random.Range(0, handlerSuccessLines.Count)];
        PlayAudio(line);
        StartCoroutine(LookingForNewComedians(line.length));
    }

    public void JokeFail()
    {
        StartCoroutine(FailDelay());
    }
    IEnumerator FailDelay()
    {
        yield return new WaitForSeconds(3.5f);
        var line = handlerFailureLines[Random.Range(0, handlerFailureLines.Count)];
        PlayAudio(line);
        StartCoroutine(LookingForNewComedians(line.length));
    }
    IEnumerator LookingForNewComedians(float delay = 0)
    {
        yield return new WaitForSeconds(delay);
        _jokeDeliveryManager.LookForComedians();
    }
    public void PlayWin()
    {
        PlayAudio(winAudio);
    }

    public void PlayLose()
    {
        PlayAudio(loseAudio);
    }
    void PlayAudio(AudioClip dialogue)
    {
        _audioSource.Stop();
        _audioSource.clip = dialogue;
        _audioSource.Play();
    }
}

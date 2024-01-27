
using UnityEngine;

[System.Serializable]
public class JokeAndPunchline
{
    [SerializeField] private AudioClip joke;
    [SerializeField] private float punchlineTimeStampInSeconds;


    public AudioClip Joke => joke;
    public float PunchlineTimeStampInSeconds => punchlineTimeStampInSeconds;
}

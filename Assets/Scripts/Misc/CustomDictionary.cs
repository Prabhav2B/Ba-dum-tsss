using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class CustomDictionary
{
    [SerializeField] private int key;
    [SerializeField] private List<AudioClip> audioClips;

    public int Key => key;
    public List<AudioClip> AudioClips => audioClips;
}

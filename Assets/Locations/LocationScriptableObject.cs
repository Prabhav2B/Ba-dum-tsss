using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Location", menuName = "ScriptableObjects/Location", order = 1)]

public class LocationScriptableObject : ScriptableObject
{
    //[SerializeField] string locationName;
    public AudioClip[] LocationLines => locationLine;
    [SerializeField] AudioClip[] locationLine;
    public Sprite LocationImage => locationImage;
    [SerializeField] Sprite locationImage;
}
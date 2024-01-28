using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{

    [SerializeField] private AudioClip firstAudio;

    private BaDumTsss _player;

    private void Awake()
    {
        _player = FindObjectOfType<BaDumTsss>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

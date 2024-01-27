using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class JokeDeliveryManager : MonoBehaviour
{
    [SerializeField] private List<JokeAndPunchline> jokesAndPunchLines;
    [SerializeField] private AudioClip cricketChirp;

    private List<ComedianCircle> _comedianCircles;
    
    public bool PlayerInComedyCircle { get; set; }
    public ComedianCircle CurrentComedyCircle { get; set; }
    
    // Start is called before the first frame update
    void Start()
    {
        _comedianCircles = FindObjectsOfType<ComedianCircle>().ToList();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Tss()
    {
        if (!PlayerInComedyCircle) return;
        
        
    }
}

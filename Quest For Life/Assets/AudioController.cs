using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField]
    protected AudioSource source;

    [SerializeField]
    protected float AverageTimeBetweenSound;
    protected float TimeBetweenSound;

    protected float timer;

    [HideInInspector]
    public Player p;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

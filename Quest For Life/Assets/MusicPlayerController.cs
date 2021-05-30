using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayerController : MonoBehaviour
{
    Player p;

    [SerializeField]
    AudioSource explorationPlayer;
    [SerializeField]
    AudioSource BattlePlayer;

    double explorationTime;
    double BattleTime;

    [SerializeField]
    [Range(0.0f, 1.0f)]
    float defaultVolume = 0.5f;

    void Awake()
    {
        explorationTime = 0;
        BattleTime = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        explorationPlayer.volume = defaultVolume * AppManager.inst.appdata.MusicVolume;
        BattlePlayer.volume = AppManager.inst.appdata.MusicVolume;
        explorationPlayer.Play();

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void StartDungeonTrack()
    {

    }


    public void TransitionToBattle()
    {
        //explorationTime = explorationPlayer.
        explorationPlayer.Pause();
        BattlePlayer.Play();
    }

    public void TransitionToExploration()
    {
        BattlePlayer.Stop();
        explorationPlayer.UnPause();        
    }

    public void StopMusic()
    {
        BattlePlayer.Stop();
        explorationPlayer.Stop();
    }
}

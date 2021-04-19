using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleStatsmenuManager : MonoBehaviour
{
    [SerializeField]
    HudManager manager;

    Player player;

    [SerializeField]
    Text AttackNr;
    [SerializeField]
    Text DefenceNr;
    [SerializeField]
    Text AccuracyNr;
    [SerializeField]
    Text DodgeNr;
    [SerializeField]
    Text SpeedNr;

    [SerializeField]
    Text Experience;
    [SerializeField]
    Text ExperienceToNextLevel;
    
    void Awake()
    {
        Player.onStatsChange += GetStatsFromPlayer;
        Player.onExperience += GetExperienceFromPlayer;
    }

     // Start is called before the first frame update
    void Start()
    {
        player = manager.player;

         GetStatsFromPlayer();
         GetExperienceFromPlayer();


    }

    void OnEnable()
    {

    }

    void OnDestroy()
    {
        Player.onStatsChange -= GetStatsFromPlayer;
        Player.onExperience -= GetExperienceFromPlayer;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void GetStatsFromPlayer()
    {
        AttackNr.text = player.Power.ToString();
        DefenceNr.text = player.Defence.ToString();
        AccuracyNr.text = player.Accuracy.ToString();
        DodgeNr.text = player.Dodge.ToString();
        SpeedNr.text = player.Speed.ToString();
    }

    void GetExperienceFromPlayer()
    {
        Experience.text = player.Experience.ToString();
        ExperienceToNextLevel.text = player.toNextLevelExp.ToString();
    }
}

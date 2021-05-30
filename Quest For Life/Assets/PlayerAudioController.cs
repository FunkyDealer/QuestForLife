using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioController : MonoBehaviour
{
    [SerializeField]
    AudioSource waterDrinking;
    [SerializeField]
    AudioSource BuySound;
    [SerializeField]
    AudioSource SellSound;

    [SerializeField]
    AudioSource stepsSource;

    [SerializeField]
    AudioSource SpellsSource;

    [SerializeField]
    AudioSource ClimbingSource;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayWaterDrinking()
    {
        waterDrinking.PlayOneShot(waterDrinking.clip, AppManager.inst.appdata.EffectsVolume);
    }

    public void PlayBuySound()
    {
        BuySound.PlayOneShot(BuySound.clip, AppManager.inst.appdata.EffectsVolume);
    }

    public void PlaySellSound()
    {
        SellSound.PlayOneShot(SellSound.clip, AppManager.inst.appdata.EffectsVolume);
    }

    public void PlayStepsSound()
    {
        AudioClip o = AudioDataBase.inst.GetStep();

        stepsSource.PlayOneShot(o, AppManager.inst.appdata.EffectsVolume);
    }

    public void PlayClimbingSound()
    {
        ClimbingSource.PlayOneShot(ClimbingSource.clip, AppManager.inst.appdata.EffectsVolume);
    }


    public void PlaySpellSound(Global.Type t)
    {
        switch (t)
        {
            case Global.Type.FIRE:
                AudioClip d = AudioDataBase.inst.FireSpell;

                stepsSource.PlayOneShot(d, AppManager.inst.appdata.EffectsVolume);
                break;
            case Global.Type.WATER:
                AudioClip w = AudioDataBase.inst.WaterSpell;

                stepsSource.PlayOneShot(w, AppManager.inst.appdata.EffectsVolume);
                break;
            case Global.Type.THUNDER:
                AudioClip T = AudioDataBase.inst.ThunderSpell;

                stepsSource.PlayOneShot(T, AppManager.inst.appdata.EffectsVolume);
                break;
            case Global.Type.LIGHT:
                AudioClip l = AudioDataBase.inst.HealthSpell;

                stepsSource.PlayOneShot(l, AppManager.inst.appdata.EffectsVolume);
                break;
            default:
                break;
        }
    }
}

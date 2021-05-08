using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenuController : MonoBehaviour
{
    [SerializeField]
    MainMenuManager manager;


    [SerializeField]
    Slider musicBar;
    [SerializeField]
    Text musicText;
    [SerializeField]
    Slider voiceBar;
    [SerializeField]
    Text voiceText;
    [SerializeField]
    Slider effectsBar;
    [SerializeField]
    Text effectsText;

    // Start is called before the first frame update
    void Start()
    {
        


    }

    void OnEnable()
    {
        getMusicVolume();
        getVoiceVolume();
        getEffectsVolume();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MusicVolumeChange()
    { 
        musicText.text = musicBar.value.ToString();
        AppManager.inst.appdata.MusicVolume = (int)musicBar.value;
    }

    public void VoiceVolumeChange()
    {
        voiceText.text = voiceBar.value.ToString();
        AppManager.inst.appdata.VoiceVolume = (int)voiceBar.value;
    }

    public void EffectsVolumeChange()
    {
        effectsText.text = effectsBar.value.ToString();
        AppManager.inst.appdata.EffectsVolume = (int)effectsBar.value;
    }

    void getMusicVolume()
    {
        musicBar.value = AppManager.inst.appdata.MusicVolume;
        musicText.text = musicBar.value.ToString();
    }

    void getVoiceVolume()
    {
        voiceBar.value = AppManager.inst.appdata.VoiceVolume;
        voiceText.text = voiceBar.value.ToString();
    }

    void getEffectsVolume()
    {
        effectsBar.value = AppManager.inst.appdata.EffectsVolume;
        effectsText.text = effectsBar.value.ToString();
    }

    public void ExitMenu()
    {
        AppManager.inst.SaveAppData();
        manager.OpenMainMenu(this.gameObject);
    }
}

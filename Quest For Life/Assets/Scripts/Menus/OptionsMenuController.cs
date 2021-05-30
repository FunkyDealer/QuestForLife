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
        AppManager.inst.appdata.MusicVolume = ((float)musicBar.value / 100);
    }

    public void VoiceVolumeChange()
    {
        voiceText.text = voiceBar.value.ToString();
        AppManager.inst.appdata.VoiceVolume = ((float)voiceBar.value / 100);
    }

    public void EffectsVolumeChange()
    {
        effectsText.text = effectsBar.value.ToString();
        AppManager.inst.appdata.EffectsVolume = ((float)effectsBar.value /100);
    }

    void getMusicVolume()
    {
        musicBar.value = AppManager.inst.appdata.MusicVolume * 100;
        musicText.text = musicBar.value.ToString();
    }

    void getVoiceVolume()
    {
        voiceBar.value = AppManager.inst.appdata.VoiceVolume * 100;
        voiceText.text = voiceBar.value.ToString();
    }

    void getEffectsVolume()
    {
        effectsBar.value = AppManager.inst.appdata.EffectsVolume * 100;
        effectsText.text = effectsBar.value.ToString();
    }

    public void ExitMenu()
    {
        AppManager.inst.SaveAppData();
        manager.OpenMainMenu(this.gameObject);
    }
}

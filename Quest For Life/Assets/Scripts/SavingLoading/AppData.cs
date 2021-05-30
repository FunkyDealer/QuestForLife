using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AppData
{
    public float MusicVolume;
    public float VoiceVolume;
    public float EffectsVolume;

    public AppData(int MusicVolume, int VoiceVolume, int EffectsVolume)
    {
        this.MusicVolume = (float)MusicVolume / 100;
        this.VoiceVolume = (float)VoiceVolume / 100;
        this.EffectsVolume = (float)EffectsVolume / 100;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AppData
{
    public int MusicVolume;
    public int VoiceVolume;
    public int EffectsVolume;

    public AppData(int MusicVolume, int VoiceVolume, int EffectsVolume)
    {
        this.MusicVolume = MusicVolume;
        this.VoiceVolume = VoiceVolume;
        this.EffectsVolume = EffectsVolume;
    }
}

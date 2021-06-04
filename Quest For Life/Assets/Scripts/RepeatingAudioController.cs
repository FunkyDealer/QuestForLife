using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatingAudioController : AudioController
{

    // Start is called before the first frame update
    void Start()
    {
        timer = 0;

        calculateNewTime();       
    }

    // Update is called once per frame
    void Update()
    {
        if (!source.isPlaying)
        {
            if (timer < TimeBetweenSound) timer += Time.deltaTime;
            else
            {
                PlayOnce();
            }
        }
    }

    void PlayOnce()
    {
        source.PlayOneShot(source.clip, AppManager.inst.appdata.EffectsVolume);

        timer = 0;
        calculateNewTime();
    }

    void calculateNewTime()
    {
        float rng = Random.Range(0, 0.5f);
        float signal = Random.Range(0, 2);
        if (signal == 0) signal = -1;
        else signal = 1;
        TimeBetweenSound = AverageTimeBetweenSound + AverageTimeBetweenSound * (rng * signal);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarAudioController : AudioController
{
    [SerializeField]
    float Distance;

    [SerializeField]
    List<AudioClip> clips;

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
                float distance = Vector3.Distance(p.transform.position, this.transform.position);
                if (Distance <= distance) PlayOnce();

            }
        }
    }

    void PlayOnce()
    {
        int rng = Random.Range(0, clips.Count);
        source.PlayOneShot(clips[rng], AppManager.inst.appdata.EffectsVolume);

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

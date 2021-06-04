using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedStairs : MonoBehaviour
{
    [HideInInspector]
    public int numKeys;

    Animator[] animators;

    [SerializeField]
    List<GameObject> PillarSpots;

    [SerializeField]
    GameObject RedPillarPrefab;
    [SerializeField]
    GameObject GreenPillarPrefab;
    [SerializeField]
    GameObject BluePillarPrefab;
    [SerializeField]
    GameObject YellowPillarPrefab;

    private bool locked;
    public bool Locked => locked;

    [SerializeField]
    AudioSource StairsSound;

    private void Start()
    {
        locked = true;
        int key = 0;
        foreach (var s in PillarSpots)
        {
            SpawnPillar(key, s);
            key++;
            if (key == numKeys) key = 0;
        }


    
        animators = GetComponentsInChildren<Animator>();
    }

    public bool CheckKeys(List<int> keys)
    {
        if (keys.Count < numKeys)
        {
            return false;
        }
        else
        {
            open();
            return true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void open()
    {
        foreach (var a in animators)
        {
            a.SetTrigger("Open");            
        }
        playSound();
        locked = false;
    }

    void SpawnPillar(int key, GameObject s)
    {
        switch (key)
        {
            case 0:
                Instantiate(RedPillarPrefab, s.transform);
                break;
            case 1:
                Instantiate(GreenPillarPrefab, s.transform);
                break;
            case 2:
                Instantiate(BluePillarPrefab, s.transform);
                break;
            case 3:
                Instantiate(YellowPillarPrefab, s.transform);
                break;
        }
    }

    public void playSound()
    {
        StairsSound.PlayOneShot(StairsSound.clip, AppManager.inst.appdata.EffectsVolume);
    }
}

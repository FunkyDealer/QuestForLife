using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioDataBase : MonoBehaviour
{

    private static AudioDataBase _instance;
    public static AudioDataBase inst { get { return _instance; } }

    [SerializeField]
    List<AudioClip> SpellVoice;

    public Dictionary<int, AudioClip> spellVoice;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        //Set up dictionaries
        //Spell voices
        spellVoice = new Dictionary<int, AudioClip>();
        for (int i = 0; i < SpellVoice.Count; i++)
        {
            spellVoice.Add(i+1, SpellVoice[i]);
        }



    }

        // Start is called before the first frame update
        void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioDataBase : MonoBehaviour
{

    private static AudioDataBase _instance;
    public static AudioDataBase inst { get { return _instance; } }

    [SerializeField]
    List<AudioClip> SpellVoice;

    [SerializeField]
    List<AudioClip> StepsClips;

    [SerializeField]
    List<AudioClip> SlimeIntroSound;

    [SerializeField]
    List<AudioClip> SlimeHurtSound;

    [SerializeField]
    List<AudioClip> GhostIntroSound;
    [SerializeField]
    List<AudioClip> GhostHurtSound;

    public AudioClip dodgeSound;

    int currentStep;

    public AudioClip WaterSpell;
    public AudioClip FireSpell;
    public AudioClip ThunderSpell;
    public AudioClip HealthSpell;

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

        currentStep = Random.Range(0, StepsClips.Count);

    }

        // Start is called before the first frame update
        void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public AudioClip GetStep()
    {
        AudioClip o = StepsClips[currentStep];
        currentStep++;
        if (currentStep == StepsClips.Count) currentStep = 0;

        return o;
    }

    public AudioClip getSlimeIntroduction()
    {
        int i = Random.Range(0, SlimeIntroSound.Count);
        return SlimeIntroSound[i];
    }

    public AudioClip getSlimeHurt()
    {
        int i = Random.Range(0, SlimeHurtSound.Count);
        return SlimeHurtSound[i];
    }

    public AudioClip getGhostIntroduction()
    {
        int i = Random.Range(0, GhostIntroSound.Count);
        return GhostIntroSound[i];
    }

    public AudioClip getGhostHurt()
    {
        int i = Random.Range(0, GhostHurtSound.Count);
        return GhostHurtSound[i];
    }
}

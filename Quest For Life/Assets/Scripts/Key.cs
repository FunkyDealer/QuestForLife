using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public int id;
    [SerializeField]
    AudioSource audio;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Entered");
            Player p = other.gameObject.GetComponent<Player>();
            playSound();

            p.PickKey(id);
            Destroy(this.gameObject);
        }
    }


    private void playSound()
    {
        audio.gameObject.transform.parent = this.gameObject.transform.parent;
        audio.PlayOneShot(audio.clip, AppManager.inst.appdata.EffectsVolume);
        Destroy(audio.gameObject, 3);
    }
}

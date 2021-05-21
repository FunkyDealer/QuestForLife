using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public int id;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Entered");
            Player p = other.gameObject.GetComponent<Player>();
            p.PickKey(id);
            Destroy(this.gameObject);
        }
    }
}

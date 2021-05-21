using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveGameController : MonoBehaviour
{
    [HideInInspector]
    public Player player;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void ExitMenu()
    {
        Destroy(this.gameObject);
    }

    public void SaveGame(int slot)
    {
        DataManager.saveGame(slot, player);


    }
}

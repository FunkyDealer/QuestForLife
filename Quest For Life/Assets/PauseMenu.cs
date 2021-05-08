using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    NavigationInterfaceManager manager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SaveGame()
    {
        DataManager.saveGame(1, manager.player);
        manager.ClosePauseMenu();
    }

    public void CloseMenu()
    {
        manager.ClosePauseMenu();
    }

    public void QuitGame()
    {

    }
}

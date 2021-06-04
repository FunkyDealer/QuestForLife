using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    NavigationInterfaceManager manager;

    [SerializeField]
    GameObject saveMenuPrefab;

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
        GameObject o = Instantiate(saveMenuPrefab);
        SaveGameController s = o.GetComponent<SaveGameController>();
        s.player = manager.player;

    }

    public void CloseMenu()
    {
        manager.ClosePauseMenu();
    }

    public void QuitGame()
    {
        SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
    }
}

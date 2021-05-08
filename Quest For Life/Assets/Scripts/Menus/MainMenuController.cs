using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField]
    public MainMenuManager manager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void NewGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void LoadGame()
    {
        manager.OpenLoadMenu(this.gameObject);
    }

    public void OptionsMenu()
    {
        manager.OpenOptionsMenu(this.gameObject);
    }


    public void QuitGame()
    {
        Application.Quit();
    }


}

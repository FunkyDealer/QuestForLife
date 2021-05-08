using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    GameObject MainMenu;

    [SerializeField]
    GameObject LoadScreen;

    [SerializeField]
    GameObject OptionsMenu;


    // Start is called before the first frame update
    void Start()
    {
        MainMenu.SetActive(true);
        LoadScreen.SetActive(false);
        OptionsMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenMainMenu(GameObject sender)
    {
        sender.SetActive(false);
        MainMenu.SetActive(true);
    }

    public void OpenLoadMenu(GameObject sender)
    {
        sender.SetActive(false);
        LoadScreen.SetActive(true);
    }

    public void OpenOptionsMenu(GameObject sender)
    {
        sender.SetActive(false);
        OptionsMenu.SetActive(true);
    }


}

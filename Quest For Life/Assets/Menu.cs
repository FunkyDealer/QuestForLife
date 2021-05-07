using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField]
    GameObject canvas;
    [SerializeField]
    GameObject menu;
    [SerializeField]
    GameObject options;
    [SerializeField]
    Scrollbar VolBar;
    int volume;
    [SerializeField]
    Text volumeHud;
    [SerializeField]
    GameObject Load;
    public void Start()
    {
        
    }

    private void Update()
    {
    }

    public void Exit()
    {
        Application.Quit();
     
    }
    public void ChangeMtoO() 
    {
        options.SetActive(!options.activeSelf);
        menu.SetActive(!menu.activeSelf);
    }
    public void ChangeMtoL()
    {
        Load.SetActive(!Load.activeSelf);

        menu.SetActive(!menu.activeSelf);
    }
    public void ActiveOrResume() 
    {
        canvas.SetActive(!canvas.activeSelf);

    }
    public void ChangeVolume() 
    {
        volume = (int)(VolBar.value*100);
        volumeHud.text = ""+volume;
    }

    public void LoadGame() 
    {
        Load.SetActive(!Load.activeSelf);
    }
    public void ChangeScene() 
    {
        SceneManager.LoadScene("GameScene");
    }
}

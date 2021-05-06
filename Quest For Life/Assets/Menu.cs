using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M)) 
        {
           ActiveOrResume();
        }    
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
    public void ActiveOrResume() 
    {
        canvas.SetActive(!canvas.activeSelf);

    }
    public void ChangeVolume() 
    {
        volume = (int)(VolBar.value*100);
        volumeHud.text = ""+volume;
    }
}

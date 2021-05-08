using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadGameController : MonoBehaviour
{
    [SerializeField]
    MainMenuManager manager;

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
        manager.OpenMainMenu(this.gameObject);
    }
}

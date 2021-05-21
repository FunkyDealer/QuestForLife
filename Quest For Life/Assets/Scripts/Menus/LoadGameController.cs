using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGameController : MonoBehaviour
{
    [SerializeField]
    MainMenuManager manager;

    [SerializeField]
    GameObject dataMailPrefab;

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

    public void LoadGame(int slot)
    {
        SaveData data = DataManager.LoadGame(slot);

        GameObject o = Instantiate(dataMailPrefab);
        DataGameMail.inst.SetData(data);

        SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
    }
}

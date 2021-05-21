using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndSceneController : MonoBehaviour
{
    [SerializeField]
    float TimeToAllowSkip = 4;
    float timerToAllowSkip;
    bool allowSkip;

    [SerializeField]
    float TimeToEndScene = 6;
    float timerToEndScene;

    bool sceneEnded;

    void Awake()
    {
        timerToAllowSkip = 0;
        allowSkip = false;
        timerToEndScene = 0;
        sceneEnded = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!allowSkip)
        {
            if (timerToAllowSkip < TimeToAllowSkip) timerToAllowSkip += Time.deltaTime;
            else allowSkip = true;
        }
        else
        {
            if (!sceneEnded && Input.anyKey)
            {
                endScene();
            }

            if (timerToEndScene < TimeToEndScene) timerToEndScene += Time.deltaTime;
            else
            {
              if (!sceneEnded) endScene();
            }

        }




    }

    void endScene()
    {
        sceneEnded = true;
        GameObject o = Instantiate(DataBase.inst.ScreenChanger);
        FadeToBlackScreenChange f = o.GetComponent<FadeToBlackScreenChange>();
        f.Init(ChangeToMainMenu, false);
    }

    void ChangeToMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main Menu", UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
}

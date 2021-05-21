using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataGameMail : MonoBehaviour
{
    [HideInInspector]
    SaveData data;

    [HideInInspector]
    public bool Used;

    private static DataGameMail _instance;
    public static DataGameMail inst { get { return _instance; } }

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {


    }

    public void SetData(SaveData data)
    {
        this.data = data;
    }

    public SaveData getData()
    {
        Destroy(this.gameObject,1);
        return data;
    }

    
}

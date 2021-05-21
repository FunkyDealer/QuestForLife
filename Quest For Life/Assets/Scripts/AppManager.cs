using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class AppManager : MonoBehaviour
{
    private static AppManager _instance;
    public static AppManager inst { get { return _instance; } }
    
    public AppData appdata;



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

        appdata = LoadAppData();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SaveAppData()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/AppData.data";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, appdata);

        stream.Close();
    }

    AppData LoadAppData()
    {
        AppData data = new AppData(50,50,50);

        string path = Application.persistentDataPath + "/AppData.data";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            data = formatter.Deserialize(stream) as AppData;

            stream.Close();
        }


        return data;
    }
}

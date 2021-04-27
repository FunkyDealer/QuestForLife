using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropsDataBase : MonoBehaviour
{
    private static PropsDataBase _instance;
    public static PropsDataBase inst { get { return _instance; } }



    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

    }


    [SerializeField]
    public List<GameObject> CeilingLamps;

        // Start is called before the first frame update
        void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

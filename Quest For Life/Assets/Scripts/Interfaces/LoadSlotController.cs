using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSlotController : MonoBehaviour
{
    [SerializeField]
    LoadGameController controller;

    [SerializeField]
    GameObject DataObj;
    [SerializeField]
    GameObject noDataObj;


    [SerializeField]
    int slot;

    void Awake()
    {
        DataObj.SetActive(false);
        noDataObj.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnEnable()
    {
        if (DataManager.isData(slot)) DataObj.SetActive(true);
        else noDataObj.SetActive(true);
    }

    void OnDisable()
    {
        DataObj.SetActive(false);
        noDataObj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadGame()
    {
        controller.LoadGame(slot);
       
    }
}

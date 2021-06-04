using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSlotController : MonoBehaviour
{
    [SerializeField]
    SaveGameController controller;



    [SerializeField]
    GameObject noDataObj;


    [SerializeField]
    int slot;

    void Awake()
    {
        noDataObj.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    void OnEnable()
    {
        if (!DataManager.isData(slot)) noDataObj.SetActive(true);
    }

    void OnDisable()
    {
        noDataObj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SaveGame()
    {
        controller.SaveGame(slot);
        noDataObj.SetActive(false);
    }
}

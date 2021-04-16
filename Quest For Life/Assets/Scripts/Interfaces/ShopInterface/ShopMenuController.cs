using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopMenuController : MonoBehaviour
{
    [SerializeField]
    public ShopInterfaceManager manager;

    public Player player => manager.player;

    [SerializeField]
    protected Text PageDisplay;

    protected int currentPage;
    protected int TotalPages;
    protected int totalItems;

    [SerializeField]
    protected int itemsPerPage;


    [SerializeField]
    protected GameObject itemMenuTable;

    [SerializeField]
    protected GameObject ItemDisplayPrefab;

    [SerializeField]
    protected GameObject nextPageButton;
    [SerializeField]
    protected GameObject previousPageButton;

    protected RectTransform TableDisplay;

    protected void Awake()
    {
        totalItems = 0;
        TotalPages = 0;
        currentPage = 0;

        TableDisplay = itemMenuTable.GetComponent<RectTransform>();
    }

    // Start is called before the first frame update
    protected void Start()
    {
        NextPageButtonOff();
        PreviousPageButtonOff();

        PopulateTable();
        Setpages();
    }
       

    // Update is called once per frame
    void Update()
    {
        
    }

    protected virtual void PopulateTable()
    {

    }

    public void NextPage()
    {
        currentPage++;
        Setpages();

        TableDisplay.localPosition -= new Vector3(400, 0);

        if (currentPage < TotalPages) NextPageButtonOn();
        else NextPageButtonOff();
    }

    public void PreviousPage()
    {
        currentPage--;
        Setpages();

        TableDisplay.localPosition += new Vector3(400, 0);

        if (currentPage > 1) PreviousPageButtonOn();
        else PreviousPageButtonOff();
    }


    protected void Setpages()
    {
        if (TotalPages == 0) currentPage = 0;

        PageDisplay.text = $"{currentPage}/{TotalPages}";

        if (currentPage < TotalPages) NextPageButtonOn();
        else NextPageButtonOff();

        if (currentPage > 1) PreviousPageButtonOn();
        else PreviousPageButtonOff();
    }


    protected void NextPageButtonOff() => nextPageButton.SetActive(false);
    protected void NextPageButtonOn() => nextPageButton.SetActive(true);
    protected void PreviousPageButtonOff() => previousPageButton.SetActive(false);
    protected void PreviousPageButtonOn() => previousPageButton.SetActive(true);


    public void confirmation()
    {
        manager.LockInterface();
    }
}

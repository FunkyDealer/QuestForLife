using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MapManager
{
    [SerializeField]
    int width;
    [SerializeField]
    int length;

    [SerializeField]
    GameObject ShopGeneratorObj;

    Vector3 spawnPos3d;
    Vector2 spawn2d;
    Global.FacingDirection entranceDir;
    public Vector3 SpawnPos3d => spawnPos3d;
    public Vector2 Spawn2d => spawn2d;
    public Global.FacingDirection EntranceDir => entranceDir;
    

    private Shop shop;
    public Shop Shop => shop;

    [HideInInspector]
    public ShopExit shopExit;

    [SerializeField]
    GameObject shopInterfacePrefab;

    void Awake()
    {
        FreeTiles = new Dictionary<Tile, GameObject>();
        GameObject o = Instantiate(ShopGeneratorObj, this.gameObject.transform.position, this.gameObject.transform.rotation, this.gameObject.transform);
        ShopGenerator SG = o.GetComponent<ShopGenerator>();
        SG.manager = this;

        SG.Initiate(width, length);

        shop = new Shop(3, 6);
    }

    public void StartShop(MapGenerator DG, Vector2 spawn, Global.FacingDirection entranceDir)
    {

        spawnPos3d = FreeTiles[map[(int)spawn.x, (int)spawn.y]].transform.position;
        spawnPos3d.y = this.gameObject.transform.position.y + 1f;
        this.entranceDir = entranceDir;

        spawn2d = spawn;


        Destroy(DG);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RestockShop(int floor)
    {
        shop.RestockInventory(floor);
    }

    internal void UseShop(Player player)
    {
        GameObject o = Instantiate(shopInterfacePrefab, Vector3.zero, Quaternion.identity);
        ShopInterfaceManager e = o.GetComponent<ShopInterfaceManager>();
        e.ShopManager = this;
        e.player = player;
    }
 

    internal bool BuyItem(Item item, int slot, Player player)
    {
        return shop.buyItem(item, slot, player);
    }

    internal void CloseShop(Player player, GameObject interfaceObj)
    {
        Destroy(interfaceObj);
        player.ActivateNavigationInterface();
        player.MovementManager.ResumeMovement();
    }
    
}

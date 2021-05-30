using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopEntrance : PhysicalTile
{
    Animator animator;
    [HideInInspector]
    public ShopManager shopManager;

    [SerializeField]
    Vector2 pos2D;
    [SerializeField]
    Vector2 forward2D;

    System.Action<PhysicalTile> moveAction;
    System.Action telePortAction;

    [HideInInspector]
    public ShopData data = null;

    [SerializeField]
    AudioSource OpenSound;

    void Awake()
    {
        animator = GetComponent<Animator>();       
    }

    // Start is called before the first frame update
    void Start()
    {
        if (data == null) shopManager.Shop.RestockInventory(tile.floor);
        else shopManager.Shop.LoadShop(data);


        facing = tile.facing;
        pos2D = new Vector2(tile.x, tile.y);
        forward2D = new Vector2(tile.getForwardTile().x, tile.getForwardTile().y);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Open(System.Action teleport, System.Action<PhysicalTile> move)
    {
        this.telePortAction = teleport;
        this.moveAction = move;
        animator.SetBool("Open", true);
    }

    public void movePlayer()
    {
        moveAction(this);
    }

    public void teleportPlayer()
    {
        FadeToBlackScreenChange e = Instantiate(DataBase.inst.DoorScreenChanger, Vector3.zero, Quaternion.identity).GetComponent<FadeToBlackScreenChange>();
        e.Init(telePortAction, false);

        //telePortAction();
        animator.SetBool("Open", false);
    }

    public void playSound()
    {
        OpenSound.PlayOneShot(OpenSound.clip, AppManager.inst.appdata.EffectsVolume);
    }

}

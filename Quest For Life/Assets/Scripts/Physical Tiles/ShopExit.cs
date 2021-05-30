using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopExit : PhysicalTile
{
    Animator animator;
    [HideInInspector]
    public ShopManager shopManager;

    System.Action<PhysicalTile> moveAction;
    System.Action telePortAction;

    [SerializeField]
    AudioSource OpenSound;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }


    // Start is called before the first frame update
    void Start()
    {
        
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

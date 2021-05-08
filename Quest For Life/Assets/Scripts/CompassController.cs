using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompassController : MonoBehaviour
{
    [SerializeField]
    Player Player;

    Quaternion nextRotation;
    float nextRotationDir;

    [SerializeField]
    float turningVelocity = 50f;

    bool rotating;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (rotating)
        {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, nextRotation, turningVelocity * Time.deltaTime);
        }
    }

    void FixedUpdate()
    {
        if (rotating)
        {
            Vector3 targetUp = nextRotation * Vector3.up;

            if (Vector3.Dot(transform.up, targetUp) > 0.99995)
            {
                transform.localRotation = nextRotation;
                rotating = false;
            }
        }  
    }

    public void Initiate(Player p, Global.FacingDirection dir)
    {
        this.Player = p;

        SetDirectionFast(dir);
    }

    //void SetDirectionFast(Global.FacingDirection dir)
    //{
    //    switch (dir)
    //    {
    //        case Global.FacingDirection.NORTH:
    //            transform.Rotate(0, 0, 0);
    //            break;
    //        case Global.FacingDirection.EAST:
    //            transform.Rotate(0, 0, -90);
    //            break;
    //        case Global.FacingDirection.WEST:
    //            transform.Rotate(0, 0, 90);
    //            break;
    //        case Global.FacingDirection.SOUTH:
    //            transform.Rotate(0, 0, 180);
    //            break;
    //    }
    //}

    public void SetDirectionFast(Global.FacingDirection dir)
    {
        switch (dir)
        {
            case Global.FacingDirection.NORTH:
                transform.localEulerAngles = new Vector3(0, 0, 0);
                //  transform.Rotate(0, 0, 0);
                break;
            case Global.FacingDirection.EAST:
                //   transform.Rotate(0, 0, -90);
                transform.localEulerAngles = new Vector3(0, 0, -90);
             //   transform.localRotation = new Quaternion(0, 0, -90, 0);
                break;
            case Global.FacingDirection.WEST:
                //  transform.Rotate(0, 0, 90);
                transform.localEulerAngles = new Vector3(0, 0, 90);
               // transform.localRotation = new Quaternion(0, 0, 90, 0);
                break;
            case Global.FacingDirection.SOUTH:
                //  transform.Rotate(0, 0, 180);
                transform.localEulerAngles = new Vector3(0, 0, 180);
                //transform.localRotation = new Quaternion(0, 0, 180, 0);
                break;
        }
    }

    public void rotate(float dir) 
    {
        Quaternion r90 = Quaternion.AngleAxis(90 * -dir, Vector3.forward);
        nextRotation = transform.localRotation * r90;
        rotating = true;
    }
}

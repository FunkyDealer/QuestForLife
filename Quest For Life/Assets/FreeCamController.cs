using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeCamController : MonoBehaviour
{
    float sensitivity = 10F;
    float rotationX = 0F;
    float rotationY = 0F;
    float rotArrayX;
    float rotAverageX = 0F;
    float rotArrayY;
    float rotAverageY = 0F;
    Quaternion originalRotation;
    CharacterController controller;
    Vector3 dir;

    [SerializeField]
    float speed;

    float originalSpeed;

    [HideInInspector]
    public PlayerMov mov;

    void Awake()
    {
        controller = transform.parent.gameObject.GetComponent<CharacterController>();
        dir = Vector3.zero;
        originalSpeed = speed;
    }

    // Start is called before the first frame update
    void Start()
    {
        //sensitivity = SettingsManager.Instance.Sensivity();
        originalRotation = transform.localRotation;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Resets the average rotation
        rotAverageY = 0f;
        rotAverageX = 0f;

        //Gets rotational input from the mouse
        rotationY += (Input.GetAxis("Mouse Y") * sensitivity) * 100 * Time.deltaTime;
        rotationX += (Input.GetAxis("Mouse X") * sensitivity) * 100 * Time.deltaTime;

        rotationY = Mathf.Clamp(rotationY, -90, 90);

        //Adds the rotation values to their relative array
        rotArrayY = rotationY;
        rotArrayX = rotationX;

        //Adding up all the rotational input values from each array
        rotAverageY += rotArrayY;
        rotAverageX += rotArrayX;

        //Get the rotation you will be at next as a Quaternion
        Quaternion yQuaternion = Quaternion.AngleAxis(rotAverageY, Vector3.left);
        Quaternion xQuaternion = Quaternion.AngleAxis(rotAverageX, Vector3.up);

        //Rotate
        transform.localRotation = originalRotation * yQuaternion;

        transform.parent.localRotation = originalRotation * xQuaternion;

        Rotate();

        Move();

        if (Input.GetButtonDown("CloseMenu"))
        {
            switchBack();
        }
        

        //sensitivity = SettingsManager.Instance.Sensivity();
    }


    void Rotate()
    {

    }

    void Move()
    {
        if (Input.GetButton("Sprint"))
        {
            speed *= 2;
        }

        dir = Vector3.zero;
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        input = Vector2.ClampMagnitude(input, 1);
        dir = transform.right * input.x + transform.forward * input.y;

        if (Input.GetButton("Jump"))
        {
            dir.y += 1;
        }               

        if (Input.GetButton("crouch"))
        {
            dir.y -= 1;
        }



        moveCharacter(dir); //Movement Calculations

        speed = originalSpeed;
    }

    void moveCharacter(Vector3 d)
    {
        controller.Move(new Vector3(
        d.x * speed * Time.deltaTime,
        d.y * speed * Time.deltaTime,
        d.z * speed * Time.deltaTime
        ));

    }

    void switchBack()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        mov.returnToPlayer();

        Destroy(this.transform.parent.gameObject);


    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCon : MonoBehaviour
{
    float x, z;
    float speed = 0.4f;
    Vector3 velocity;
    Vector3 acceration;
    bool isJumping;
    public GameObject cam;
    public GameObject equip;
    Quaternion cameraRot, characterRot;
    float Xsensityvity = 3f, Ysensityvity = 3f;
    public bool canMove = true;

    public bool cursorLock = true;

    //??????????(?p?x???????p)
    float minX = -90f, maxX = 90f;

    // Start is called before the first frame update
    void Start()
    {
        cameraRot = cam.transform.localRotation;
        characterRot = transform.localRotation;
        velocity = Vector3.zero;
        acceration = new Vector3(0f, -0.05f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!canMove)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Space)&&!isJumping)
        {
            jump();
        }
        float xRot = Input.GetAxis("Mouse X") * Ysensityvity;
        float yRot = Input.GetAxis("Mouse Y") * Xsensityvity;

        cameraRot *= Quaternion.Euler(-yRot, 0, 0);
        characterRot *= Quaternion.Euler(0, xRot, 0);

        //Update????????????????????????
        cameraRot = ClampRotation(cameraRot);

        cam.transform.localRotation = cameraRot;
        transform.localRotation = characterRot;
        equip.transform.localRotation = cameraRot;

        UpdateCursorLock();
        if(canMove) gravity();
    }

    private void FixedUpdate()
    {
        x = 0;
        z = 0;

        x = Input.GetAxisRaw("Horizontal") * speed;
        z = Input.GetAxisRaw("Vertical") * speed;

        //transform.position += new Vector3(x,0,z);

       
            
        Vector3 moveDir = cam.transform.forward * z + cam.transform.right * x;
        moveDir.y = 0;

        transform.position += moveDir;
    }

    private void gravity()
    {
        transform.position += velocity;
        velocity += acceration;
        if (transform.position.y < -0.91f)
        {
            var tempPosi = this.transform.position;
            tempPosi.y = - 0.91f;
            this.transform.position = tempPosi;
            velocity = Vector3.zero;
            isJumping = false;
        }
    }


    public void UpdateCursorLock()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            cursorLock = false;
        }
        else if (Input.GetMouseButton(0))
        {
            cursorLock = true;
        }


        if (cursorLock)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else if (!cursorLock)
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    //?p?x??????????????
    public Quaternion ClampRotation(Quaternion q)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1f;

        float angleX = Mathf.Atan(q.x) * Mathf.Rad2Deg * 2f;

        angleX = Mathf.Clamp(angleX, minX, maxX);

        q.x = Mathf.Tan(angleX * Mathf.Deg2Rad * 0.5f);

        return q;
    }

    IEnumerator jumping()
    {
        for(int i=0; i < 10; i++)
        {
            yield return null;
            jump();
        }
       

    }

    void jump()
    {
        velocity = new Vector3(0f, 1.5f, 0f) ;
        isJumping = true;
    }

}
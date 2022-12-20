using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    float speed;
    float runSpeed = 5f;
    float walkSpeed = 3f;
    Quaternion targetRotation;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        move();   
    }

    private void move()
    {
        //カメラの向きで補正した入力ベクトルの取得
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");
        var horizontalRotation = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up);
        var velocity = horizontalRotation * new Vector3(horizontal, 0, vertical).normalized;
        Debug.Log(horizontal);
        Debug.Log(vertical);
        ///シフトを押しているときに、スピード走る速度を変える。
        
        speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
        if (horizontal == 0 && vertical == 0)
        {
            speed = 0f;
        }
        var rotationSpeed = 30 * Time.deltaTime;
        transform.position += velocity * speed * 1f * Time.deltaTime;

        //移動方向を向く
        /*
        if (velocity.magnitude > 0.5f)
        {
            targetRotation = Quaternion.LookRotation(velocity, Vector3.up);
        }
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed);
        */
    }
}

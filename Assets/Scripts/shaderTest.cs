using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shaderTest : MonoBehaviour
{
    public Material _material;
    bool flag = false;
   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        checkInput();
    }

    void checkFlag()
    {
 
        if(flag == false)
        {
            _material.SetFloat("_isCompleted", 0f);
        }
        else if (flag == true)
        {
            _material.SetFloat("_isCompleted", 1f);
        }
    }

    void checkInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            flag = !flag;
        }
        checkFlag();
    }
}

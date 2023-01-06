using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class getTextureFromShader : MonoBehaviour
{
    Texture MainTexture;
    [SerializeField]Material _material;
    [SerializeField] Material _material2;
    void Start()
    {
        _material.SetFloat("_Float", 1);
        MainTexture = _material.GetTexture("_MainTex");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            getImage();
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            deleteMud();
        }

    }

    void getImage()
    {
        
        RenderTexture buf = RenderTexture.GetTemporary(MainTexture.width, MainTexture.height);
        Graphics.Blit(MainTexture, buf,_material,0);
        _material2.SetTexture("_mainTex2", buf);
        MainTexture = buf;
    }

    void deleteMud()
    {
        _material.SetFloat("_Float", 0);

    }
}

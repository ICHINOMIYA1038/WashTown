using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterManager : MonoBehaviour
{
    [SerializeField] Material dirtyMaterial;
    [SerializeField] Texture MainTexture;
    Texture dirtyTexture;
    [SerializeField] Texture BlushTexture;
    [SerializeField] float blushScale;
    [SerializeField] Vector4 blushColor;
    [SerializeField] Material _material;
    [SerializeField] Material _material2;
    [SerializeField] GameObject[] waterLines;
    [SerializeField] Material washableMaterial;
    bool flag;
    float ratio = 0f;
    [SerializeField] GameObject src;
    Ray ray;
    RaycastHit hit;
    // Start is called before the first frame update
    void Start()
    {
        ray = new Ray(src.transform.position, src.transform.forward);
        _material.SetTexture("_MainTex", MainTexture);
        var texture = CreateTempTexture(2048, 2048, new Vector4(1f,1f,1f,0f));
        // これを呼ばないと色が書き込まれない
        texture.Apply(); ;
        dirtyTexture = texture;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            blushColor = Color.red;
            ray = new Ray(src.transform.position + src.transform.forward * 5f, src.transform.forward);
            if (Physics.Raycast(ray, out hit, 10))
            {
                if (hit.transform.gameObject.tag == "washable")
                {
                    Debug.DrawLine(ray.origin, hit.point);
                    Vector2 hitPosi = hit.textureCoord2;
                    paint(hitPosi);
                }

            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            ray = new Ray(src.transform.position + src.transform.forward * 5f, src.transform.forward);
            if (Physics.Raycast(ray, out hit, 10))
            {
                if (hit.transform.gameObject.tag == "washable")
                {
                    blushColor = Color.blue;
                    Debug.DrawLine(ray.origin, hit.point);
                    Vector2 hitPosi = hit.textureCoord2;
                    Debug.Log(hitPosi);
                    dirtify(hitPosi);
                }

            }
        }

        if (Input.GetMouseButton(0))
        {
            ratio += 0.01f;
            if (ratio > 1) { ratio = 1; }
            foreach (var waterLine in waterLines)
            {
                waterLine.transform.localScale = new Vector3(ratio, 1, 1);
            }


        }
        else if (Input.GetMouseButtonUp(0))
        {
            ratio = 0f;
            foreach (var waterLine in waterLines)
            {
                waterLine.transform.localScale = new Vector3(ratio, 1, 1);
            }
        }
    }



    void paint(Vector2 hitPosi)
    {

        _material.SetVector("_PaintUV", hitPosi);
        _material.SetTexture("_Blush", BlushTexture);
        _material.SetFloat("_BlushScale", blushScale);
        _material.SetVector("_BlushColor", blushColor);
        var buf = RenderTexture.GetTemporary(MainTexture.width, MainTexture.height);

        Graphics.Blit(MainTexture, buf, _material, 0);
        _material2.SetTexture("_MainTex", buf);
        MainTexture = buf;
        //buf.Release();
    }

    void dirtify(Vector2 hitPosi)
    {
        blushColor = new Vector4(0f, 0f, 0f, 1f);
        dirtyMaterial.SetVector("_PaintUV", hitPosi);
        dirtyMaterial.SetTexture("_Blush", BlushTexture);
        dirtyMaterial.SetFloat("_BlushScale", blushScale);
        dirtyMaterial.SetVector("_BlushColor", blushColor);
        var buf = RenderTexture.GetTemporary(dirtyTexture.width, dirtyTexture.height);

        Graphics.Blit(dirtyTexture, buf, dirtyMaterial, 0);

        washableMaterial.SetColor("_Color", new Vector4(0f, 0f, 0f, 1f));
        washableMaterial.SetTexture("_DirtyTex", buf);
        dirtyTexture = buf;
        //buf.Release();
    }

    void changeTexture(Material material, Color color)
    {
        blushColor = color;

    }


    private static Texture2D CreateTempTexture(int width, int height, Color defaultColor = default)
    {
        var texture = new Texture2D(width, height, TextureFormat.RGBA32, false);

        for (int y = 0; y < texture.height; y++)
            for (int x = 0; x < texture.width; x++)
                texture.SetPixel(x, y, defaultColor);
        return texture;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshCollider))]
public class WashableObject : MonoBehaviour
{
    [SerializeField]Texture mainTexture;
    Texture dirtyTexture;
    [SerializeField] Material washableMaterial;
    [SerializeField] Material dirtyMaterial;
    [SerializeField] Color maincolor;
    [SerializeField] Color dirtycolor;
    MeshRenderer renderer;
    int textureSize = 256;
    int allNum = 0;
    int dirtyCount = 0;
    [Range(0f,1f)]
    [SerializeField]float dirtyRatio;

    void Start()
    {
        this.gameObject.tag = "washable";
        renderer = GetComponent<MeshRenderer>();
        renderer.material = washableMaterial;
        washableMaterial.SetColor("_Color", dirtycolor);
        if (mainTexture == null)
        {
            var texture = CreateTempTexture(textureSize, textureSize, maincolor);
            texture.Apply();
            mainTexture = texture;
            washableMaterial.SetTexture("_MainTex", mainTexture);

        }
        washableMaterial.SetTexture("_MainTex", mainTexture);
        if (dirtyTexture == null)
        {
            var texture = CreateDirtyTexture(textureSize, textureSize, new Vector4(1f, 1f, 1f, 0f));
            
            texture.Apply();
            dirtyTexture = texture;
            washableMaterial.SetTexture("_DirtyTex", dirtyTexture);
        }
        calcDirty();


    }

    void Update()
    {

    }

    public Texture2D getDirtyTexture()
    {
        return ToTexture2D(dirtyTexture);
    }

    public void setDirtyRatio(float ratio)
    {
        dirtyRatio = ratio;
    }


    public void changeTexture(Vector2 hitPosi, float blushScale, Texture BlushTexture, Color blushColor)
    {

        dirtyMaterial.SetVector("_PaintUV", hitPosi);
        dirtyMaterial.SetTexture("_Blush", BlushTexture);
        dirtyMaterial.SetFloat("_BlushScale", blushScale);
        dirtyMaterial.SetVector("_BlushColor", blushColor);
        var buf = RenderTexture.GetTemporary(dirtyTexture.width, dirtyTexture.height);

        Graphics.Blit(dirtyTexture, buf, dirtyMaterial, 0);

        washableMaterial.SetTexture("_DirtyTex", buf);
        dirtyTexture = buf;
        calcDirty();
        //buf.Release();
    }

    private static Texture2D CreateTempTexture(int width, int height, Color defaultColor = default)
    {
        var texture = new Texture2D(width, height, TextureFormat.RGBAFloat, false);

        for (int y = 0; y < texture.height; y++)
            for (int x = 0; x < texture.width; x++)
                texture.SetPixel(x, y, defaultColor);
        return texture;
    }

    private  Texture2D CreateDirtyTexture(int width, int height, Color defaultColor)
    {
        float noise;
        Texture2D tempTexture = CreateTempTexture(width, height, defaultColor);
        for (int y = 0; y < tempTexture.height; y++)
        {
            for (int x = 0; x < tempTexture.width; x++)
            {
                noise = Random.Range(0f, 1.0f);
                if (noise < dirtyRatio)
                {
                    noise = 0f;
                    tempTexture.SetPixel(x, y, new Vector4(0f, 0f, 0f, noise));
                }

                else if (noise >= dirtyRatio)
                {
                    noise = 1f;
                    tempTexture.SetPixel(x, y, new Vector4(1f, 0f, 0f, noise));
                }
            }
        }
        return tempTexture;
    }

    void calcDirty()
    {
        Texture2D texture = ToTexture2D(dirtyTexture);
        var count = 0;
        var all = 0;
        for (int y = 0; y < texture.height; y++)
        {
            for (int x = 0; x < texture.width; x++)
            {
                float a = texture.GetPixel(x, y).a;
                if (a > 0.2)
                {
                    count += 1;
                }
                all += 1;

            }
        }
        dirtyCount = count;
        allNum = all;
    }

    public int getAllNum()
    {
        return allNum;
    }

    public int getDirtyCount()
    {
        return dirtyCount;
    }


    public Texture2D ToTexture2D(Texture self)
    {
        var sw = self.width;
        var sh = self.height;
        var format = TextureFormat.RGBA32;
        var result = new Texture2D(sw, sh, format, false);
        var currentRT = RenderTexture.active;
        var rt = new RenderTexture(sw, sh, 32);
        Graphics.Blit(self, rt);
        RenderTexture.active = rt;
        var source = new Rect(0, 0, rt.width, rt.height);
        result.ReadPixels(source, 0, 0);
        result.Apply();
        RenderTexture.active = currentRT;
        return result;
    }
}

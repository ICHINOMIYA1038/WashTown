using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 洗浄する対象を規定するクラス
/// このクラスのインスタンスにメインのテクスチャと汚れのテクスチャを保持する。
/// </summary>
[RequireComponent(typeof(MeshCollider))]
public class WashableObject : MonoBehaviour
{
    [SerializeField]Texture mainTexture;
    private Texture dirtyTexture;
    [SerializeField] Texture2D mudTexture;
    [SerializeField] Texture2D sampler;
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
        

        if (mudTexture == null)
        {
            var texture = CreateDirtyTexture(textureSize, textureSize, new Vector4(1f, 1f, 1f, 0f));
            if (sampler != null)
            {
                texture = TextureMultipy(sampler, texture);
            }
            texture.Apply();
            dirtyTexture = texture;
        }
        else
        {
                var texture = CreateDirtyTexture(textureSize, textureSize, new Vector4(1f, 1f, 1f, 0f));
                if (sampler != null)
                {
                    texture = TextureMultipy(sampler, mudTexture);
                    texture.Apply();
                    dirtyTexture = texture;
                }
            
        }
        washableMaterial.SetTexture("_DirtyTex", dirtyTexture);

        calcDirty();


    }

    void Update()
    {

    }

    void setTexture(Texture2D srcTexture)
    {
        dirtyTexture = srcTexture;
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
        dirtyTexture = washableMaterial.GetTexture("_DirtyTex");
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

    public static void InvertAlpha32(Texture2D texture)
    {
        // テクスチャのピクセルデータを取得する
        Color32[] pixels = texture.GetPixels32();

        // すべてのピクセルを走査し、α値を反転させる
        for (int i = 0; i < pixels.Length; i++)
        {
            Color32 pixel = pixels[i];
            // α値が0より大きい場合は0に変更し、0の場合は255に変更する
            if (pixel.a > 0)
            {
                pixel.a = 0;
            }
            else
            {
                pixel.a = 255;
            }
            pixels[i] = pixel;

            texture.SetPixels32(pixels);
            texture.Apply();
        }
    }

        public static Texture2D TextureMultipy(Texture2D _texture1, Texture2D _texture2)
    {
        // テクスチャが2つ入力されているか確認する
        if (_texture1 == null || _texture2 == null)
        {
            Debug.LogError("Please input two textures.");
            return null;
        }

        // テクスチャのサイズが同じか確認する
        if (_texture1.width != _texture2.width || _texture1.height != _texture2.height)
        {
            Debug.LogError("Texture sizes are different.");
            return null;
        }

        // 出力するテクスチャを作成する
        var outputTexture = new Texture2D(_texture1.width, _texture1.height);

        // すべてのピクセルを走査し、テクスチャを乗算する
        for (int y = 0; y < outputTexture.height; y++)
        {
            for (int x = 0; x < outputTexture.width; x++)
            {
                // 2つのテクスチャのピクセルを取得する
                var color1 = _texture1.GetPixel(x, y);
                var color2 = _texture2.GetPixel(x, y);

                // 2つのピクセルの色を乗算して、出力する色を作成する
                var outputColor = new Color(color1.r * color2.r, color1.g * color2.g, color1.b * color2.b, color1.a * color2.a);
                outputTexture.SetPixel(x, y, outputColor);
            }
        }

        // ピクセルデータをテクスチャに書き込み、適用する
        outputTexture.Apply();
        return outputTexture;
    }
}

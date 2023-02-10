using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーが発射する水を管理するクラス
/// </summary>
public class WaterManager : MonoBehaviour
{
    [SerializeField]ActionSoundManager soundManager;
    [SerializeField] Material dirtyMaterial;
    [SerializeField] Texture MainTexture;
    Texture dirtyTexture;
    [SerializeField] Texture BlushTexture;
    float blushScale = 0.03f;
    [SerializeField] float blushScale1 = 0.03f;
    [SerializeField] float blushScale2 = 0.05f;
    [SerializeField] float blushScale3 = 0.1f;
    [SerializeField] Vector4 blushColor;
    [SerializeField] Material _material;
    [SerializeField] Material _material2;
    [SerializeField] GameObject[] waterLines;
    [SerializeField] Material washableMaterial;
    /// <summary>
    /// 音声が流れているかどうか。重複を防ぐフラグ
    /// </summary>
    bool isAudioPlaying = false;
    bool flag;
    float ratio = 0f;
    [SerializeField] GameObject src;
    Ray ray;
    RaycastHit hit;

    /// <summary>
    /// ブラシサイズの初期化
    /// マテリアルのセット
    ///　洗浄するためのdirtyTextureをセット
    /// </summary>
    void Start()
    {
        blushScale = blushScale1;
        ray = new Ray(src.transform.position, src.transform.forward);
        _material.SetTexture("_MainTex", MainTexture);
        var texture = CreateTempTexture(2048, 2048, new Vector4(1f,1f,1f,0f));
        texture.Apply(); 
        dirtyTexture = texture;
    }

    // Update is called once per frame
    void Update()
    {
        //ゲームが終了していたら、処理をやめる。
        if (GameManager.gameEnd == true)
        {
            return;
        }
       
        ///water
        if (Input.GetMouseButton(0))
        {
            if (!isAudioPlaying)
            {
                soundManager.playSoundWater();
                isAudioPlaying = true;
            }
           
            ray = new Ray(src.transform.position + src.transform.forward * 5f, src.transform.forward);
            if (Physics.Raycast(ray, out hit, 30))
            {
                Debug.DrawLine(ray.origin, hit.point, Color.blue, 5f);
                if (hit.transform.gameObject.tag == "washable")
                {
                    WashableObject washableObject = hit.transform.gameObject.GetComponent<WashableObject>();
                    Vector2 hitPosi = hit.textureCoord;
                    washableObject.changeTexture(hitPosi, blushScale * 3f, BlushTexture, new Color(0f, 0f, 0f, 0f));
                }
            }
        }


            if (Input.GetMouseButtonDown(1))
        {
            ray = new Ray(src.transform.position + src.transform.forward * 5f, src.transform.forward);
            if (Physics.Raycast(ray, out hit, 10))
            {
                Debug.DrawLine(ray.origin, hit.point,Color.blue,5f);
                if (hit.transform.gameObject.tag == "washable")
                {
                    WashableObject washableObject = hit.transform.gameObject.GetComponent<WashableObject>();
                    Vector2 hitPosi = hit.textureCoord;
                    washableObject.changeTexture(hitPosi, blushScale,BlushTexture, new Color(0f, 0f, 0f, 1f));
                }
            }
        }

        else if (Input.GetMouseButtonUp(0))
        {
            ratio = 0f;
            foreach (var waterLine in waterLines)
            {
                waterLine.transform.localScale = new Vector3(ratio, 1, 1);
            }
            soundManager.stopSoundWater();
            isAudioPlaying = false; 
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
        var tempPosition = hitPosi;
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

    public void changeBlushScale(int flag)
    {
        if(flag == 0)
        {
            blushScale = blushScale1;
        }
        if (flag == 1)
        {
            blushScale = blushScale2;
        }
        if(flag == 2)
        {
            blushScale = blushScale3;
        }
    }
}
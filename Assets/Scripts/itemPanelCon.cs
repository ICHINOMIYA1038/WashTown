using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using TMPro;

/// <summary>
///
/// 購入画面を表示するためのスクリプト
/// Jsonデータから読み取り。
/// itemJsonDataクラスのインスタンスを生成する。
/// </summary>
public class itemPanelCon : MonoBehaviour
{
    [SerializeField]
    TextAsset text;
    [SerializeField]
    GameObject parent;
    [SerializeField]
    GameObject original;
    GameObject[] panels;
    shopJsonData data;
    string basePath;
    int num = 10;

    private void Start()
    {
        show();
    }

    public void show()
    {
        basePath = Application.dataPath + "/Image/WorkImage/";
        readJson();
        panels = new GameObject[num];
        for (int i = 0; i < num; i++)
        {
            panels[i] = Instantiate(original, parent.transform);
            string path = basePath + data.itemData[i].fileName;
            byte[] bytes = File.ReadAllBytes(path);
            Texture2D loadTexture = new Texture2D(1, 1); //mock size 1x1
            loadTexture.LoadImage(bytes);
            panels[i].transform.GetChild(1).GetComponent<RawImage>().texture = loadTexture;
            panels[i].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = data.itemData[i].itemName;
            panels[i].transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "$"+data.itemData[i].cost;
            panels[i].transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = data.itemData[i].effect;
        }
    }

    /// <summary>
    /// Jsonファイルを読み込む。workJsonDataは自作の型。
    /// </summary>
    private void readJson()
    {
        string jsonText = text.ToString();
        data = JsonUtility.FromJson<shopJsonData>(jsonText);



    }

}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using UnityEngine.UI;

/// <summary>
/// 依頼内容を表示するパネルを作るクラス
/// onStart()で全ての処理を実行する。
/// テキストアセットに配置したjsonファイルから、画像のパスとタイトルと本文を取得し、workJsonData型として格納する。
///　また、画像のパスからテクスチャをロードする。
///　パネル自体は元となるプレハブで管理する。
///　プレハブ内に、クリックした時の処理をするクラスのインスタンスを持たせている。
/// </summary>
public class WorkPanelCon : choicedManager
{
    [SerializeField]
    TextAsset text;
    [SerializeField]
    GameObject parent;
    [SerializeField]
    GameObject original;
    GameObject[] panels;
    workJsonData data;
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
            choiceBtn tempBtn = panels[i].GetComponent<choiceBtn>();
            tempBtn.choicedManager = this;
            tempBtn.index = i;
            string path =  basePath + data.workData[i].filename;
            byte[] bytes = File.ReadAllBytes(path);
            Texture2D loadTexture = new Texture2D(1, 1); //mock size 1x1
            loadTexture.LoadImage(bytes);
            panels[i].transform.GetChild(1).GetComponent<RawImage>().texture = loadTexture;
            panels[i].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = data.workData[i].header;
            panels[i].transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = data.workData[i].body;


        }
    }

    /// <summary>
    /// Jsonファイルを読み込む。workJsonDataは自作の型。
    /// </summary>
    private void readJson()
    {
       string jsonText = text.ToString();
       data = JsonUtility.FromJson<workJsonData>(jsonText);
       
    }

}

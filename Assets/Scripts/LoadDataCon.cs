using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using UnityEngine.UI;

/// <summary>
/// セーブデータをロードする時のパネルコントローラー
/// jsonファイルを読み取り、表示し、ボタンが押された時に色を変化させスタートボタンへindexを返す働きをする。
/// </summary>
public class LoadDataCon : choicedManager
{ 
    [SerializeField]
    GameObject parent;
    [SerializeField]
    GameObject original;
    GameObject[] panels;
    SaveData data;
    string basePath;
    int num = 3;
    int choicedSceneIndex;

    private void Start()
    {
        show();
    }

    public void show()
    {
        basePath = Application.streamingAssetsPath + "/Image/WorkImage/";
        readJson();
        panels = new GameObject[num];
        for (int i = 0; i < num; i++)
        {
            panels[i] = Instantiate(original, parent.transform);
            choiceBtn tempBtn = panels[i].GetComponent<choiceBtn>();
            tempBtn.choicedManager = this;
            tempBtn.index = i;
            //string path = basePath + data.workData[i].filename;
            //byte[] bytes = File.ReadAllBytes(path);
            //Texture2D loadTexture = new Texture2D(1, 1); //mock size 1x1
            //loadTexture.LoadImage(bytes);
            //panels[i].transform.GetChild(1).GetComponent<RawImage>().texture = loadTexture;
            panels[i].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "name:" + data.playerData[i].username;
            panels[i].transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "shopRate:"+ data.playerData[i].shopRate;
            panels[i].transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "townRate:" + data.playerData[i].townRate;
            panels[i].transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = "money:" + data.playerData[i].money;

        }
    }

    /// <summary>
    /// Jsonファイルを読み込む。workJsonDataは自作の型。
    /// </summary>
    private void readJson()
    {
        var textReader = new StreamReader(Application.streamingAssetsPath + "/savedata/savedata.json");
        string jsonText = textReader.ReadToEnd();
        textReader.Close();
        data = JsonUtility.FromJson<SaveData>(jsonText);

    }

}

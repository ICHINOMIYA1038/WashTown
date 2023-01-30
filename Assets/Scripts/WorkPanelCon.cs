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
    [SerializeField]
    TextMeshProUGUI indexText;

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
            workChoiceBtn tempBtn = panels[i].GetComponent<workChoiceBtn>();
            tempBtn.choicedManager = this;
            tempBtn.index = i;
            string path =  basePath + data.workData[i].filename;
            byte[] bytes = File.ReadAllBytes(path);
            Texture2D loadTexture = new Texture2D(1, 1); //mock size 1x1
            loadTexture.LoadImage(bytes);
            panels[i].transform.GetChild(1).GetComponent<RawImage>().texture = loadTexture;
            panels[i].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = data.workData[i].header;
            panels[i].transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = data.workData[i].body;
            tempBtn.status = GameManager.workIndex[i];
            if (tempBtn.getStatus()== 0)
            {
                tempBtn.comingSoonPanel.SetActive(true);
            }

        }
    }

    override public void choice(choiceBtn btn)
    {
        
        if (choicedBtn != null) choicedBtn.unchoiceEvent();
        if (btn == choicedBtn)
        {
            indexText.text = "引き受ける依頼を選択してください。";
            choicedBtn = null;
            choicedIndex = -1;
        }
        else
        {
            choicedBtn = btn;
            choicedIndex = choicedBtn.index;
            indexText.text = "引き受けた依頼:" + data.workData[choicedIndex].header;
        }

    }

    int getStatus(choiceBtn btn)
    {
        workChoiceBtn workBtn = (workChoiceBtn)btn;
        return workBtn.status;
    }

    /// <summary>
    /// Jsonファイルを読み込む。workJsonDataは自作の型。
    /// </summary>
    private void readJson()
    {
        var textReader = new StreamReader(Application.streamingAssetsPath + "/Json/work.json");
        string jsonText = textReader.ReadToEnd();
        textReader.Close();
        data = JsonUtility.FromJson<workJsonData>(jsonText);
    }

    public override int getIndex()
    {
        if (choicedBtn == null)
        {
            return -1;
        }
        if (getStatus(choicedBtn) != 1)
        {
            return -1;
        }
        return choicedBtn.index;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using UnityEngine.UI;


public class LoadDataCon : MonoBehaviour
{ 
    [SerializeField]
    GameObject parent;
    [SerializeField]
    GameObject original;
    GameObject[] panels;
    SaveData data;
    string basePath;
    int num = 3;
    choiceBtn choicedBtn;
    int choicedSceneIndex;
    [SerializeField]
    TextMeshProUGUI indexText;

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
            tempBtn.loadDataCon = this;
            tempBtn.index = i;
            //string path = basePath + data.workData[i].filename;
            //byte[] bytes = File.ReadAllBytes(path);
            //Texture2D loadTexture = new Texture2D(1, 1); //mock size 1x1
            //loadTexture.LoadImage(bytes);
            //panels[i].transform.GetChild(1).GetComponent<RawImage>().texture = loadTexture;
            panels[i].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "name:" + data.playerData[i].name;
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
        var textReader = new StreamReader(Application.dataPath + "/savedata/savedata.json");
        string jsonText = textReader.ReadToEnd();
        textReader.Close();
        data = JsonUtility.FromJson<SaveData>(jsonText);

    }

    public void choice(choiceBtn btn)
    {
        if (choicedBtn != null) choicedBtn.unchoiceEvent();
        if (btn == choicedBtn)
        {
            choicedBtn = null;
            choicedSceneIndex = 0;
        }
        else
        {
            choicedBtn = btn;
            choicedSceneIndex = choicedBtn.index;
        }
        indexText.text = "" + choicedSceneIndex;
        Debug.Log(choicedSceneIndex);
    }

    public int getIndex()
    {
        return choicedBtn.index;
    }
}

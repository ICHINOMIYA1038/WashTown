using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using TMPro;

public class InventoryPanelCon : choicedManager
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
    int num = 8;


    public void Start()
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
            string path = basePath + data.itemData[i].fileName;
            byte[] bytes = File.ReadAllBytes(path);
            Texture2D loadTexture = new Texture2D(1, 1); //mock size 1x1
            loadTexture.LoadImage(bytes);
            panels[i].transform.GetChild(1).GetComponent<RawImage>().texture = loadTexture;
            panels[i].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = data.itemData[i].itemName;
            panels[i].transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "所持数:" + GameManager.itemList[i];
        }
    }

    public void UpdateText()
    {
        if (!parent.activeSelf)
        {
            return;
        }
        for (int i = 0; i < num; i++)
        {
            panels[i].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = data.itemData[i].itemName;
            panels[i].transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "所持数:" + GameManager.itemList[i];
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



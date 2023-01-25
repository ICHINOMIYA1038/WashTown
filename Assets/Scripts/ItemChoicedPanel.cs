using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ItemChoicedPanel : choicedManager
{
    [SerializeField]
    GameObject parent;
    [SerializeField]
    GameObject original;
    GameObject[] panels;
    ItemData data;
    string basePath;
    int num = 3;

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
            string path = basePath + data.fileName;
            byte[] bytes = File.ReadAllBytes(path);
            Texture2D loadTexture = new Texture2D(1, 1); //mock size 1x1
            loadTexture.LoadImage(bytes);
            panels[i].transform.GetChild(1).GetComponent<RawImage>().texture = loadTexture;
            panels[i].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = data.itemName;
            panels[i].transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = ""+data.cost;

        }
    }

    private void readJson()
    {
        var textReader = new StreamReader(Application.dataPath + "/Json/itemJson.json");
        string jsonText = textReader.ReadToEnd();
        textReader.Close();
        data = JsonUtility.FromJson<ItemData>(jsonText);

    }
}

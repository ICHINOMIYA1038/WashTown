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
    shopJsonData data;
    string basePath;
    int num = 3;
    List<choiceBtn> btnList;
    public List<int> indexList;
     
    public void Start()
    {
        show();
        btnList = new List<choiceBtn>();
        indexList = new List<int>();
    }
    public void show()
    {
        basePath = Application.streamingAssetsPath + "/Image/ItemImage/";
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
            panels[i].transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "" + GameManager.itemList[i];
        }
    }

    public void UpdateText() 
    {
        if (!parent.gameObject.activeSelf)
        {
            return;
        }
        for (int i = 0; i < num; i++)
        {
            panels[i].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = data.itemData[i].itemName;
            panels[i].transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "" + GameManager.itemList[i];

        }
    }

    public override void choice(choiceBtn btn)
    {
        //所持数のチェック
        
        if (btnList.Contains(btn))
        {
            btnList.Remove(btn);
            GameManager.itemList[btn.index] += 1;
            UpdateText();
        }
        else if (GameManager.itemList[btn.index] <= 0)
        {
            btn.unchoiceEvent();
            return;
        }
        else
        {
            btnList.Add(btn);
            GameManager.itemList[btn.index] -= 1;
            UpdateText();
        }
    }

    public void extractIndex()
    {
        foreach(var elem in btnList)
        {
            indexList.Add(elem.index);
        }
    } 
    



    public new int getIndex()
    {
        if (choicedBtn == null)
        {
            return -1;
        }
        return choicedBtn.index;
    }

    private void readJson()
    {
        var textReader = new StreamReader(Application.streamingAssetsPath + "/Json/itemJson.json");
        string jsonText = textReader.ReadToEnd();
        textReader.Close();
        data = JsonUtility.FromJson<shopJsonData>(jsonText);

    }

    
}

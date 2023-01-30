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
    [SerializeField] TextMeshProUGUI itemName;
    [SerializeField] TextMeshProUGUI itemEffectText;

    public void Start()
    {
        show();
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
            Texture2D loadTexture = new Texture2D(1, 1); 
            loadTexture.LoadImage(bytes);
            panels[i].transform.GetChild(1).GetComponent<RawImage>().texture = loadTexture;
            panels[i].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = data.itemData[i].itemName;
            panels[i].transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "所持品:" + GameManager.itemList[i];
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
            panels[i].transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "所持品:" + GameManager.itemList[i];
        }
    }

    override public void choice(choiceBtn btn)
    {
        
        if (choicedBtn != null) choicedBtn.unchoiceEvent();
        if (btn == choicedBtn)
        {
            choicedBtn = null;
            choicedIndex = -1;
        }
        else
        {
            choicedBtn = btn;
            choicedIndex = choicedBtn.index;
            itemName.text = data.itemData[choicedIndex].itemName;
            itemEffectText.text = data.itemData[choicedIndex].effect;
        }
        

    }




    /// <summary>
    /// Json?t?@?C?????????????BworkJsonData?????????^?B
    /// </summary>
    private void readJson()
    {
        var textReader = new StreamReader(Application.streamingAssetsPath + "/Json/itemJson.json");
        string jsonText = textReader.ReadToEnd();
        textReader.Close();
        data = JsonUtility.FromJson<shopJsonData>(jsonText);
    }

}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System;

/// <summary>
/// Achievementにインスタンスを持たせて、パネルのexecuteを行う。
/// </summary>
public class AchievePanelGenerator : MonoBehaviour
{
    [SerializeField] RawImage image;
    [SerializeField] TextMeshProUGUI headerText;
    [SerializeField] TextMeshProUGUI descText;
    [SerializeField] AchievementSystem achievementSystem;

    Queue<achieveData> sendData = new Queue<achieveData>();

    public void Start()
    {
        
        if (achievementSystem == null)
        {
            Debug.Log("aa");
        }
    }

    public void receiveNotification(int id)
    {
        if (achievementSystem == null)
        {
            Debug.Log("aa");
        }
        AchievementSystem.elemData data = achievementSystem.getFromId(id);
        byte[] bytes = File.ReadAllBytes(Application.dataPath +"/Image/Achievement/"+ data.filename);
        Texture2D texture = new Texture2D(64, 64);
        texture.LoadImage(bytes);
        achieveData achiveData = new achieveData()
        {
            texture = texture,
            header = data.header,
            desc = data.body
        };
        sendData.Enqueue(achiveData);

       
    }

    struct achieveData
    {
        public Texture2D texture;
        public string header;
        public string desc;
    }

    public void setImage(Texture2D texture)
    {
        image.texture = texture;
    }

    public void setHeader(string text)
    {
        headerText.text = text;
    }

    public void setDescText(string text)
    {
        descText.text = text;
    }
    /// <summary>
    /// パネルの出現から消えるまで
    /// </summary>
    public void execute()
    {
        if (sendData.Count <= 0)
        {
            return;
        }
        achieveData executeData =  sendData.Dequeue();
        
            setImage(executeData.texture);
            setHeader(executeData.header);
            setDescText(executeData.desc);
             
    }
}

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
    [SerializeField] Vector2 StartPosi;
    [SerializeField] Vector2 EndPosi;
    Queue<achieveData> sendData = new Queue<achieveData>();
    //パネルの出現速度
    [SerializeField] float swipeSpeed;
    //パネル出現後の待機時間
    [SerializeField] float waitTime;
    bool isExecute=false;

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
            return;
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

    public void Update()
    {
        if (sendData.Count <= 0)
        {
            return;
        }
        if (!isExecute)
        {
            isExecute = true;
            execute();
        }
        
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
        StartCoroutine(SwingPanel());
        
    }

    IEnumerator SwingPanel()
    {
        
        RectTransform recttransform = this.GetComponent<RectTransform>();
        Vector2 posi = recttransform.anchoredPosition;
        Debug.Log("exe");
        //ズレの許容
        float delta = 3.0f;
        while (delta < Mathf.Abs(EndPosi.x - posi.x))
        {
            posi.x += swipeSpeed*(EndPosi.x - StartPosi.x) / Mathf.Abs(EndPosi.x - StartPosi.x);
            recttransform.anchoredPosition = posi;
            yield return null;
        }
        yield return new WaitForSeconds(waitTime);
        while (delta < Mathf.Abs(StartPosi.x - posi.x))
        {
            posi.x -= swipeSpeed * (EndPosi.x - StartPosi.x) / Mathf.Abs(EndPosi.x - StartPosi.x);
            recttransform.anchoredPosition = posi;
     
            yield return null;
        }
        isExecute = false;

    }
}

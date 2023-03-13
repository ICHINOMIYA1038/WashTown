using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

//[CreateAssetMenu(menuName = "MyScriptable/Create EnemyData")]
public class AchievementSystem : MonoBehaviour
{

    public string fileName = "JSON/AchievementData.json";
    private achieveData data;

    [System.Serializable]
    public class achieveData
    {
        public elemData[] datas;
    
    }

    public elemData getFromId(int id)
    {
        Debug.Log(id);
        return data.datas[id];
    }
    [System.Serializable]
    public struct elemData
    {
        public string filename;
        public string header;
        public string body;
    }

    private void Awake()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);
        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            data = JsonUtility.FromJson<achieveData>(jsonData);
            Debug.Log(data.datas[0].filename);
        }
        else
        {
            Debug.Log("NotExist");
        }
       
    }

}

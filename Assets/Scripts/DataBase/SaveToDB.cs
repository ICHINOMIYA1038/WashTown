using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SaveToDB : MonoBehaviour
{
    public static SaveToDB instance;



    private static string baseUrl = "https://ichinomiya.gekidankatakago.com/Unity/update.php";
    static private string url;
    static private int id;
    private static string DataResult = "";
    // Start is called before the first frame update

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {
        id = 7;
        url = baseUrl;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void setId(int num)
    {
        id = num;
    }

    public static void SaveData(int money, int shopRate, int townRate, int itemList)
    {
        instance.StartCoroutine(WaitForResponse( money, shopRate,townRate, itemList, result =>
        {
            DataResult = result;
        }));
    }


    static IEnumerator  WaitForResponse( int money ,int shopRate,int townRate,int itemList, Action<string> callback)
    {


        url = baseUrl + "/?id=" + id;
        Debug.Log(url);
        WWWForm form = new WWWForm();

        form.AddField("money", money);
        form.AddField("shopRate", shopRate);
        form.AddField("townRate", townRate);
        form.AddField("itemList", itemList);
        form.AddField("id", id);

        // データを送信してレスポンスを受け取る
        using UnityWebRequest request = UnityWebRequest.Post(url, form);
        yield return request.SendWebRequest();

        // エラーが起きた場合はエラーテキストを表示して終了
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error");
            callback("Error");
            yield break;
        }

        // レスポンスのテキストを取得して、成功テキストを表示してから3秒後にシーンを遷移する
        string responseText = request.downloadHandler.text;
        Debug.Log(responseText);
        if (responseText == "OK")
        {
            callback(responseText);
            // 認証が成功した場合はメインシーンに遷移
            
        }
        else
        {
            callback(responseText);
        }

    }
}

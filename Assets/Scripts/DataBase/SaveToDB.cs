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

        // �f�[�^�𑗐M���ă��X�|���X���󂯎��
        using UnityWebRequest request = UnityWebRequest.Post(url, form);
        yield return request.SendWebRequest();

        // �G���[���N�����ꍇ�̓G���[�e�L�X�g��\�����ďI��
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error");
            callback("Error");
            yield break;
        }

        // ���X�|���X�̃e�L�X�g���擾���āA�����e�L�X�g��\�����Ă���3�b��ɃV�[����J�ڂ���
        string responseText = request.downloadHandler.text;
        Debug.Log(responseText);
        if (responseText == "OK")
        {
            callback(responseText);
            // �F�؂����������ꍇ�̓��C���V�[���ɑJ��
            
        }
        else
        {
            callback(responseText);
        }

    }
}

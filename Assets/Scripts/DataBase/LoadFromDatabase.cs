using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LoadFromDatabase : MonoBehaviour
{
    public static LoadFromDatabase instance;


    
    private static string baseUrl = "https://ichinomiya.gekidankatakago.com/Unity/load.php";
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

    public static void LoadData()
    {
        instance.StartCoroutine(WaitForResponse(result =>
        {
            DataResult = result;
        }));
    }

    public static string getData()
    {
        Debug.Log(DataResult);
        return DataResult;
    }

    static IEnumerator  WaitForResponse(Action<string> callback)
    {


        url = baseUrl + "/?id=" + id;
        using UnityWebRequest request = UnityWebRequest.Get(url);
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
        if (responseText == "OK")
        {
            callback(responseText);
            // �F�؂����������ꍇ�̓��C���V�[���ɑJ��
            UnityEngine.SceneManagement.SceneManager.LoadScene("main");
        }
        else
        {
            callback(responseText);
        }

    }
}

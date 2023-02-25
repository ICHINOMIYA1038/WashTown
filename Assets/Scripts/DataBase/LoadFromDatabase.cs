using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LoadFromDatabase : MonoBehaviour
{

    private string baseUrl = "https://ichinomiya.gekidankatakago.com/Unity/load.php";
    private string url;
    private int id;
    public InputField usernameInputField;
    public InputField passwordInputField;
    public Button loginButton;
    public Text resultText;
    // Start is called before the first frame update
    void Start()
    {
        url = baseUrl;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string loadData()
    {
        StartCoroutine(WaitForResponse());
    }

    IEnumerator WaitForResponse()
    {


        url = baseUrl + "/?id=" + id;
        using UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        // エラーが起きた場合はエラーテキストを表示して終了
        if (request.result != UnityWebRequest.Result.Success)
        {
            resultText.color = Color.red;
            resultText.text = "データベースの接続エラー";
            Debug.LogError("Error");
            yield break;
        }

        // レスポンスのテキストを取得して、成功テキストを表示してから3秒後にシーンを遷移する
        string responseText = request.downloadHandler.text;
        if (responseText == "OK")
        {
            resultText.color = Color.green;
            resultText.text = "ログインに成功しました。";
            // 認証が成功した場合はメインシーンに遷移
            UnityEngine.SceneManagement.SceneManager.LoadScene("main");
        }
        else
        {
            resultText.color = Color.red;
            resultText.text = responseText;
        }

    }
}

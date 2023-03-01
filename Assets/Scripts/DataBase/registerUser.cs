using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Security.Cryptography;
using System;

/// <summary>
/// ユーザーの新規登録のコード
/// </summary>
public class registerUser : MonoBehaviour
{
    public InputField usernameInput;
    public InputField passwordInput;
    public Button insertButton;
    public Text resultText;
    public GameObject Loadcanvas;
    public Image Loadimage;
    public Sprite image0;
    public Sprite image1;
    public Sprite image2;
    private string url = "https://ichinomiya.gekidankatakago.com/Unity/register.php"; 

    void Start()
    {
        insertButton.onClick.AddListener(Insert);
    }

    void Insert()
    {
        StartCoroutine(nowLoading());
        StartCoroutine(InsertCoroutine());
    }

    IEnumerator nowLoading()
    {
        Loadcanvas.SetActive(true);
        yield return new WaitForSeconds(1.1f);
        Loadimage.sprite = image0;
        yield return new WaitForSeconds(1.1f);
        Loadimage.sprite = image1;
        yield return new WaitForSeconds(1.1f);
        Loadimage.sprite = image2;
        Loadcanvas.SetActive(false);
        
    }

    IEnumerator InsertCoroutine()
    {
        // パスワードをハッシュ化
        string hashedPassword = HashPassword(passwordInput.text);

        // POSTデータの作成
        WWWForm form = new WWWForm();
        Debug.Log(usernameInput.text);

        form.AddField("username", usernameInput.text);
        form.AddField("password_hash", hashedPassword);

        // データを送信してレスポンスを受け取る
        using UnityWebRequest request = UnityWebRequest.Post(url, form);
        yield return request.SendWebRequest();

        // エラーが起きた場合はエラーテキストを表示して終了
        if (request.result != UnityWebRequest.Result.Success)
        {
            resultText.color = Color.red;
            resultText.text = "データベースの接続エラー";
            yield break;
        }
        
        // レスポンスのテキストを取得して、成功テキストを表示してから3秒後にシーンを遷移する
        string responseText = request.downloadHandler.text;
        string[] arr = new string[2];
        arr = responseText.Split(',');
        if (!arr[0].Equals("NG"))
        {
            resultText.color = Color.green;
            resultText.text = "新規ユーザー登録が完了しました。";
            yield return new WaitForSeconds(3);
            int id = Int32.Parse(arr[1]);
            GameManager.NewGame(id, usernameInput.text);
        }
        else
        {
            resultText.color = Color.red;
            resultText.text = responseText;
        }
        
        
    }

    // パスワードをハッシュ化する関数
    string HashPassword(string password)
    {
        byte[] passwordBytes = System.Text.Encoding.UTF8.GetBytes(password);
        SHA256 sha256 = SHA256.Create();
        byte[] hashBytes = sha256.ComputeHash(passwordBytes);
        string hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        // ここにハッシュ化の処理を記述する
        return hash;
    }
}
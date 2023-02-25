using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class LoginForm : MonoBehaviour
{
    public InputField usernameInputField;
    public InputField passwordInputField;
    public Button loginButton;
    public Text resultText;

    private string url = "https://ichinomiya.gekidankatakago.com/Unity/login.php";

    void Start()
    {
        loginButton.onClick.AddListener(Login);
    }

    void Login()
    {
        
        // レスポンスを待機
        StartCoroutine(WaitForResponse());
    }

    IEnumerator WaitForResponse()
    {
        // ユーザー名とパスワードを取得
        string username = usernameInputField.text;
        string password = passwordInputField.text;

        // パスワードをSHA256でハッシュ化
        string hashedPassword = HashSHA256(password);

        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password_hash", hashedPassword);
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

    string HashSHA256(string password)
    {
        System.Security.Cryptography.SHA256Managed sha256 = new System.Security.Cryptography.SHA256Managed();
        byte[] passwordBytes = System.Text.Encoding.UTF8.GetBytes(password);
        byte[] hashedBytes = sha256.ComputeHash(passwordBytes);
        return System.BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
    }
}
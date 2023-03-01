using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using System;

public class LoginForm : MonoBehaviour
{
    public InputField usernameInputField;
    public InputField passwordInputField;
    public Button loginButton;
    public Text resultText;
    public GameObject Loadcanvas;
    public Image Loadimage;
    public Sprite image0;
    public Sprite image1;
    public Sprite image2;


    private string url = "https://ichinomiya.gekidankatakago.com/Unity/login.php";

    void Start()
    {
        loginButton.onClick.AddListener(Login);
    }

    void Login()
    {
        StartCoroutine(nowLoading());
        // ���X�|���X��ҋ@
        StartCoroutine(WaitForResponse());
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

    IEnumerator WaitForResponse()
    {
        // ���[�U�[���ƃp�X���[�h���擾
        string username = usernameInputField.text;
        string password = passwordInputField.text;

        // �p�X���[�h��SHA256�Ńn�b�V����
        string hashedPassword = HashSHA256(password);

        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password_hash", hashedPassword);
        using UnityWebRequest request = UnityWebRequest.Post(url, form);
        yield return request.SendWebRequest();

        // �G���[���N�����ꍇ�̓G���[�e�L�X�g��\�����ďI��
        if (request.result != UnityWebRequest.Result.Success)
        {
            resultText.color = Color.red;
            resultText.text = "�f�[�^�x�[�X�̐ڑ��G���[";
            yield break;
        }

        // ���X�|���X�̃e�L�X�g���擾���āA�����e�L�X�g��\�����Ă���3�b��ɃV�[����J�ڂ���
        string responseText = request.downloadHandler.text;
        string[] arr = new string[2];
        arr = responseText.Split(',');
        if (!arr[0].Equals("NG"))
        {
            int id = Int32.Parse(arr[1]);
            resultText.color = Color.green;
            resultText.text = "���O�C���ɐ������܂����B";
            // �F�؂����������ꍇ�̓��C���V�[���ɑJ��
            GameManager.LoadFromDataBase(id);
            

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
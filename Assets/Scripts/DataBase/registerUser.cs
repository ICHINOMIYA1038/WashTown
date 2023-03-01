using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Security.Cryptography;
using System;

/// <summary>
/// ���[�U�[�̐V�K�o�^�̃R�[�h
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
        // �p�X���[�h���n�b�V����
        string hashedPassword = HashPassword(passwordInput.text);

        // POST�f�[�^�̍쐬
        WWWForm form = new WWWForm();
        Debug.Log(usernameInput.text);

        form.AddField("username", usernameInput.text);
        form.AddField("password_hash", hashedPassword);

        // �f�[�^�𑗐M���ă��X�|���X���󂯎��
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
            resultText.color = Color.green;
            resultText.text = "�V�K���[�U�[�o�^���������܂����B";
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

    // �p�X���[�h���n�b�V��������֐�
    string HashPassword(string password)
    {
        byte[] passwordBytes = System.Text.Encoding.UTF8.GetBytes(password);
        SHA256 sha256 = SHA256.Create();
        byte[] hashBytes = sha256.ComputeHash(passwordBytes);
        string hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        // �����Ƀn�b�V�����̏������L�q����
        return hash;
    }
}
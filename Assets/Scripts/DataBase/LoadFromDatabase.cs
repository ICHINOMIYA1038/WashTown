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

        // �G���[���N�����ꍇ�̓G���[�e�L�X�g��\�����ďI��
        if (request.result != UnityWebRequest.Result.Success)
        {
            resultText.color = Color.red;
            resultText.text = "�f�[�^�x�[�X�̐ڑ��G���[";
            Debug.LogError("Error");
            yield break;
        }

        // ���X�|���X�̃e�L�X�g���擾���āA�����e�L�X�g��\�����Ă���3�b��ɃV�[����J�ڂ���
        string responseText = request.downloadHandler.text;
        if (responseText == "OK")
        {
            resultText.color = Color.green;
            resultText.text = "���O�C���ɐ������܂����B";
            // �F�؂����������ꍇ�̓��C���V�[���ɑJ��
            UnityEngine.SceneManagement.SceneManager.LoadScene("main");
        }
        else
        {
            resultText.color = Color.red;
            resultText.text = responseText;
        }

    }
}

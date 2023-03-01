using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabButton : MonoBehaviour
{
    public GameObject register;
    public GameObject login;
    public bool isRegister=true;
    Button button;
    // Start is called before the first frame update
    void Start()
    {
        button = this.GetComponent<Button>();
        button.onClick.AddListener(btnClick);
    }

    // Update is called once per frame
    void btnClick()
    {
        if (isRegister)
        {
            register.SetActive(true);
            login.SetActive(false);
        }
        else
        {
            login.SetActive(true);
            register.SetActive(false);
        }
    }
}

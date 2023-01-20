using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainUIManager : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI moneyText;
    [SerializeField]
    GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        changeMoney();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeMoney()
    {
        moneyText.text = "$" + gameManager.getMoney();
    }

    public void changeShopRank()
    {

    }

    public void changeTownRank()
    {

    }
}

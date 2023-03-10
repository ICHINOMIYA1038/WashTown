using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventCanvas : MonoBehaviour
{
    [SerializeField] StarGenerator[] starGenerators = new StarGenerator[3];
    [SerializeField] TextMeshProUGUI moneyText;
    [SerializeField] 　Button inventBtn;
    [SerializeField] RawImage image;
    [SerializeField]Texture[] mainimageTexture;
    [SerializeField] int[] requiredMoney;
    [SerializeField] GameManager gameManager;
    int starNum = 0;
    // Start is called before the first frame update
    void Start()
    {
        inventBtn.onClick.AddListener(UpdateStar);
        init(GameManager.getShopRate());
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    
    public void Update()
    {
        changeButton();
    }

    void init(int shopRate)
    {
        if (shopRate == 0)
        {
            starNum = 0;
            moneyText.text = "店を拡大するのに $"+requiredMoney[0]+ " 必要です";
        }
        if (shopRate == 1)
        {
            starNum = 1;
            moneyText.text = "店を拡大するのに $" + requiredMoney[1] + " 必要です";
        }
        if (shopRate == 2)
        {
            starNum = 2;
            moneyText.text = "店を拡大するのに $" + requiredMoney[2] + " 必要です";
        }
        if (shopRate == 3)
        {
            starNum = 3;
            moneyText.text = "店を拡大するのに $" + requiredMoney[3] + " 必要です";
        }
        if (shopRate == 4)
        {
            starNum = 4;
            moneyText.text = "店を拡大するのに $" + requiredMoney[4] + " 必要です";
        }
        UpdateCanvas();
        changeButton();
    }

    void UpdateStar()
    {

        if (starNum < 5)
        {
            gameManager.setMoney(gameManager.getMoney() - requiredMoney[starNum]);
            starNum += 1;
            
            GameManager.setShopRate(starNum);
            gameManager.setWorkIndex(starNum,1);
            UpdateCanvas();
        }
        changeButton();





    }

    void changeButton()
    {
        if (requiredMoney[starNum] <= GameManager.returnMoney())
            inventBtn.enabled = true;
        else if (requiredMoney[starNum] > GameManager.returnMoney())
            inventBtn.enabled = false;
    }
    void UpdateCanvas()
    {
        image.texture = mainimageTexture[starNum];
        starGenerators[0].setStarNum(starNum);
        starGenerators[1].setStarNum(starNum);
        if (starNum <5)
        {
            starGenerators[2].setStarNum(starNum + 1);
            moneyText.text = "店を拡大するのに $" + requiredMoney[starNum] + " 必要です";
        }
        else if (starNum==5)
        {
            moneyText.text = "Completed!";
        }
    }
}

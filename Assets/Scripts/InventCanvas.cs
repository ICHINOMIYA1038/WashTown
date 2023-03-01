using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventCanvas : MonoBehaviour
{
    [SerializeField] StarGenerator[] starGenerators = new StarGenerator[3];
    [SerializeField] TextMeshProUGUI moneyText;
    [SerializeField] @Button inventBtn;
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
            moneyText.text = "“X‚ğŠg‘å‚·‚é‚Ì‚É $"+requiredMoney[0]+ " •K—v‚Å‚·";
        }
        if (shopRate == 1)
        {
            starNum = 1;
            moneyText.text = "“X‚ğŠg‘å‚·‚é‚Ì‚É $" + requiredMoney[1] + " •K—v‚Å‚·";
        }
        if (shopRate == 2)
        {
            starNum = 2;
            moneyText.text = "“X‚ğŠg‘å‚·‚é‚Ì‚É $" + requiredMoney[2] + " •K—v‚Å‚·";
        }
        if (shopRate == 3)
        {
            starNum = 3;
            moneyText.text = "“X‚ğŠg‘å‚·‚é‚Ì‚É $" + requiredMoney[3] + " •K—v‚Å‚·";
        }
        if (shopRate == 4)
        {
            starNum = 4;
            moneyText.text = "“X‚ğŠg‘å‚·‚é‚Ì‚É $" + requiredMoney[4] + " •K—v‚Å‚·";
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
        }
        else if (starNum==5)
        {
            moneyText.text = "Completed!";
        }
    }
}

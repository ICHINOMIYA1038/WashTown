using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PurchaseManager : MonoBehaviour
{
    IDictionary<int, int> purchaseList = new Dictionary<int, int>();
    [SerializeField]
    TextMeshProUGUI[] numTexts;
    [SerializeField]
    TextMeshProUGUI[] itemNameTexts;
    [SerializeField]
    TextMeshProUGUI sumText;
    shopJsonData shopData;
    [SerializeField]
    TextAsset text;
    [SerializeField]
    GameManager gameManager;
    

    // Start is called before the first frame update
    public void Start()
    {
        readJson();
    }

    public int add(int index,int num)
    {
        if (num < 0) return -4;//そのほかのエラー
        int tempCost = getItemCost(index) * num;
        if(costSum() + tempCost > gameManager.getMoney())
        {
            return -3;
        } 
        if (purchaseList.ContainsKey(index))
        {
            if(purchaseList[index] + num > 9)
            {
                return -2;//購入個数オーバー
            }
            purchaseList[index] = purchaseList[index] + num;
            textUpdate();
            return 0;//正常追加
        }
        else
        {
            if (purchaseList.Count >= 5)
            {
                return -1; //要素数オーバー
            }
            purchaseList.Add(index, num);
            textUpdate();
            return 0; //正常追加
        }
    }

    public void delete(int index)
    {
        if (purchaseList.ContainsKey(index))
        {
            purchaseList.Remove(index);
        }
        textUpdate();
    }

    public void decrement(int index)
    {
        if (purchaseList.ContainsKey(index))
        {
            purchaseList[index] = purchaseList[index] -1;
        }
        else
        {
            return;
        }
        if (purchaseList[index] == 0)
        {
            delete(index);
        }
        textUpdate();
    }

    public void textUpdate()
    {
        for(int j = 0; j < 5; j++)
        {
            numTexts[j].text = "";
            itemNameTexts[j].text =  "";
        }
        int i = 0;
        foreach (var elem in purchaseList)
        {
            numTexts[i].text = elem.Value + "個";
            itemNameTexts[i].text = getItemName(elem.Key);
            i++;
        }
        sumText.text = "$" + costSum();
    }

    public int costSum()
    {
        int sum = 0;
        foreach(var elem in purchaseList)
        {
            sum += getItemCost(elem.Key) * elem.Value;
        }
        return sum;
    }

    public string getItemName(int index)
    {
       
       return shopData.itemData[index].itemName;
    }

    public int getItemCost(int index)
    {
        return shopData.itemData[index].cost;
    }

    /// <summary>
    /// Jsonファイルを読み込む。workJsonDataは自作の型。
    /// </summary>
    private void readJson()
    {
        string jsonText = text.ToString();
        shopData = JsonUtility.FromJson<shopJsonData>(jsonText);
    }

    public void cancel()
    {
        purchaseList.Clear();
        textUpdate();
    }

    public void addItem()
    {
        foreach(var elem in purchaseList)
        {
            GameManager.itemList[elem.Key] += elem.Value;
            if (GameManager.itemList[elem.Key] > 99)
            {
                GameManager.itemList[elem.Key] = 99;
            }
        }
       
    }

    public void buy()
    {
        addItem();
        gameManager.setMoney(GameManager.money - costSum());
        gameManager.textUpdate();
        cancel();
    }

}

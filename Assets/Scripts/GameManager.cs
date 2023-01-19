using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameManager : MonoBehaviour
{
    string playerName;
    int dataSlot = 5;
    SaveData savedata;
    PlayerData playerData;
    int playerIndex = 0;
    string itemList = "111111111";
    /// <summary>
    /// ShopRank??????
    /// </summary>
    enum ShopRank
    {
        zero,
        one,
        two,
        three,
        four,
        five,
    }
    /// <summary>
    /// TownRank??????
    /// </summary>
    enum TownRank
    {
        zero,
        one,
        two,
        three,
        four,
        five,
    }

    /// <summary>
    /// shopRank?????????l
    /// </summary>
    enum ShopRateThreshHold
    {
        zeroToOne = 1000,
        oneToTwo = 2000,
        twoToThree = 3000,
        threeToFour = 4000,
        fourToFive = 5000,
    }

    /// <summary>
    /// TownRank?????????l
    /// </summary>
    enum TownRateThreshHold
    {
        zeroToOne = 1000,
        oneToTwo = 2000,
        twoToThree = 3000,
        threeToFour = 4000,
        fourToFive = 5000,
    }

    /// <summary>
    /// townRank???l?@?????l??0,????5????
    /// </summary>
    int townRank=0;
    /// <summary>
    /// shopRank???l?@?????l??0,????5????
    /// </summary>
    int shopRank=0;
    /// <summary>
    /// townRate??townRank?????????????????[?g?l
    /// </summary>
    int townRate =100;
    /// <summary>
    /// shopRate??shopRank?????????????????[?g?l
    /// </summary>
    int shopRate = 200;
    /// <summary>
    /// ?X????????????????(4byte???????A2147483647?????????????C???t????)
    /// </summary>
    int money = 10000;

    void addTownRate(int num)
    {
        townRate += num;
    }

    void addShopRate(int num)
    {
        shopRate += num;
    }
    void addShop(int num)
    {
        shopRate += num;
    }

    void checkShopRate()
    {
        
    }

    public void createSaveData()
    {
        if (savedata != null)
        {
            return;
        }
        savedata = new SaveData(dataSlot);

    }

    public void save()
    {
        createSaveData();
        StreamWriter writer;
        
        savedata.upDateData(playerName,money,shopRate,townRate,itemList,playerIndex);
        string jsonstr = JsonUtility.ToJson(savedata);
        writer = new StreamWriter(Application.dataPath + "/savedata/savedata.json", false);
        writer.Write(jsonstr);
        writer.Flush();
        writer.Close();

    }

}

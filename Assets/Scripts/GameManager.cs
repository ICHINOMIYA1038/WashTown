using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    MainUIManager mainUIManager;
    string playerName;
    int dataSlot = 5;
    SaveData savedata;
    PlayerData playerData;
    int playerIndex = 0;
    string itemList = "111111111";

    int SceneIndex = SCENE_TITLE;
    public static readonly int SCENE_TITLE = 0;
    public static readonly int SCENE_MAIN = 1;
    public static readonly int SCENE_ACTION = 2;

    private void Start()
    {
        DontDestroyOnLoad(this);
    }

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

    public int getMoney()
    {
        return money;
    }

    public void setMoney(int num)
    {
        money = num;
        if (SceneIndex == SCENE_MAIN)
        {
            mainUIManager.changeMoney();
        }
        
    }

    public int getShopRank()
    {
        return shopRank;
    }

    public int getTownRank()
    {
        return townRank;
    }

    public void setShopRank(int rank)
    {
        shopRank = rank;
    }

    public void setTownRank(int rank)
    {
        townRank = rank;
    }

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

    public void Load(int index)
    {
        dataSlot = index;
        
        var textReader = new StreamReader(Application.dataPath + "/savedata/savedata.json");
        textReader.Read();
        string jsonText = textReader.ToString();
        textReader.Close();
        savedata = JsonUtility.FromJson<SaveData>(jsonText);
        Debug.Log(savedata.playerData[index].name);
    }

    public void SceneChanage(int src, int dst)
    {
        SceneIndex = dst;
        if(src==SCENE_TITLE && dst == SCENE_MAIN)
        {
            
        }

        
    }

}

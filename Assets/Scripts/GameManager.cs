using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    /*
    public static GameManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }
    */



    public static string playerName;
    int dataSlot = 5;
    static SaveData savedata;
    PlayerData playerData;
    public static int playerIndex = 0;
    public static int[] itemList;

    public static int SceneIndex = SCENE_TITLE;
    public static readonly int SCENE_TITLE = 0;
    public static readonly int SCENE_MAIN = 1;
    public static readonly int SCENE_ACTION = 2;

    /// <summary>
    /// UIのUpdateに関するオブジェクト
    /// </summary>
    [SerializeField]
    ItemChoicedPanel itemChoicedPanel;
    [SerializeField]
    MainUIManager mainUIManager;
    [SerializeField]
    InventoryPanelCon inventoryPanelCon;


    public void Start()
    {
        
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
    public static int townRank =0;
    /// <summary>
    /// shopRank???l?@?????l??0,????5????
    /// </summary>
    public static int shopRank =0;
    /// <summary>
    /// townRate??townRank?????????????????[?g?l
    /// </summary>
    public static int townRate =100;
    /// <summary>
    /// shopRate??shopRank?????????????????[?g?l
    /// </summary>
    public static int shopRate = 200;
    /// <summary>
    /// ?X????????????????(4byte???????A2147483647?????????????C???t????)
    /// </summary>
    public static int money = 10000;

    public int getMoney()
    {
        return money;
    }

    public void setMoney(int num)
    {
        money = num;
        mainUIManager.changeMoney();
    }

    public void textUpdate()
    {
        
        mainUIManager.changeMoney();
        itemChoicedPanel.UpdateText();
        inventoryPanelCon.UpdateText();
        

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

    public static void Load(int index)
    {
        playerIndex = index;
        itemList = new int[10];
        var textReader = new StreamReader(Application.dataPath + "/savedata/savedata.json");
        string jsonText = textReader.ReadToEnd();
        textReader.Close();
        savedata = JsonUtility.FromJson<SaveData>(jsonText);
        money = savedata.playerData[index].money;
        playerName = savedata.playerData[index].name;
        townRate = savedata.playerData[index].townRate;
        shopRate = savedata.playerData[index].shopRate;
        itemList = savedata.playerData[index].itemList;
    }

    public static void SceneChanage(int src, int dst)
    {
        SceneIndex = dst;
        if(src==SCENE_TITLE && dst == SCENE_MAIN)
        {
            SceneManager.LoadScene(dst);
        } 
    }



}

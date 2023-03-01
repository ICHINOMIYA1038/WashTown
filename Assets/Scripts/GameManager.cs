using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class GameManager : MonoBehaviour
{
    
    public static GameManager instance;

    /*
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

    //?????p?l???????I?????????d??
    //1 ?I?????\ 0 ?I???s???@-1?@??????
    public static int[] workIndex = new int[] { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};

    public static string playerName;
    int dataSlot = 5;
    static SaveData savedata;
    static PlayerData playerData;
    public static int playerIndex = 0;
    public static int[] itemList;

    public static int SceneIndex = SCENE_TITLE;
    public static readonly int SCENE_TITLE = 0;
    public static readonly int SCENE_MAIN = 1;
    public static readonly int SCENE_ACTION = 2;

    public static bool gameEnd = false;
    public static int defaultTownRate = 0;
    public static int defaultShopRate = 0;
    public static int defaultMoney = 5000;

    /// <summary>
    /// UI??Update?????????I?u?W?F?N?g
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
    public static int townRate =0;
    /// <summary>
    /// shopRate??shopRank?????????????????[?g?l
    /// </summary>
    public static int shopRate = 0;
    /// <summary>
    /// ?X????????????????(4byte???????A2147483647?????????????C???t????)
    /// </summary>
    public static int money = 10000;



    public int getMoney()
    {
        return money;
    }

    public static int returnMoney()
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

    public static int getShopRank()
    {
        return shopRank;
    }


    public  static int getShopRate()
    {
        return shopRate;
    }
    public static  void setShopRate(int num)
    {
        shopRate = num;
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
        
        savedata.upDateData(playerName,money,shopRate,townRate, DigitsToNumber(itemList), playerIndex);
        string jsonstr = JsonUtility.ToJson(savedata);
        writer = new StreamWriter(Application.streamingAssetsPath + "/savedata/savedata.json", false);
        writer.Write(jsonstr);
        writer.Flush();
        writer.Close();
    }

    public static void SavetoDataBase()
    {
        SaveToDB.setId(playerIndex);
        Debug.Log(money);
        SaveToDB.SaveData(money,shopRate,townRate, DigitsToNumber(itemList));
        
        
    }

    public static void NewGame(int id,string userName)
    {
        playerIndex = id;
        money = defaultMoney;
        playerName = userName;
        townRate = defaultTownRate;
        shopRate = defaultShopRate;
        itemList = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0};
        UnityEngine.SceneManagement.SceneManager.LoadScene("main");

    }



    public static void Load(int index)
    {
        playerIndex = index;
        itemList = new int[9];

        var textReader = new StreamReader(Application.streamingAssetsPath + "/savedata/savedata.json");
        string jsonText = textReader.ReadToEnd();
        textReader.Close();
        
        
        savedata = JsonUtility.FromJson<SaveData>(jsonText);
        money = savedata.playerData[index].money;
        playerName = savedata.playerData[index].username;
        townRate = savedata.playerData[index].townRate;
        shopRate = savedata.playerData[index].shopRate;
        itemList = NumberToDigits(savedata.playerData[index].itemList);
    }

    

    public static  async void LoadFromDataBase(int index)
    {
        playerIndex = index;
        LoadFromDatabase.setId(playerIndex);
        LoadFromDatabase.LoadData();
        await LoginTask();

    }

    public static async Task LoginTask()
    {
        await Task.Delay(3000);
        string jsonText = LoadFromDatabase.getData();
        if (jsonText == "" || jsonText==null)
        {
            Debug.LogError("Error");
            return;
        }
        playerData = JsonUtility.FromJson<PlayerData>(jsonText);
        money = playerData.money;
        playerName = playerData.username;
        townRate = playerData.townRate;
        shopRate = playerData.shopRate;
        itemList = NumberToDigits(playerData.itemList);
        UnityEngine.SceneManagement.SceneManager.LoadScene("main");
    }

    static int[] NumberToDigits(long number)
    {
        int[] digits = new int[9];

        for (int i = 8; i >= 0; i--)
        {
            digits[i] = (int)(number % 10);
            number /= 10;
        }

        return digits;
    }

    static int DigitsToNumber(int[] digits)
    {
        int number = 0;

        for (int i = 0; i < 9; i++)
        {
            number = number * 10 + digits[i];
        }

        return number;
    }

    public static void SaveToDataBase()
    {

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

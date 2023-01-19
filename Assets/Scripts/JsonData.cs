using UnityEngine;

[System.Serializable]
public class workJsonData
{
    [SerializeField]
    public WorkData[] workData = null;
}

[System.Serializable]
public class WorkData
{
    [SerializeField]
    public string filename = string.Empty;
    [SerializeField]
    public string header = string.Empty;
    [SerializeField]
    public string body = string.Empty;

} // class PersonalData

[System.Serializable]
public struct AccessoryData
{
    [SerializeField]
    public string name;
    [SerializeField, Range(0, 10)]
    public int defense;
} // AccessoryData


[System.Serializable]
public class shopJsonData
{
    [SerializeField]
    public ItemData[] itemData = null;
    

}

[System.Serializable]
public class ItemData
{
    [SerializeField]
    public string itemName = string.Empty;
    [SerializeField]
    public string fileName = string.Empty;
    [SerializeField]
    public int cost;
    [SerializeField]
    public string effect = string.Empty;
}


[System.Serializable]
public class SaveData
{
    [SerializeField]
    public PlayerData[] playerData = null;


    public void upDateData(string name,int money, int shopRate, int townRate, string itemList, int playerIndex)
    {
        PlayerData data = new PlayerData(name,money, shopRate, townRate, itemList);
        playerData[playerIndex] = data;
    }

    public SaveData(int slot)
    {
        playerData = new PlayerData[slot];
    }
    
}

[System.Serializable]
public class PlayerData
{
    [SerializeField]
    //　名前
    public string name;
    [SerializeField]
    //　publicデータ
    public int money;
    //　staticデータ
    //shopRateに依存して店が豪華になる
    [SerializeField]
    public int shopRate;
    //townRateに依存して客層が変わる
    [SerializeField]
    public int townRate;
    [SerializeField]
    public string itemList = string.Empty;

    public PlayerData(string name, int money, int shopRate, int townRate, string itemList)
    {
        this.name = name;
        this.money = money;
        this.shopRate = shopRate;
        this.townRate = townRate;
        this.itemList = itemList;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アクション画面中での、アイテムの処理。
/// アクション前に選択されたアイテムのインデックスを受け取り、処理する。
/// シーンごとに、indexListにアイテムの番号を選択しておくことを忘れないようにする。
/// </summary>
public class ItemManager : MonoBehaviour
{
    static List<int> items;
    [SerializeField]
    GameObject item1;
    [SerializeField]
    GameObject item2;
    [SerializeField]
    GameObject item3;
    [SerializeField]
    GameObject itemImage1;
    [SerializeField]
    GameObject itemImage2;
    [SerializeField]
    GameObject itemImage3;
    [SerializeField]
    int[] indexList;
    static bool item1Active = true;
    static bool item2Active = false;
    static bool item3Active = false;
    [SerializeField] WaterManager watermanager;
    [SerializeField] WashableObject washableObject;
    // Start is called before the first frame update
    void Start()
    {
        Inactive();
        CheckItem();
        changeActive();
        ItemEffect();
        ItemClear();
       

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void setItemList(List<int> itemList)
    {
        items = itemList;
    }

    void CheckItem()
    {
        for(int i=0; i<3; i++)
        {
            if (items.Contains(indexList[i]))
            {
                if (i == 0) item1Active = true;
                if (i == 1) item2Active = true;
                if (i == 2) item3Active = true;

            }
           
        }
    }

    void Inactive()
    {
        item1Active = false;
        item2Active = false;
        item3Active = false;
        if (!item1Active)
        {
            if (item1 != null) item1.SetActive(false);
            if (itemImage1 != null) itemImage1.SetActive(false);
        }
        if (!item2Active)
        {
            if (item2 != null) item2.SetActive(false);
            if (itemImage2 != null) itemImage2.SetActive(false);
        }
        if (!item3Active)
        {
            if (item3 != null) item3.SetActive(false);
            if (itemImage3 != null) itemImage3.SetActive(false);
        }

    }

    void changeActive()
    {
        if (item1Active)
        {
            if (item1 != null) item1.SetActive(true);
            if (itemImage1 != null) itemImage1.SetActive(true);
        }
        if (item2Active)
        {
            if (item2 != null) item2.SetActive(true);
            if (itemImage2 != null) itemImage2.SetActive(true);
        }
        if (item3Active)
        {
            if (item3 != null) item3.SetActive(true);
            if (itemImage3 != null) itemImage3.SetActive(true);
        }
    }

    static void ItemClear()
    {
        items.Clear();
    }

    void ItemEffect()
    {


        ///洗剤の効果
        if (items.Contains(0))
        {
            washableObject.setDirtyRatio(0.8f);
        }
        else
        {
            washableObject.setDirtyRatio(0.6f);
        }

        ///ノズルの効果
        if (items.Contains(1))
        {
            watermanager.changeBlushScale(1);
        }
        else
        {
            watermanager.changeBlushScale(0);
        }

     
    }
}

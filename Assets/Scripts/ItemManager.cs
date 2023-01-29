using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    static List<int> items;
    [SerializeField]
    GameObject item1;
    static bool item1Active = true;
    static bool item2Active = false;
    // Start is called before the first frame update
    void Start()
    {
        if (item1Active)
        {
            item1.SetActive(true);
        }
        else
        {
            item1.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    static void setItemList(List<int> itemList)
    {
    }

}

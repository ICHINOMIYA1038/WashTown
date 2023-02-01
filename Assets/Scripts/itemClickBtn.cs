using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 購入画面のアイテムをクリックした時に処理をするボタン
/// </summary>
public class itemClickBtn : OriginalBtn
{
    public int itemIndex;
    public PurchaseManager purchaseManager;

    // Start is called before the first frame update
    void Start()
    {
        this.onClickCallback = clickEvent;
    }

    void clickEvent()
    {
        purchaseManager.add(itemIndex, 1);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 購入ボタン
/// このボタンを押すと、PurchaseManagerの購入処理を行う。
/// </summary>
public class BuyBtn : OriginalBtn
{
    [SerializeField] PurchaseManager purchaseManager;
    void Start()
    {
        this.onClickCallback = clickEvent;
    }

    void clickEvent()
    {
        purchaseManager.buy();

    }
}

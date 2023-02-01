using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clearBtn : OriginalBtn
{
    /// <summary>
    /// 購入画面で、買い物かごに入っているリストを全消去するためのボタン
    /// </summary>
    [SerializeField] PurchaseManager purchaseManager;

    // Start is called before the first frame update
    void Start()
    {
        this.onClickCallback = clickEvent;
    }

    void clickEvent()
    {
        purchaseManager.cancel();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clearBtn : OriginalBtn
{
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

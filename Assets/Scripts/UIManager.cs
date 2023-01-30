using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    [SerializeField] WashableObject[] washableObjects;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] progressGauge progress;
    [SerializeField] FPSCon fpscon;
    [SerializeField] GameObject mainCanvas;
    [SerializeField] GameObject endCnavas;
    [SerializeField] GameObject gameClearCanvas;
    int earnMoney = 5000;


    int allPixel = 0;
    int dirtyCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        calcAllPixel();

    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.gameEnd)
        {
            return;
        }
        calcAllPixel();
        calcRatio();
        completeCheck();
    }

    void calcAllPixel()
    {
        int allCount = 0;
        int dirtyNum = 0;
        foreach (var elem in washableObjects)
        {
            allCount += elem.getAllNum();
            dirtyNum += elem.getDirtyCount();
        }
        allPixel = allCount;
        dirtyCount = dirtyNum;
    }

    void calcRatio()
    {
        float ratio = 100*(float)(allPixel - dirtyCount) / (float)allPixel;
        text.text = String.Format("{0:#.##}%", ratio);
        progress.setRatio(ratio / 100);
    }

    void completeCheck()
    {
        Debug.Log(dirtyCount);
        if(dirtyCount <= 4)
        {
            complete();
        }  
    }

    void complete()
    {
        GameManager.gameEnd = true;
        fpscon.canMove = false;
        mainCanvas.SetActive(false);
        gameClearCanvas.SetActive(true);
        StartCoroutine(endCanvasAppear());
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        fpscon.cursorLock = false;
    }

    IEnumerator endCanvasAppear()
    {
        yield return new WaitForSeconds(3.0f);
        endCnavas.SetActive(true);
        GameManager.money += earnMoney;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    [SerializeField]ActionSoundManager soundManager;
    [SerializeField] WashableObject[] washableObjects;
    [SerializeField] TextMeshProUGUI headerMsg;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] TextMeshProUGUI endText;
    [SerializeField] TextMeshProUGUI moneyText;
    [SerializeField] progressGauge progress;
    [SerializeField] progressGauge endProgress;
    [SerializeField] FPSCon fpscon;
    [SerializeField] GameObject mainCanvas;
    [SerializeField] GameObject endCnavas;
    [SerializeField] GameObject gameClearCanvas;
    [SerializeField] GameObject gameOverCanvas;
    [SerializeField] TextMeshProUGUI resultEvaluation;
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
        if (GameManager.gameEnd)
        {
            return;
        }
        Debug.Log(dirtyCount);
        if(dirtyCount <= 4)
        {
            complete();
        }  
    }

    public void complete()
    {
        soundManager.stopSoundWater();
        soundManager.playSoundGameClear();
        float ratio = 100 * (float)(allPixel - dirtyCount) / (float)allPixel;
        endText.text = String.Format("{0:#}%", ratio);
        endProgress.setRatio(ratio / 100);
        headerMsg.text = "ƒ~ƒbƒVƒ‡ƒ“’B¬";
        GameManager.gameEnd = true;
        fpscon.canMove = false;
        mainCanvas.SetActive(false);
        gameClearCanvas.SetActive(true);

        StartCoroutine(endCanvasAppear());
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        fpscon.cursorLock = false;
        earnMoney = 5000;
        moneyText.text = "$" + earnMoney + "";
    }

    public void Failure()
    {
        soundManager.stopSoundWater();
        soundManager.playSoundGameFailure();
        float ratio = 100 * (float)(allPixel - dirtyCount) / (float)allPixel;
        endText.text = String.Format("{0:#}%", ratio);
        endProgress.setRatio(ratio / 100);
        headerMsg.text = "ƒ~ƒbƒVƒ‡ƒ“Ž¸”s";
        resultEvaluation.text = getResultEvaluation(ratio);
        earnMoney = 0;
        moneyText.text = "$"+earnMoney + "";
        soundManager.playSoundGameFailure();
        GameManager.gameEnd = true;
        fpscon.canMove = false;
        mainCanvas.SetActive(false);
        gameOverCanvas.SetActive(true);
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



    string getResultEvaluation(float ratio)
    {
        if(ratio >99.9)
        {
            return "•]‰¿:S";
        }
        else if (ratio > 80)
        {
            return "•]‰¿:A";
        }
        else if( ratio > 50)
        {
            return "•]‰¿:B";
        }
        else
        {
            return "•]‰¿:C";
        }
    } 

}

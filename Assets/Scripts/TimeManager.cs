using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;


/// <summary>
/// timerを管理するクラス。
/// 外部からインスタンスを作成して使用する。
/// 利用時には、timerReset()で必ずタイマーを初期化する。
/// timerStart()でタイマーを開始し、timerStop()で一時停止する。
/// </summary>
public class TimeManager : MonoBehaviour
{
    [SerializeField]float timeLimit;
    [SerializeField] float timeLeft;
    bool isStarting;
    bool timerEnd;
    [SerializeField]TextMeshProUGUI text;
    [SerializeField] bool autoBegin;
    [SerializeField]  UIManager uiManager;

    
    
    // Start is called before the first frame update
    void Start()
    {
        if (autoBegin)
        {
        
            timerReset();
            timerStart();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(isStarting)
        {
            timeUpdate();
            textUpdate();
        }
        if(checkTimer())
        {
            timerEndEvent();
        }
    }

    /// <summary>
    /// タイマーが終了した時の処理。
    /// </summary>
    public void timerEndEvent()
    {
        text.text = "Timer End";
        uiManager.Failure();
    }

    public void timeUpdate()
    {
        if (timeLeft < 0) return;
        timeLeft -= Time.deltaTime;
    }

    public void textUpdate()
    {
        text.text = String.Format("制限時間:{0:#.##}", timeLeft);
    }

    /// <summary>
    /// checkTimer()はisStartingがtrueの時以外には、falseを返すようにする。
    /// </summary>
    /// <returns></returns>
    public bool checkTimer()
    {
        if (!isStarting) return false;
        if(timeLeft<0)
        {
            timeLeft = 0f;
            timerEnd = true;
            return true;
        }
        return false;
    }

    public void timerReset()
    {
        timeLeft = timeLimit;
        isStarting = false;
        timerEnd = false;
    }

    public void timeLimitSet(float time)
    {
        timeLimit = time;
    }

    public float getLeftTime()
    {
        return timeLeft;
    }

    public float getTimeLimit()
    {
        return timeLimit;
    }

    public void timerStart()
    {
        isStarting = true;
    }

    public void timerStop()
    {
        isStarting = false;
    }

   
}

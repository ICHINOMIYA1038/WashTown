using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//実績を判断し通知を送るスクリプト
public class Achievements : MonoBehaviour, IObserver<int>
{
    [SerializeField] AchievePanelGenerator achievePanelGenerator;
    static Observable observable;
    public void Start()
    {
        observable = new Observable();
        IDisposable disposableA = observable.Subscribe(this);
        //SendNotice(1);
        //observable.SendNoticeSample();
        //disposableA.Dispose();
        //observable.SendNoticeSample();
    }

  

    private static Achievements _instance;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void SendNotice(int id)
    {
        observable.SendNotice(id);
    }

    public static Achievements Instance
    {
        get { return _instance; }
    }


    #region Iobserver
    /// <summary>
    /// Iobserverの実装部分
    /// </summary>
    void IObserver<int>.OnCompleted()
    {
        Debug.Log("通知の受け取りを完了しました。");
    }

    void IObserver<int>.OnError(Exception error)
    {
        Debug.Log("エラーを受信しました。");
    }

    void IObserver<int>.OnNext(int value)
    {
        achievePanelGenerator.receiveNotification(value);
    }
    #endregion

    #region Achivementsシステム

    


    #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Achievements : MonoBehaviour, IObserver<int>
{
    [SerializeField] AchievePanelGenerator achievePanelGenerator;
    public void Awake()
    {
        Observable observable = new Observable();
        IDisposable disposableA = observable.Subscribe(this);
        observable.SendNotice();
        disposableA.Dispose();
        observable.SendNotice();
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
        achievePanelGenerator.execute();
    }
    #endregion

    #region Achivementsシステム

    


    #endregion
}

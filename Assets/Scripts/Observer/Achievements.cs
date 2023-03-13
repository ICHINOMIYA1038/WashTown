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
    /// Iobserver�̎�������
    /// </summary>
    void IObserver<int>.OnCompleted()
    {
        Debug.Log("�ʒm�̎󂯎����������܂����B");
    }

    void IObserver<int>.OnError(Exception error)
    {
        Debug.Log("�G���[����M���܂����B");
    }

    void IObserver<int>.OnNext(int value)
    {
        achievePanelGenerator.receiveNotification(value);
        achievePanelGenerator.execute();
    }
    #endregion

    #region Achivements�V�X�e��

    


    #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//���т𔻒f���ʒm�𑗂�X�N���v�g
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
    }
    #endregion

    #region Achivements�V�X�e��

    


    #endregion
}

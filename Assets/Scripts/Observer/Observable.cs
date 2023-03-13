using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// C#標準のSystemから作成したオブザーバーデザインパターン
/// 参考記事: https://qiita.com/yutorisan/items/6e960426da71b7e02af7
/// </summary>
public class Observable : MonoBehaviour, IObservable<int>
{
    //購読されたIObserver<int>のリスト
    private List<IObserver<int>> m_observers = new List<IObserver<int>>();

    public IDisposable Subscribe(IObserver<int> observer)
    {
        if (!m_observers.Contains(observer))
            m_observers.Add(observer);
        return new Unsubscriber(m_observers, observer);
    }

    public void SendNotice()
    {
        //すべての発行先に対して1,2,3を発行する
        foreach (var observer in m_observers)
        {
            observer.OnNext(1);
            observer.OnNext(2);
            observer.OnNext(3);
            observer.OnCompleted();
        }
    }

    public class Unsubscriber:IDisposable
    {
        //発行先リスト
        private List<IObserver<int>> m_observers;
        //DisposeされたときにRemoveするIObserver<int>
        private IObserver<int> m_observer;

        public Unsubscriber(List<IObserver<int>> observers, IObserver<int> observer)
        {
            m_observers = observers;
            m_observer = observer;
        }

        public void Dispose()
        {
            //Disposeされたら発行先リストから対象の発行先を削除する
            m_observers.Remove(m_observer);
        }
    }


}


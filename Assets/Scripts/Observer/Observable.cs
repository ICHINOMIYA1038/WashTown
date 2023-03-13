using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// C#�W����System����쐬�����I�u�U�[�o�[�f�U�C���p�^�[��
/// �Q�l�L��: https://qiita.com/yutorisan/items/6e960426da71b7e02af7
/// </summary>
public class Observable : MonoBehaviour, IObservable<int>
{
    //�w�ǂ��ꂽIObserver<int>�̃��X�g
    private List<IObserver<int>> m_observers = new List<IObserver<int>>();

    public IDisposable Subscribe(IObserver<int> observer)
    {
        if (!m_observers.Contains(observer))
            m_observers.Add(observer);
        return new Unsubscriber(m_observers, observer);
    }

    public void SendNotice()
    {
        //���ׂĂ̔��s��ɑ΂���1,2,3�𔭍s����
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
        //���s�惊�X�g
        private List<IObserver<int>> m_observers;
        //Dispose���ꂽ�Ƃ���Remove����IObserver<int>
        private IObserver<int> m_observer;

        public Unsubscriber(List<IObserver<int>> observers, IObserver<int> observer)
        {
            m_observers = observers;
            m_observer = observer;
        }

        public void Dispose()
        {
            //Dispose���ꂽ�甭�s�惊�X�g����Ώۂ̔��s����폜����
            m_observers.Remove(m_observer);
        }
    }


}



using System.Threading.Tasks;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    async void Start()
    {
        // �񓯊����������s
        await SomeAsyncMethod();

        // �񓯊�����������������ɓ������������s
        SomeSyncMethod();
    }

    async Task SomeAsyncMethod()
    {
        // �񓯊������̗�Ƃ��āA1�b�ԑҋ@����
        await Task.Delay(10000);
    }

    void SomeSyncMethod()
    {
        // ���������̗�Ƃ��āADebug.Log�����s����
        Debug.Log("�������������s����܂���");
    }
}


using System.Threading.Tasks;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    async void Start()
    {
        // 非同期処理を実行
        await SomeAsyncMethod();

        // 非同期処理が完了した後に同期処理を実行
        SomeSyncMethod();
    }

    async Task SomeAsyncMethod()
    {
        // 非同期処理の例として、1秒間待機する
        await Task.Delay(10000);
    }

    void SomeSyncMethod()
    {
        // 同期処理の例として、Debug.Logを実行する
        Debug.Log("同期処理が実行されました");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogStarter : MonoBehaviour
{
    [SerializeField]NPCConversation nPCConversation;
    [SerializeField] GameObject player;
    //rangeより近い距離に入ったら会話を始めることができる。
    [SerializeField] float range;
    // Start is called before the first frame update
    private void Update()
    {
        if((player.transform.position - this.transform.position).magnitude>range)
        {
            //Debug.Log((player.transform.position - this.transform.position).magnitude);
            return;
        }
        if(Input.GetKeyDown(KeyCode.E))
        {
            if (player!= null)
            {
                // プレイヤーの位置とNPCの位置の差分を計算
                Vector3 direction = player.transform.position - transform.position;

                // y軸の角度を計算して、Quaternionで回転する
                float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, angle, 0);
            }
            nPCConversation.StartConversation();
        }
        
    }
}

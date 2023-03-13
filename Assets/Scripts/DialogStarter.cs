using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogStarter : MonoBehaviour
{
    [SerializeField]NPCConversation nPCConversation;
    [SerializeField] GameObject player;
    //range���߂������ɓ��������b���n�߂邱�Ƃ��ł���B
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
                // �v���C���[�̈ʒu��NPC�̈ʒu�̍������v�Z
                Vector3 direction = player.transform.position - transform.position;

                // y���̊p�x���v�Z���āAQuaternion�ŉ�]����
                float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, angle, 0);
            }
            nPCConversation.StartConversation();
        }
        
    }
}

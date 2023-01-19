using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class CountDown : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI text;
    [SerializeField]
    int timer;
    int remainTime;
    // Start is called before the first frame update
    void Start()
    {
        text.text = ""+timer;
        remainTime = timer;
        StartCoroutine("Count");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Count()
    {
        while (remainTime > 0)
        {
            yield return new WaitForSeconds(1);
            remainTime -= 1;
            text.text = "" + remainTime;
        }
    }
}

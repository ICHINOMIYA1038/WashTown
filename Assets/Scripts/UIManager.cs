using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    [SerializeField] WashableObject[] washableObjects;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] progressGauge progress;
    int allPixel = 0;
    int dirtyCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        calcAllPixel();

    }

    // Update is called once per frame
    void Update()
    {
        calcAllPixel();
        calcRatio();
    }

    void calcAllPixel()
    {
        int allCount = 0;
        int dirtyNum = 0;
        foreach (var elem in washableObjects)
        {
            allCount += elem.getAllNum();
            dirtyNum += elem.getDirtyCount();
        }
        allPixel = allCount;
        dirtyCount = dirtyNum;
    }

    void calcRatio()
    {
        float ratio = 100*(float)(allPixel - dirtyCount) / (float)allPixel;
        text.text = String.Format("{0:#.##}%", ratio);
        progress.setRatio(ratio / 100);
    }
}

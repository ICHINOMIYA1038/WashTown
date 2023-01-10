using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] WashableObject[] washableObjects;
    [SerializeField] TextMeshProUGUI text;
    int allPixel = 0;
    int dirtyCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        
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
        Debug.Log(allPixel);
        Debug.Log(dirtyNum);
    }

    void calcRatio()
    {
        float ratio = (allPixel- dirtyCount )/allPixel;
        text.SetText(""+ratio + "%");
    }
}

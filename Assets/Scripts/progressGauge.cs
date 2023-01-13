using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class progressGauge : MonoBehaviour
{
    [SerializeField]Image image;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setRatio(float ratio)
    {
        image.fillAmount = ratio;
    }
}

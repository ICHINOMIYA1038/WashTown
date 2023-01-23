using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarGenerator : MonoBehaviour
{
    [SerializeField] Texture2D starImage;
    [SerializeField] Texture2D blankStarImage;
    [SerializeField] int starNum=0;
    RawImage[] images;

    // Start is called before the first frame update
    void Start()
    {
        images = this.transform.GetComponentsInChildren<RawImage>();
        
        foreach(var elem in images)
        {
            elem.texture = blankStarImage;
        }
        setStar();
    }

    public void setStar()
    {
        for(int i=0; i < starNum; i++)
        {
            images[i].texture = starImage;
        }
        for(int i=starNum;i<5;i++)
        {
            images[i].texture = blankStarImage;
        }
    }
}

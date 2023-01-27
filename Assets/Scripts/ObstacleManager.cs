using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ObstacleManager : MonoBehaviour
{
    [SerializeField] ObstacleObject[] obstacles;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        randomUpdate();
    }

    void randomUpdate()
    {
        System.Random rand = new System.Random();
        foreach (var elem in obstacles)
        {
            if (!elem.isLaunching)
            {
                if (rand.Next(0, 100000) < 1)
                {
                    elem.Enable();
                    Debug.Log("tomato");
                }
               
            }
            
        }
    }
}

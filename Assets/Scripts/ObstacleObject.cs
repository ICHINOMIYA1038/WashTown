using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleObject : MonoBehaviour
{
    public bool isLaunching = false;
    protected Vector3 initPosi;
    protected Vector3 initVelocity;

    
    
    virtual public void Launch()
    {

    }

    public void Enable()
    {
        isLaunching = true;
    }

    virtual public void respwan()
    {
        this.transform.position = initPosi;
        
        isLaunching = false;

    }
}

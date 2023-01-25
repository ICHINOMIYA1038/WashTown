using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Effect : MonoBehaviour
{
    [SerializeField] VisualEffect effect;
    Vector3 position;
    Vector3 normal;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            effect.SendEvent("OnStop");
            //StopPlayはEvent Nameで指定した任意の名称
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            effect.SendEvent("OnPlay");
            //OnPlayはEvent Nameで指定した任意の名称
            effect.SetVector3("position", position);
            effect.SetVector3("normal", normal);
            Ray ray = new Ray(this.transform.position, this.transform.forward);
            ray.
        }
    }
}

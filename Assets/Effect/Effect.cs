using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Effect : MonoBehaviour
{
    [SerializeField] VisualEffect effect;
    Vector3 position;
    Vector3 normal;
    Vector3 dir;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        dir = this.transform.forward;

        if (Input.GetMouseButtonUp(0))
        {
            effect.SendEvent("OnStop");
            //StopPlay??Event Name???w???????C????????
        }
        if (Input.GetMouseButton(0))
        {
            effect.SendEvent("OnPlay");
            //OnPlay??Event Name???w???????C????????
            effect.SetVector3("position", position);
            effect.SetVector3("normal", normal);
            effect.SetVector3("dir", dir);
            Ray ray = new Ray(this.transform.position, this.transform.forward);
        }
    }
}

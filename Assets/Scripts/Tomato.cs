using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tomato : ObstacleObject
{
    [SerializeField]Texture flattenTexture;
    [SerializeField]float blushScale;
    [SerializeField] Vector3 velocity;
    [SerializeField] Vector3 acceleration;

    public void Start()
    {
        initPosi = transform.position;
        initVelocity = velocity;
        // flattenTexture = util.CreateTempTexture(256,256);
    }
    // Update is called once per frame
    void Update()
    {
        if (isLaunching)
        {
            Launch();
        }
    }

    void thrown()
    {
        RaycastHit hit;
        Ray ray = new Ray(this.transform.position + this.transform.forward * 1f, this.transform.forward);
        if (Physics.Raycast(ray, out hit, 2))
        {

            Debug.DrawLine(ray.origin, hit.point, Color.blue, 5f);
            if (hit.transform.gameObject.tag == "washable")
            {
                WashableObject washableObject = hit.transform.gameObject.GetComponent<WashableObject>();
                Vector2 hitPosi = hit.textureCoord;
                washableObject.changeTexture(hitPosi, blushScale, flattenTexture, new Color(0f, 1f, 0f, 0.5f));
            }
        }
    }

    override public void Launch()
    {
        velocity += acceleration * Time.deltaTime;
        transform.position += velocity * Time.deltaTime;
        thrown();
    }

    override public void respwan()
    {
        this.transform.position = initPosi;
        velocity = initVelocity;
        isLaunching = false;

    }

    private void OnCollisionEnter(Collision collision)
    {
        respwan();
    }



}

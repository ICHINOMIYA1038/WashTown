using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tomato : MonoBehaviour
{
    [SerializeField]Texture flattenTexture;
    [SerializeField]float blushScale;
    [SerializeField] Vector3 velocity;
    [SerializeField] Vector3 acceleration;
    // Start is called before the first frame update
    void Start()
    {
       // flattenTexture = util.CreateTempTexture(256,256);
    }

    // Update is called once per frame
    void Update()
    {
        velocity += acceleration*Time.deltaTime;
        transform.position += velocity*Time.deltaTime;
        thrown();
    }

    void thrown()
    {
        RaycastHit hit;
        Ray ray = new Ray(this.transform.position + this.transform.forward * 1f, this.transform.forward);
        if (Physics.Raycast(ray, out hit, 2))
        {

            Debug.Log("hit");
            Debug.DrawLine(ray.origin, hit.point, Color.blue, 5f);
            Debug.Log(hit.transform.gameObject.name);
            if (hit.transform.gameObject.tag == "washable")
            {
                WashableObject washableObject = hit.transform.gameObject.GetComponent<WashableObject>();
                Vector2 hitPosi = hit.textureCoord;
                washableObject.changeTexture(hitPosi, blushScale, flattenTexture, new Color(0f, 1f, 0f, 0.5f));
                Debug.Log("execute");
                Debug.Log(hitPosi);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(this.gameObject);
    }



}

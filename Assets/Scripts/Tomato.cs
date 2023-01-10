using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tomato : MonoBehaviour
{
    Texture2D flattenTexture;
    float blushScale;
    [SerializeField] Vector3 velocity;
    [SerializeField]Vector3 acceleration;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        velocity += acceleration*Time.deltaTime;
        transform.position += velocity*Time.deltaTime;
    }

    void thrown()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        RaycastHit hit;
        Ray ray = new Ray(this.transform.position - this.transform.forward * 3f, this.transform.forward);
        if (Physics.Raycast(ray, out hit, 10))
        {
            Debug.Log("hit");
            Debug.DrawLine(ray.origin, hit.point, Color.blue, 5f);
            if (hit.transform.gameObject.tag == "washable")
            {
                WashableObject washableObject = hit.transform.gameObject.GetComponent<WashableObject>();
                Vector2 hitPosi = hit.textureCoord;
                washableObject.changeTexture(hitPosi, blushScale * 10f, flattenTexture, new Color(0f, 0f, 0f, 0.5f));
            }
        }
    }

}

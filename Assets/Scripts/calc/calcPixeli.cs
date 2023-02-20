using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class calcPixeli : MonoBehaviour
{
    Mesh mesh;
    Texture texture;
    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;
        Vector2[] uvs = mesh.uv;
        int[] tris = mesh.triangles;
        Debug.Log(tris.Length/3);

    }

    // Update is called once per frame
    void Update()
    {
       
    }

    void calcTexture(Texture texture,Vector2 uv)
    {
        
    }
}

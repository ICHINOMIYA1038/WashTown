using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMapping : MonoBehaviour
{

    [SerializeField] private Material[] materials;

    void Awake()
    {
        float size = 1f;
        Vector2[] uv = new Vector2[24];

        // back
        uv[2].x = 0.0f; uv[2].y = 1.0f;
        uv[3].x = 1.0f; uv[3].y = 1.0f;
        uv[0].x = 0.0f; uv[0].y = 0.0f;
        uv[1].x = 1.0f; uv[1].y = 0.0f;

        // top
        uv[9].x = 0.0f; uv[9].y = 0.0f;
        uv[5].x = 1.0f; uv[5].y = 0.0f;
        uv[8].x = 0.0f; uv[8].y = 1.0f;
        uv[4].x = 1.0f; uv[4].y = 1.0f;

        // right
        uv[23].x = 1.0f; uv[23].y = 0.0f;
        uv[21].x = 0.0f; uv[21].y = 1.0f;
        uv[20].x = 0.0f; uv[20].y = 0.0f;
        uv[22].x = 1.0f; uv[22].y = 1.0f;

        // left
        uv[19].x = 1.0f; uv[19].y = 0.0f;
        uv[17].x = 0.0f; uv[17].y = 1.0f;
        uv[16].x = 0.0f; uv[16].y = 0.0f;
        uv[18].x = 1.0f; uv[18].y = 1.0f;

        // bottom
        uv[12].x = 1.0f; uv[12].y = 0.0f;
        uv[14].x = 0.0f; uv[14].y = 1.0f;
        uv[13].x = 0.0f; uv[13].y = 0.0f;
        uv[15].x = 1.0f; uv[15].y = 1.0f;

        // front
        uv[6].x = 1.0f; uv[6].y = 0.0f;
        uv[7].x = 0.0f; uv[7].y = 0.0f;
        uv[10].x = 1.0f; uv[10].y = 1.0f;
        uv[11].x = 0.0f; uv[11].y = 1.0f;


        int[] back = {
            0, 2, 1,
            1, 2, 3
        };
        int[] front = {
            7, 11, 6,
            6, 11, 10
        };

        int[] left = {
            16, 17, 19,
            19, 17, 18
        };

        int[] right = {
            20, 21, 23,
            23, 21, 22
        };

        int[] top = {
            8, 4, 9,
            9, 4, 5
        };

        int[] bottom = {
            12, 13, 15,
            15, 13, 14
        };

        Vector3[] vertices = {
            new Vector3(size/2, -size/2, size/2),
            new Vector3(-size/2, -size/2, size/2),
            new Vector3(size/2, size/2, size/2),
            new Vector3(-size/2, size/2, size/2),
            new Vector3(size/2, size/2, -size/2),
            new Vector3(-size/2, size/2, -size/2),
            new Vector3(size/2, -size/2, -size/2),
            new Vector3(-size/2, -size/2, -size/2),
            new Vector3(size/2, size/2, size/2),
            new Vector3(-size/2, size/2, size/2),
            new Vector3(size/2, size/2, -size/2),
            new Vector3(-size/2, size/2, -size/2),
            new Vector3(size/2, -size/2, -size/2),
            new Vector3(size/2, -size/2, size/2),
            new Vector3(-size/2, -size/2, size/2),
            new Vector3(-size/2, -size/2, -size/2),
            new Vector3(-size/2, -size/2, size/2),
            new Vector3(-size/2, size/2, size/2),
            new Vector3(-size/2, size/2, -size/2),
            new Vector3(-size/2, -size/2, -size/2),
            new Vector3(size/2, -size/2, -size/2),
            new Vector3(size/2, size/2, -size/2),
            new Vector3(size/2, size/2, size/2),
            new Vector3(size/2, -size/2, size/2)
        };

        MeshFilter mf = gameObject.AddComponent<MeshFilter>() as MeshFilter;
        MeshRenderer renderer = gameObject.AddComponent<MeshRenderer>() as MeshRenderer;
        Material[] mats = renderer.materials;

        mats = materials;
        GetComponent<Renderer>().materials = mats;

        mf.mesh.vertices = vertices;
        mf.mesh.uv = uv;

        mf.mesh.subMeshCount = 6;
        mf.mesh.SetTriangles(front, 0);
        mf.mesh.SetTriangles(back, 1);
        mf.mesh.SetTriangles(right, 2);
        mf.mesh.SetTriangles(left, 3);
        mf.mesh.SetTriangles(top, 4);
        mf.mesh.SetTriangles(bottom, 5);

        // メッシュデータを最適化
        mf.mesh.Optimize();
        // メッシュの法線を更新
        mf.mesh.RecalculateNormals();
    }
}
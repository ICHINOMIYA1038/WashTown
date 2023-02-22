using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TexturePixelCounter : MonoBehaviour
{
    // �e�X�g�p�̎O�p�`���b�V��
     Mesh testMesh;

    // �e�X�g�p�̃}�e���A���i�e�N�X�`�����܂�)
    public Material testMaterial;

    void Start()
    {
        testMesh = GetComponent<MeshFilter>().mesh;
        // ���b�V���̒��_�f�[�^���擾
        //Vector3[] vertices = testMesh.vertices;
        Vector2[] uvs = testMesh.uv;

        // �e�N�X�`���T�C�Y���擾
        Texture2D texture = (Texture2D)testMaterial.mainTexture;
        int textureWidth = texture.width;
        int textureHeight = texture.height;

        // ���ۂɃe�N�X�`���Ŏg�p������f�����Z�o
        Debug.Log(testMesh.triangles.Length);
        
        /*
        for (int i = 0; i < testMesh.triangles.Length; i += 3)
        {
            // �O�p�`��3�̒��_�ɂ��āA�e�N�X�`����ł̍��W�𐳋K��
            Vector2 uv1 = new Vector2(uvs[testMesh.triangles[i]].x % 1, uvs[testMesh.triangles[i]].y % 1);
            Vector2 uv2 = new Vector2(uvs[testMesh.triangles[i + 1]].x % 1, uvs[testMesh.triangles[i + 1]].y % 1);
            Vector2 uv3 = new Vector2(uvs[testMesh.triangles[i + 2]].x % 1, uvs[testMesh.triangles[i + 2]].y % 1);

            // �e���_��uv���W�Ƀe�N�X�`���T�C�Y���|���āA���ۂ̃s�N�Z�����W�ɕϊ�
            Vector2 pixelUV1 = new Vector2(uv1.x * textureWidth, uv1.y * textureHeight);
            Vector2 pixelUV2 = new Vector2(uv2.x * textureWidth, uv2.y * textureHeight);
            Vector2 pixelUV3 = new Vector2(uv3.x * textureWidth, uv3.y * textureHeight);
            int result = CountPixelsInTriangle(texture, pixelUV1, pixelUV2, pixelUV3);
            Debug.Log(result);
        }
        */
        
    }

    public static int CountPixelsInTriangle(Texture2D texture, Vector2 uv1, Vector2 uv2, Vector2 uv3)
    {
        // �e�N�X�`���̏��
        int width = texture.width;
        int height = texture.height;

        // �v�Z���ȗ������邽�߂ɁA��`��p�ӂ���B
        float minX = Mathf.Min(uv1.x, Mathf.Min(uv2.x, uv3.x));
        float maxX = Mathf.Max(uv1.x, Mathf.Max(uv2.x, uv3.x));
        float minY = Mathf.Min(uv1.y, Mathf.Min(uv2.y, uv3.y));
        float maxY = Mathf.Max(uv1.y, Mathf.Max(uv2.y, uv3.y));

        //��`�̃f�[�^���擾
        int x1 = Mathf.FloorToInt(minX * width);
        int x2 = Mathf.CeilToInt(maxX * width);
        int y1 = Mathf.FloorToInt(minY * height);
        int y2 = Mathf.CeilToInt(maxY * height);

        // �s�N�Z����
        int pixelCount = 0;

        // ���ۂ̌v�Z�ꏊ
        for (int y = y1; y < y2; y++)
        {
            for (int x = x1; x < x2; x++)
            {
                // ���W��uv�l�ɕϊ�
                float u = (float)x / width;
                float v = (float)y / height;

                // �O�p�`�̒��ɂ��邩�𔻒�
                //if (IsPointInTriangle(u, v, uv1, uv2, uv3))
                {
                    // Get pixel color
                    Color color = texture.GetPixel(x, y);

                    // Check if pixel is not transparent
                    if (color.a > 0)
                    {
                        
                    }
                    pixelCount++;
                }
            }
        }

        return pixelCount;
    }

    /// <summary>
    /// �O�p�`�̂Ȃ��ɂ��邩�ǂ����𔻒�
    /// </summary>
    /// <param name="u"></param>
    /// <param name="v"></param>
    /// <param name="uv1"></param>
    /// <param name="uv2"></param>
    /// <param name="uv3"></param>
    /// <returns></returns>
    private static bool IsPointInTriangle(float u, float v, Vector2 uv1, Vector2 uv2, Vector2 uv3)
    {
        float detT = (uv2.y - uv3.y) * (uv1.x - uv3.x) + (uv3.x - uv2.x) * (uv1.y - uv3.y);
        float lambda1 = ((uv2.y - uv3.y) * (u - uv3.x) + (uv3.x - uv2.x) * (v - uv3.y)) / detT;
        float lambda2 = ((uv3.y - uv1.y) * (u - uv3.x) + (uv1.x - uv3.x) * (v - uv3.y)) / detT;
        float lambda3 = 1 - lambda1 - lambda2;
        return lambda1 >= 0 && lambda1 <= 1 && lambda2 >= 0 && lambda2 <= 1 && lambda3 >= 0 && lambda3 <= 1;
    }
}


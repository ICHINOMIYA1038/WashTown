using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class util : MonoBehaviour
{
    
    public static  Texture2D CreateTempTexture(int width, int height, Color defaultColor = default)
    {
        var texture = new Texture2D(width, height, TextureFormat.RGBA32, false);

        for (int y = 0; y < texture.height; y++)
            for (int x = 0; x < texture.width; x++)
                texture.SetPixel(x, y, defaultColor);
        return texture;
    }
}

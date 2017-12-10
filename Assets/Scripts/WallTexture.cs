using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallTexture : MonoBehaviour
{
    int width = 20;
    int height = 20;

    int gridSize = 2;

    void Start()
    {
        Renderer rend = GetComponent<Renderer>();
        //rend.material.color = Color.green;

        Texture2D tex = new Texture2D(width, height);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Color col;
                if (x % gridSize == 0 || y % gridSize == 0) col = Color.black;
                else
                {
                    col = new Color(Mathf.Sin(x), 1, Mathf.Cos(y));
                }

                tex.SetPixel(x, y, col);
                //tex.SetPixel(x, y, Color.cyan);
            }
        }

        tex.Apply();

        rend.material.mainTexture = tex;
        //rend.material.SetTexture("GroundTex", tex);
    }

    // Update is called once per frame
    void Update ()
    {
		
	}
}

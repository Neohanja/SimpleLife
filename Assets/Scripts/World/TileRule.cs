using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tile Data", menuName = "Simple Life/Tile Data")]
public class TileRule : ScriptableObject
{
    public static readonly int TileSize = 8;

    public string tileName;

    public Texture2D[] textures;

    public Color[] GetTextureData(byte borders)
    {
        Color[] cols = new Color[TileSize * TileSize];

        if (textures == null || textures.Length == 0) return cols;

        if (textures.Length == 1) return textures[0].GetPixels();

        bool[] edges = new bool[8];

        byte index = 0, rot = 0;

        #region Find Edges with Same Tile
        if (borders >= 128) // Top Left Corner
        {
            edges[0] = true;
            borders -= 128;
        }
        else edges[0] = false;

        if (borders >= 64) // Top
        {
            edges[1] = true;
            borders -= 64;
        }
        else edges[1] = false;

        if (borders >= 32) // Top Right Corner
        {
            edges[2] = true;
            borders -= 32;
        }
        else edges[2] = false;

        if (borders >= 16) // Left
        {
            edges[3] = true;
            borders -= 16;
        }
        else edges[3] = false;

        if (borders >= 8) // Right
        {
            edges[4] = true;
            borders -= 8;
        }
        else edges[4] = false;

        if (borders >= 4) // Bottom Left Corner
        {
            edges[5] = true;
            borders -= 4;
        }
        else edges[5] = false;

        if (borders >= 2) // Bottom
        {
            edges[6] = true;
            borders -= 2;
        }
        else edges[6] = false;

        if (borders >= 1) // Bottom Right Corner
        {
            edges[7] = true;
            borders -= 1;
        }
        else edges[7] = false;
        #endregion

        #region Map Correct Tile and Rotation
        if (edges[1] && edges[3] && edges[4] && edges[6])
        {
            int corners = edges[0] ? 1 : 0;
            corners += edges[2] ? 1 : 0;
            corners += edges[5] ? 1 : 0;
            corners += edges[7] ? 1 : 0;

            switch (corners)
            {
                case 0:
                    index = 6;
                    break;
                case 1:
                    index = 6;
                    break;
                case 2:
                    if (edges[5] && edges[7]) index = 3;
                    else if (edges[0] && edges[5]) { index = 3; rot = 1; }
                    else if (edges[0] && edges[2]) { index = 3; rot = 2; }
                    else if (edges[2] && edges[7]) { index = 3; rot = 3; }
                    else if (edges[2] && edges[5]) { index = 4; rot = 1; }
                    else index = 4;
                    break;
                case 3:
                    index = 2;
                    if (!edges[7]) rot = 1;
                    else if (!edges[5]) rot = 2;
                    else if (!edges[0]) rot = 3;
                    break;
                case 4:
                    index = 1;
                    break;
            }
        }
        else
        {
            int exits = edges[1] ? 1 : 0;
            exits += edges[3] ? 1 : 0;
            exits += edges[4] ? 1 : 0;
            exits += edges[6] ? 1 : 0;

            switch (exits)
            {
                case 3:
                    if (!edges[6])
                    {
                        if (edges[0] && edges[2])
                        {
                            index = 11;
                            rot = 2;
                        }
                        else if (edges[0])
                        {
                            index = 12;
                            rot = 2;
                        }
                        else if (edges[2])
                        {
                            index = 13;
                            rot = 2;
                        }
                        else
                        {
                            index = 7;
                            rot = 1;
                        }
                    }
                    else if (!edges[3])
                    {
                        if (edges[2] && edges[7])
                        {
                            index = 11;
                            rot = 3;
                        }
                        else if (edges[2])
                        {
                            index = 12;
                            rot = 3;
                        }
                        else if (edges[7])
                        {
                            index = 13;
                            rot = 3;
                        }
                        else
                        {
                            index = 7;
                            rot = 2;
                        }
                    }
                    else if (!edges[1])
                    {
                        if (edges[5] && edges[7])
                        {
                            index = 11;
                        }
                        else if(edges[7])
                        {
                            index = 12;
                        }
                        else if(edges[5])
                        {
                            index = 13;
                        }
                        else
                        {
                            index = 7;
                            rot = 3;
                        }
                    }
                    else
                    {
                        if (edges[0] && edges[5])
                        {
                            index = 11;
                            rot = 1;
                        }
                        else if (edges[5])
                        {
                            index = 12;
                            rot = 1;
                        }
                        else if (edges[0])
                        {
                            index = 13;
                            rot = 1;
                        }
                        else index = 7;
                    }
                    break;
                case 2:
                    if (edges[3] && edges[6])
                    {
                        if(edges[5]) index = 5;
                        else index = 8;
                    }
                    else if (edges[3] && edges[1])
                    {
                        if (edges[0]) index = 5;
                        else index = 8;
                        rot = 1;
                    }
                    else if (edges[1] && edges[4])
                    {
                        if (edges[2]) index = 5;
                        else index = 8;
                        rot = 2;
                    }
                    else if (edges[4] && edges[6])
                    {
                        if (edges[7]) index = 5;
                        else index = 8;
                        rot = 3;
                    }
                    else if (edges[3] && edges[4])
                    {
                        index = 9;
                        rot = 1;
                    }
                    else
                    {
                        index = 9;
                    }
                    break;
                case 1:
                    index = 10;
                    if (edges[3]) rot = 1;
                    else if (edges[1]) rot = 2;
                    else if (edges[4]) rot = 3;
                    break;
            }
        }
        #endregion

        for (int x = 0; x < TileSize; x++)
        {
            for (int y = 0; y < TileSize; y++)
            {
                Color c;

                switch(rot)
                {
                    case 0:
                        c = textures[index].GetPixel(x, y);
                        break;
                    case 1:
                        c = textures[index].GetPixel(TileSize - 1 - y, x);
                        break;
                    case 2:
                        c = textures[index].GetPixel(TileSize - 1 - x, TileSize - 1 - y);
                        break;
                    case 3:
                        c = textures[index].GetPixel(y, TileSize - 1 - x);
                        break;
                    default:
                        c = Color.magenta;
                        break;
                }

                cols[y * TileSize + x] = c;
            }
        }

        return cols;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk
{
    public GameObject chunkObj;
    SpriteRenderer chunkImage;

    int chunkHeight;
    int chunkWidth;

    TileData[,] chunkData;

    public Chunk()
    {
        chunkObj = new GameObject("Level");
        chunkImage = chunkObj.AddComponent<SpriteRenderer>();

        chunkImage.material = Map.World.tileMaterial;
        chunkHeight = 16;
        chunkWidth = 16;

        chunkData = new TileData[chunkWidth, chunkHeight];

        BuildBaseChunk(0);
        BuildMapImage();
    }

    public void BuildBaseChunk(int baseTile)
    {
        int[,] map = new int[,]
        {
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2 },
            { 0, 1, 1, 1, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2 },
            { 0, 1, 1, 1, 0, 2, 2, 0, 0, 0, 0, 0, 0, 0, 2, 0 },
            { 0, 1, 1, 1, 2, 2, 0, 0, 0, 0, 2, 2, 0, 2, 2, 0 },
            { 0, 0, 1, 0, 0, 2, 0, 0, 2, 2, 2, 2, 2, 2, 0, 0 },
            { 0, 0, 1, 0, 1, 0, 0, 0, 1, 0, 2, 2, 0, 0, 0, 0 },
            { 0, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0 },
            { 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 0, 0, 0, 0 },
            { 0, 2, 2, 0, 0, 0, 0, 0, 0, 0, 2, 2, 0, 0, 0, 0 },
            { 0, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        };


        for (int y = 0; y < chunkHeight; y++)
        {
            int row = chunkHeight - y - 1;
            for (int col = 0; col < chunkWidth; col++)
            {
                byte borders = 0, amp = 1;
                for(int i = 0; i < 8; i++)
                {
                    int cX = col + Directionals[i].x;
                    int cY = row + Directionals[i].y;

                    if(cX < 0 || cY < 0 || cX >= chunkWidth || cY >= chunkHeight)
                    {
                        borders += amp;
                    }
                    else
                    {
                        if (map[row, col] == map[cY, cX]) borders += amp;
                    }

                    amp *= 2;
                }

                chunkData[col, y] = new TileData(map[row, col], 0, borders);
            }
        }
    }



    public void BuildMapImage()
    {

        Texture2D sprite = new Texture2D(LevelWidth, LevelHeight);
        Color[] cols = new Color[LevelWidth * LevelHeight];

        for (int y = 0; y < chunkHeight; y++)
        {
            for (int x = 0; x < chunkWidth; x++)
            {
                Color[] c = GetTile(x, y);
                
                for (int h = 0; h < TileSize; h++)
                {
                    for (int w = 0; w < TileSize; w++)
                    {
                        int xPos = x * TileSize + w;
                        int yPos = y * TileSize + h;

                        cols[yPos * LevelWidth + xPos] = c[h * TileSize + w];
                    }
                }
            }
        }

        sprite.SetPixels(cols);
        sprite.filterMode = FilterMode.Point;
        sprite.wrapMode = TextureWrapMode.Clamp;
        sprite.Apply();

        chunkImage.sortingOrder = -1;
        chunkImage.sprite = Sprite.Create(sprite, new Rect(0, 0, LevelWidth, LevelHeight),
            new Vector2(0.5f, 0.5f), TileSize);
    }

    Color[] GetTile(int x, int y)
    {
        if (x < 0 || x >= chunkWidth || y < 0 || y >= chunkHeight) return new Color[TileSize * TileSize];

        return Map.World.autoTiles[chunkData[x, y].tile].GetTextureData(chunkData[x, y].edges);
    }

    static int TileSize { get { return TileRule.TileSize; } }

    public int LevelWidth { get { return TileSize * chunkWidth; } }
    public int LevelHeight { get { return TileSize * chunkHeight; } }

    static byte[] Amp = new byte[] { 1, 2, 4, 8, 16, 32, 64, 128 };

    static readonly Vector2Int[] Directionals = new Vector2Int[]
    {
        new Vector2Int( 1,  1),
        new Vector2Int( 0,  1),
        new Vector2Int(-1,  1),
        new Vector2Int( 1,  0),
        new Vector2Int(-1,  0),
        new Vector2Int( 1, -1),
        new Vector2Int( 0, -1),
        new Vector2Int(-1, -1),
    };
}

[System.Serializable]
public class TileData
{
    public int tile;
    public int rotation;
    public byte edges;

    public TileData(int tileID, int rot, byte borders)
    {
        tile = tileID;
        rotation = rot % 4;
        edges = borders;
    }
}
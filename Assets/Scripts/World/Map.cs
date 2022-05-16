using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public static Map World { private set; get; }
    public static Chunk CurrentMap { private set; get; }

    [Header("Game Data")]
    public Texture2D[] tiles;
    public TileRule[] autoTiles;
    public Material tileMaterial;

    void Awake()
    {
        if (World != null && World != this) Destroy(gameObject);
        else World = this;
    }

    void Start()
    {
        CurrentMap = new Chunk();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

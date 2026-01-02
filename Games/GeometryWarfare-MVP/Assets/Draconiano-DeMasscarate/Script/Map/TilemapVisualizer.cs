using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapVisualizer : MonoBehaviour
{
    [SerializeField]
    private Tilemap floorTileMap, wallTileMap;
    
    [SerializeField]
    private TileBase floorTile, wallTop;

   
    
    public void PointFoorTiles(IEnumerable<Vector2Int> floorPosition)
    {
        PaintTiles(floorPosition,floorTileMap, floorTile);
    }

    private void PaintTiles(IEnumerable<Vector2Int> positions, Tilemap tilemap, TileBase tile)
    {
        foreach(var position in positions)
        {
            PaintSingleTile(tilemap, tile, position);
        }
    }

    private void PaintSingleTile(Tilemap tilemap, TileBase tile, Vector2Int position)
    {
        var tilePosition = tilemap.WorldToCell((Vector3Int)position);
        tilemap.SetTile(tilePosition, tile);
    }

    public void Clear()
    {
        floorTileMap.ClearAllTiles();
        wallTileMap.ClearAllTiles();
    }

    internal void PaintSignleBasicWall(Vector2Int position)
    {
        PaintSingleTile(wallTileMap, wallTop, position);
    }
}

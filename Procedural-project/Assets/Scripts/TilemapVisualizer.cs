using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapVisualizer : MonoBehaviour
{
    [SerializeField]
    private Tilemap floorTilemap,wallTileMap;
    [SerializeField]
    private TileBase floorTile,corridorTile,wallTop;

    public GameObject[] enemyToSpawn;

    public bool playerSpawned;
    public int tilesPainted;

    public void PaintFloorTiles(IEnumerable<Vector2Int> floorPositions)
    {
        PaintFloorTiles(floorPositions, floorTilemap, floorTile, true);
    }

    public void PaintCorridorFloorTiles(IEnumerable<Vector2Int> floorPositions)
    {
        PaintFloorTiles(floorPositions, floorTilemap, floorTile,false);
    }

    private void PaintFloorTiles(IEnumerable<Vector2Int> positions, Tilemap tilemap, TileBase tile, bool Spawn)
    {
        foreach (var position in positions)
        {
            if (Spawn)
            {
                PaintSingleTilePlusSpawn(tilemap, tile, position);
            }
            else
            {
                PaintSingleTile(tilemap, tile, position);
            }
            
        }
    }

    internal void PaintSingleBasicWall(Vector2Int position)
    {
        PaintSingleTile(wallTileMap, wallTop, position);
    }

    private void PaintSingleTile(Tilemap tilemap, TileBase tile, Vector2Int position)
    {
        var tilePosition = tilemap.WorldToCell((Vector3Int)position);
        tilemap.SetTile(tilePosition, tile);
    }

    private void PaintSingleTilePlusSpawn(Tilemap tilemap, TileBase tile, Vector2Int position)
    {
        tilesPainted++;

        var tilePosition = tilemap.WorldToCell((Vector3Int)position);
        tilemap.SetTile(tilePosition, tile);

        if (tilesPainted <= 2000)
        {
            int randomRoomAssingment = UnityEngine.Random.Range(1, 100);
            if (randomRoomAssingment == 1)
            {
                int wichEnemy = UnityEngine.Random.Range(0, enemyToSpawn.Length);
                Instantiate(enemyToSpawn[wichEnemy], new Vector3(position.x, position.y, 0), Quaternion.identity);
            }
        }


    }

    public void Clear()
    {
        tilesPainted = 0;
        floorTilemap.ClearAllTiles();
        wallTileMap.ClearAllTiles();
    }
}

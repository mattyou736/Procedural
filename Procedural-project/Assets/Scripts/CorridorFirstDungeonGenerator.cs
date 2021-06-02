using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class CorridorFirstDungeonGenerator : SimpleRandomWalkDungeonGenerator
{
    [SerializeField]
    int corridorLength = 14, corridorCount = 5;

    [SerializeField]
    [Range(0.1f, 1)]
    float roomPercent = 0.8f;

    public GameObject[] enemyToSpawn;
    public GameObject player,destroyer,key,chest,boss;
    bool keyExists, chestExists;
    bool playerSpawned;

    GameObject[] toDestroy;

    public List <Vector2> emtyRoomPositions;
    int roomspawned;

    public GameObject cam;
   
    protected override void RunProceduralGeneration()
    {
        CorridorFirstGeneration();
    }

    public void CorridorFirstGeneration()
    {
        //reset all things
        tilemapVisualizer.Clear();
        emtyRoomPositions.Clear();
        roomspawned = 0;
        toDestroy = GameObject.FindGameObjectsWithTag("Spawnable");
        playerSpawned = false;
        keyExists = false;
        chestExists = false;
        if(toDestroy != null)
        {
            for (var i = 0; i < toDestroy.Length; i++)
            {
                DestroyImmediate(toDestroy[i]);
            }
        }
        

        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();
        HashSet<Vector2Int> potentialRoomPositions = new HashSet<Vector2Int>();

        //make the hallways between
        CreateCorridors(floorPositions, potentialRoomPositions);

        HashSet<Vector2Int> roomPositions = CreateRooms(potentialRoomPositions);

        //list for all the positions that have a dead end to create rooms at
        List<Vector2Int> deadEnds = FindAllDeadEnds(floorPositions);

        CreateRoomsAtDeadEnd(deadEnds,roomPositions);

        floorPositions.UnionWith(roomPositions);

        //create the floor tiles
        tilemapVisualizer.PaintCorridorFloorTiles(floorPositions);
        tilemapVisualizer.PaintFloorTiles(roomPositions);

        //creat the walls
        WallGenerator.CreateWalls(floorPositions, tilemapVisualizer);
    }

    //crate the rooms at the dead ends based on a list
    private void CreateRoomsAtDeadEnd(List<Vector2Int> deadEnds, HashSet<Vector2Int> roomFloors)
    {
        foreach (var position in deadEnds)
        {
            if(roomFloors.Contains(position) == false)
            {
                var room = RunRandomWalk(randomWalkParameters,position);
                roomFloors.UnionWith(room);
                Debug.Log("dead End");
            }
        }
    }

    // the way i find dead end here might be a decent way to locate specific tiles as well
    private List<Vector2Int> FindAllDeadEnds(HashSet<Vector2Int> floorPositions)
    {
        List<Vector2Int> deadEnds = new List<Vector2Int>();
        foreach (var position in floorPositions)
        {
            int neighboursCount = 0;
            foreach (var direction in Direction2D.cardinalDirectionsList)
            {
                if(floorPositions.Contains(position + direction))
                {
                    neighboursCount++;
                }
            }
            if (neighboursCount == 1)
            {
                deadEnds.Add(position);
                emtyRoomPositions.Add(position);
            }
        }
        return deadEnds;
    }

    //these spots might also be good to put the itemes and enemies in
    private HashSet<Vector2Int> CreateRooms(HashSet<Vector2Int> potentialRoomPositions)
    {
        
        HashSet<Vector2Int> roomPositions = new HashSet<Vector2Int>();
        int roomToCreateCount = Mathf.RoundToInt(potentialRoomPositions.Count * roomPercent);

        //Guid creates a unique ID
        List<Vector2Int> roomsToCreate = potentialRoomPositions.OrderBy(x => Guid.NewGuid()).Take(roomToCreateCount).ToList();

        

        foreach (var roomPosition in roomsToCreate)
        {

            var roomFloor = RunRandomWalk(randomWalkParameters, roomPosition);
            Addroom(roomPosition);
            roomPositions.UnionWith(roomFloor);
        }

        //spawn the stuff
        SpawnPlayer();
        SpawnBoss();
        SpawnKey();
        SpawnChest();
        return roomPositions;
    }

    //add room to list
    private void Addroom(Vector2Int roomPos)
    {
        roomspawned++;
        emtyRoomPositions.Add(roomPos);
       
    }

    public void SpawnPlayer()
    {
        if (!playerSpawned)
        {
            Debug.Log("spawn player");
            Instantiate(player, new Vector3(emtyRoomPositions[1].x, emtyRoomPositions[1].y, 0), Quaternion.identity);
            Instantiate(destroyer, new Vector3(emtyRoomPositions[1].x, emtyRoomPositions[1].y, 0), Quaternion.identity);
            cam.transform.position = new Vector3(emtyRoomPositions[1].x, emtyRoomPositions[1].y, -10);
            playerSpawned = true;
        }
        
    }

    //spwan boss in last room
    public void SpawnBoss()
    {
        Debug.Log("spawn Sephiroth");
        int lastRoom = emtyRoomPositions.Count - 1;
        Instantiate(boss, new Vector3(emtyRoomPositions[lastRoom].x, emtyRoomPositions[lastRoom].y, 0), Quaternion.identity);
    }

    //fuctions just in case my chest and key dont spawn
    public void SpawnKey()
    {
        if(!keyExists)
        {
            Debug.Log("spawn key");
            int roomToSpawnIn = emtyRoomPositions.Count - 2;
            Instantiate(key, new Vector3(emtyRoomPositions[roomToSpawnIn].x, emtyRoomPositions[roomToSpawnIn].y, 0), Quaternion.identity);
            keyExists = true;
        }
        
    }

    public void SpawnChest()
    {
        if (keyExists && !chestExists)
        {
            Debug.Log("spawn chest");
            int roomToSpawnIn = emtyRoomPositions.Count - 3;
            Instantiate(chest, new Vector3(emtyRoomPositions[roomToSpawnIn].x, emtyRoomPositions[roomToSpawnIn].y, 0), Quaternion.identity);
            chestExists = true;
        }
    }

    private void CreateCorridors(HashSet<Vector2Int> floorPositions, HashSet<Vector2Int> potentialRoomPositions)
    {
        var currentPosition = startPosition;
        potentialRoomPositions.Add(currentPosition);
        for (int i = 0; i < corridorCount; i++)
        {
            var corridor = ProceduralGenerationAlgorithms.RandomWalkCorridor(currentPosition, corridorLength);
            currentPosition = corridor[corridor.Count - 1];
            potentialRoomPositions.Add(currentPosition);
            floorPositions.UnionWith(corridor);
        }
    }
}

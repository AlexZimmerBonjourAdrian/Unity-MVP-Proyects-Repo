using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class CorridorFirsDungeononGaneration : SimpleRandomWalkDungeonGenerator
{
    //[SerializeField]
    //protected TilemapVisualizer tilemapVisualizer = null;

    //[SerializeField]
    //protected Vector2Int startPostition = Vector2Int.zero;



    //public void GenerateDungeon()
    //{
    //    tilemapVisualizer.Clear();
    //    RunProceduralGeneration();
    //}

    [SerializeField]
    private int corridorLenght = 14, corridorCount = 5;
    
    [SerializeField]
    [Range(0.1f, 1f)]
    private float roomPercent = 0.8f;

    private SimpleRandomWalkSO roomGenerationParameter;

    protected override void RunProceduralGeneration()
    {
        CorridorFirstGeneration();
    }

    private void CorridorFirstGeneration()
    {
        HashSet<Vector2Int> floorPosition = new HashSet<Vector2Int>();
        HashSet<Vector2Int> potentialRoomPositions = new HashSet<Vector2Int>();
        
        CreateCorridors(floorPosition, potentialRoomPositions);

        HashSet<Vector2Int> roomPosition = CreateRooms(potentialRoomPositions);

        List<Vector2Int> deadEnds = FindAllDeadEnds(floorPosition);

        CrateRoomsAtDeadEnd(deadEnds, roomPosition);
        
        floorPosition.UnionWith(roomPosition);

        tilemapVisualizer.PointFoorTiles(floorPosition);
        WallGenerator.CreateWalls(floorPosition, tilemapVisualizer);
    }

    private void CrateRoomsAtDeadEnd(List<Vector2Int> deadEnds, HashSet<Vector2Int> roomFloors)
    {
        foreach (var position in deadEnds)
        {
            if(roomFloors.Contains(position) == false)
            {
                var room = RunRandomWalk(randomWalkParameters, position);
                roomFloors.UnionWith(room);
            }
        }
    }

    private List<Vector2Int> FindAllDeadEnds(HashSet<Vector2Int> floorPosition)
    {
        List<Vector2Int> deadEnds = new List<Vector2Int>();

        foreach(var position in floorPosition)
        {
            int neighboursCount = 0;
            foreach(var direction in Direction2D.cardinalDirectionsList)
            {
                if(floorPosition.Contains(position + direction))
                {
                    neighboursCount++;
                }
            }
            if(neighboursCount == 1)
            {
                deadEnds.Add(position);
            }
        }
        return deadEnds;
    }

    private HashSet<Vector2Int> CreateRooms(HashSet<Vector2Int> potentialRoomPosition)
    {
        HashSet<Vector2Int> roomPositions = new HashSet<Vector2Int>();
        int roomToCreateCount = Mathf.RoundToInt(potentialRoomPosition.Count * roomPercent);

        List<Vector2Int> roomToCreate = potentialRoomPosition.OrderBy(x => Guid.NewGuid()).Take(roomToCreateCount).ToList();

        foreach (var roomPosition in roomToCreate)
        {
            var roomFloor = RunRandomWalk(randomWalkParameters, roomPosition);
            roomPositions.UnionWith(roomFloor);

        }
        return roomPositions;

    }
    private void CreateCorridors(HashSet<Vector2Int> floorPosition, HashSet<Vector2Int> potentialRoomPositions)
    {
        var currentPosition = startPosition;
        potentialRoomPositions.Add(currentPosition);
        for(int i = 0; i < corridorCount; i++)
        {
            var corridor = ProceduralGenerationAlgorithms.RandomWalkCorrido(currentPosition, corridorLenght);
            currentPosition = corridor[corridor.Count - 1];
            potentialRoomPositions.Add(currentPosition);
            floorPosition.UnionWith(corridor);

        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

using Random = UnityEngine.Random;

public class SimpleRandomWalkDungeonGenerator : AbstractDungeonGenerator
{


    [SerializeField]
    protected SimpleRandomWalkSO randomWalkParameters;



    protected override void RunProceduralGeneration()
    {
        HashSet<Vector2Int> floorPosition = RunRandomWalk(randomWalkParameters, startPosition);
        tilemapVisualizer.Clear();
        tilemapVisualizer.PointFoorTiles(floorPosition);
        WallGenerator.CreateWalls(floorPosition, tilemapVisualizer);
        
    }

    protected HashSet<Vector2Int> RunRandomWalk(SimpleRandomWalkSO parameters, Vector2Int position)
    {
        var currentPosition = position;
        HashSet<Vector2Int> floorPosition = new HashSet<Vector2Int>();
        for (int i = 0;i < parameters.interactions; i++)
        {
            var path = ProceduralGenerationAlgorithms.SimpleRandomWalk(currentPosition, randomWalkParameters.walkLength);
            floorPosition.UnionWith(path);
            if(parameters.startRandomlyEachInteration)
            {
                currentPosition = floorPosition.ElementAt(Random.Range(0,floorPosition.Count));
            }
           
        }
        return floorPosition;

    }

}

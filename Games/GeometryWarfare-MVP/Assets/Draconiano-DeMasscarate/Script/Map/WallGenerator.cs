using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WallGenerator
{
   public static void CreateWalls(HashSet<Vector2Int> floorPosition, TilemapVisualizer tilemapVisualizer)
    {
        var basicWallPositions = FindWallsInDirections(floorPosition,Direction2D.cardinalDirectionsList);

        foreach(var position in basicWallPositions)
        {
            tilemapVisualizer.PaintSignleBasicWall(position);
        }
    }

    private static HashSet<Vector2Int> FindWallsInDirections(HashSet<Vector2Int> floorPosition, List<Vector2Int> directionList)
    {
        HashSet<Vector2Int> wallPositions = new HashSet<Vector2Int>(0);
        foreach(var position in floorPosition)
        {
            foreach (var direction in directionList)
            {
                var neighbourPosition = position + direction; 
                if(floorPosition.Contains(neighbourPosition) == false)
                {
                    wallPositions.Add(neighbourPosition);
                }
            }
            
        }
        return wallPositions;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CGridManager : MonoBehaviour
{
    public int gridWidth = 8; // Example size
    public int gridHeight = 8;
    private GameObject[,] grid; 
    public enum GenerateGridState
    {
        Empty,
        Generated,
    };
    [SerializeField]
    private GameObject PrefabGame;

    public GenerateGridState generateGridstate;
   
    void Start()
    {
        grid = new GameObject[gridWidth, gridHeight];
        GenerateGrid();
    }

    void GenerateGrid()
  {
      for (int x = 0; x < gridWidth; x++)
      {
          for (int y = 0; y < gridHeight; y++)
          {
              float probability = CalculateSpawnProbability(x, y);
              if (Random.value < probability) 
              {
                  SpawnBlock(x, y); 
              }
          }
      }
  }
   float CalculateSpawnProbability(int x, int y)
  {
      // Calculate distance from the center
      float centerX = (gridWidth - 1) / 2f;
      float centerY = (gridHeight - 1) / 2f;
      float distance = Vector2.Distance(new Vector2(x, y), new Vector2(centerX, centerY));

      // Invert distance to get higher probability near the center
      float maxDistance = Mathf.Max(centerX, centerY); // Max possible distance
      float normalizedDistance = 1 - (distance / maxDistance); // 1 at center, 0 at edges

      // Apply a curve (optional) for finer control over probability distribution
      float probability = Mathf.Pow(normalizedDistance, 2); // Example: squares the value

      return probability;
  }

  void SpawnBlock(int x, int y)
  {
    var obj = Instantiate(PrefabGame, new Vector2(x,y), Quaternion.identity);
    obj.transform.SetParent(transform);
    grid[x, y] = obj;
  }
}

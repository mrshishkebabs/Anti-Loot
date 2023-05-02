using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int width, height;
    [SerializeField] private Tiles tilePrefab;
    [SerializeField] private Transform cam;

    private Dictionary<Vector2, Tiles> tileDiction;

    private void Start()
    {
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        tileDiction = new Dictionary<Vector2, Tiles>();
        for(int x= 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                var spawnedTile = Instantiate(tilePrefab, new Vector3(x, y), Quaternion.identity);
                spawnedTile.name = $"tile {x} {y}";

                var isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
                spawnedTile.Init(isOffset);

                tileDiction[new Vector2(x, y)] = spawnedTile;
            }
        }

        cam.transform.position = new Vector3((float)width / 2 - 0.5f, (float)height / 2 - 0.5f, -10);
    }

    public Tiles GetTileAtPosition(Vector2 pos)
    {
        if(tileDiction.TryGetValue(pos, out var tiles))
        {
            return tiles;
        }

        return null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int width, height;
    [SerializeField] private Tiles tilePrefab;
    [SerializeField] private GameObject tilesParent;

    [SerializeField] private float xMinBound, xMaxBound;
    [SerializeField] private float yMinBound, yMaxBound;

    // [SerializeField] private Transform cam;

    public static GridManager instance;
    private Dictionary<Vector2, Tiles> tileDiction;

    public float Speed = 1;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        EventBroker.OnTrapPhaseStart += GenerateGrid;
    }

    private void OnDisable()
    {
        EventBroker.OnTrapPhaseStart -= GenerateGrid;
    }

    private void Start()
    {
    }

    private void Update()
    {
        if (GameManager.instance != null && GameManager.instance.state == GameState.TrapPhase)
        {
            if(transform.position.y >= yMaxBound)
            {
                transform.position = new Vector3(transform.position.x, yMaxBound, transform.position.z);
            }
            else if(transform.position.y <= yMinBound)
            {
                transform.position = new Vector3(transform.position.x, yMinBound, transform.position.z);
            }
            if (transform.position.x >= xMaxBound)
            {
                transform.position = new Vector3(xMaxBound, transform.position.y, transform.position.z);
            }
            else if (transform.position.x <= xMinBound)
            {
                transform.position = new Vector3(xMinBound, transform.position.y, transform.position.z);
            }
            float xAxisValue = Input.GetAxis("Horizontal") * Speed;
            float yAxisValue = Input.GetAxis("Vertical") * Speed;

            transform.position = new Vector3(transform.position.x + xAxisValue, transform.position.y + yAxisValue, transform.position.z);
        }
    }

    private void GenerateGrid()
    {
        tileDiction = new Dictionary<Vector2, Tiles>();
        for(int x= 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                var spawnedTile = Instantiate(tilePrefab, new Vector3(x, y), Quaternion.identity,tilesParent.transform);
                spawnedTile.name = $"tile {x} {y}";

                var isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
                spawnedTile.Init(isOffset);

                tileDiction[new Vector2(x, y)] = spawnedTile;
            }
        }

        //cam.transform.position = new Vector3((float)width / 2 - 0.5f, (float)height / 2 - 0.5f, -10);
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

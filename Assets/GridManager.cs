using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    public LevelData levelData;
    public int WIDTH;
    public int HEIGHT;
    public float TILE_SIZE = 1f;
    public Material tileDark;
    public Material tileLight;
    public Material highlightMaterial;
    public Material reachableMaterial;
    public Tile[,] grid;
    
    public GameObject unitPrefab;

    void Awake()
    {
        WIDTH = levelData.width;
        HEIGHT = levelData.height;
        
        GenerateGrid();
        CreateWalls();
        SetUpUnits();
    }

    void GenerateGrid()
    {
        grid = new Tile[WIDTH, HEIGHT];
        for (int x = 0; x < WIDTH; ++x)
        {
            for (int z = 0; z < HEIGHT; ++z)
            {
                GameObject tileObj = GameObject.CreatePrimitive(PrimitiveType.Quad);

                tileObj.transform.position = new Vector3(x * TILE_SIZE, 0, z * TILE_SIZE);
                tileObj.transform.rotation = Quaternion.Euler(90, 0, 0);
                tileObj.name = $"Tile_{x}_{z}";

                Tile tile = tileObj.AddComponent<Tile>();
                tile.x = x;
                tile.z = z;
                tile.defaultMaterial = ((x + z) % 2 == 0) ? tileLight : tileDark;
                tile.highlightMaterial = highlightMaterial;
                tile.reachableMaterial = reachableMaterial;
                tile.SetDefault();
                grid[x, z] = tile;
            }
        }
    }

    void MakeWallAt(int x, int z)
    {
        Tile tile = grid[x, z];
        tile.isWall = true;
        GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        wall.transform.position = new Vector3(x, 0.5f, z);
        wall.tag = "Wall";
    }

    void CreateWalls()
    {
        foreach (Vector2Int pos in levelData.wallPositions)
        {
            MakeWallAt(pos.x, pos.y);
        }
    }
    
    void SetUpUnits()
    {
        UnitManager unitManager = FindAnyObjectByType<UnitManager>();
        foreach (var data in levelData.units)
        {
            GameObject unitObj = Instantiate(unitPrefab);
            unitObj.transform.position = new Vector3(
                data.position.x,
                0.5f,
                data.position.y                
            );
            
            Unit unit = unitObj.GetComponent<Unit>();
            
            unit.unitType = data.unitType;
            unit.maxDistance = data.moveDist;
            unit.shootDistance = data.shootDist;
            unit.tile = grid[data.position.x, data.position.y];
            unit.tile.isOccupied = true;
            unit.canMove = true;
            unit.canShoot = true;
            
            unitManager.RegisterUnit(unit);
        }
    }

    public List<Tile> GetReachableTiles(Tile startTile, int range)
    {
        return GetTilesAtDistanceWithFilter(startTile, range, (Tile n) => (n.isWall || n.isOccupied));
    }
    
    public List<Tile> GetShootableTiles(Tile startTile, int range)
    {
        return GetTilesAtDistanceWithFilter(startTile, range, (Tile n) => (n.isWall));
    }
    
    private List<Tile> GetTilesAtDistanceWithFilter(Tile startTile, int range, System.Func<Tile, bool> filter)
    {
        List<Tile> res = new List<Tile>();
        Queue<Tile> q = new Queue<Tile>();
        Dictionary<Tile, int> dist = new Dictionary<Tile, int>();

        q.Enqueue(startTile);
        dist[startTile] = 0;

        while (q.Count > 0)
        {
            Tile cur = q.Dequeue();

            foreach (Tile n in GetNeighbors(cur))
            {
                if (filter(n))
                    continue;

                int d = dist[cur] + 1;

                if (d > range || dist.ContainsKey(n))
                    continue;
                dist[n] = d;
                q.Enqueue(n);
                res.Add(n);
            }
        }
        return res;
        
    }

    List<Tile> GetNeighbors(Tile tile)
    {
        List<Tile> res = new List<Tile>();
        int x = tile.x;
        int z = tile.z;

        if (x > 0) res.Add(grid[x - 1, z]);
        if (z > 0) res.Add(grid[x, z - 1]);
        if (x < WIDTH - 1) res.Add(grid[x + 1, z]);
        if (z < HEIGHT - 1) res.Add(grid[x, z + 1]);

        return res;
    }
    
    public Stack<Tile> FindPath(Tile start, Tile finish)
    {
        Debug.Log($"start=({start.x}, {start.z}), finish=({finish.x}, {finish.z})");
        Stack<Tile> res = new Stack<Tile>();
        Queue<Tile> q = new Queue<Tile>();
        Dictionary<Tile, Tile> parrent = new Dictionary<Tile, Tile>();
        
        q.Enqueue(start);
        bool finishFinded = false;
        while (q.Count > 0)
        {
            Tile cur = q.Dequeue();
            foreach (Tile n in GetNeighbors(cur))
            {
                if (n.isWall || n.isOccupied) continue;
                if (parrent.ContainsKey(n)) continue;
                parrent[n] = cur;
                q.Enqueue(n);
                if (n == finish) 
                {
                    finishFinded = true;
                    break;
                }
            }
        }
        
        if (!finishFinded) return null;
        
        Tile tile = finish;
        res.Push(tile);
        while (tile != start)
        {
            tile = parrent[tile];
            res.Push(tile);
        }
        return res;
    }
}
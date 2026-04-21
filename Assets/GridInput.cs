using UnityEngine;
using System.Collections.Generic;

public class GridInput : MonoBehaviour
{
    private Tile currentTile;
    private List<Tile> reachableTiles = new List<Tile>();
    
    public GridManager gridManager;
    public Unit unit;

    void Start()
    {
        unit.tile = gridManager.grid[0, 0];
        reachableTiles = gridManager.GetReachableTiles(unit.tile, unit.maxDistance);
        foreach (Tile t in reachableTiles)
        {
            t.SetReacheble();
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleClick();
        }
    }

    void HandleClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Tile tile = hit.collider.GetComponent<Tile>();
            if (tile == null) return;

            currentTile?.SetDefault();
            currentTile = tile;
            currentTile.SetHighlight();
            
            foreach (Tile t in reachableTiles)
            {
                t.SetDefault();
            }
            reachableTiles.Clear();
            
            reachableTiles = gridManager.GetReachableTiles(tile, unit.maxDistance);
            foreach (Tile t in reachableTiles)
            {
                t.SetReacheble();
            }
            unit.MoveTo(gridManager.FindPath(unit.tile, tile));
        }
    }
}

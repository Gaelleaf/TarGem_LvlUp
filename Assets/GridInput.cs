using UnityEngine;
using System.Collections.Generic;

public class GridInput : MonoBehaviour
{
    private Tile currentTile;
    private List<Tile> reachableTiles = new List<Tile>();
    
    public UnityEngine.UI.Button shootButton;
    
    public InputMode inputMode = InputMode.Move;
    public GridManager gridManager;
    public UnitManager unitManager;
    public Unit unit;

    void Start()
    {
        shootButton.onClick.AddListener(SetShootMode);
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
        Debug.Log("Checking what is clicked");
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
    
        if (!Physics.Raycast(ray, out hit)) return;
    
        Unit clickedUnit = hit.collider.GetComponent<Unit>();
        if (clickedUnit != null)
        {
            Debug.Log($"Unit clocked {clickedUnit}");
            unitManager.SelectUnit(clickedUnit);
            if (unit != null)
                unit.GetComponent<Renderer>().material.color = Color.white;
            unit = unitManager.ActiveUnit;
            unit.GetComponent<Renderer>().material.color = Color.yellow;
            if (unit.canMove)
                ShowReachableTiles(unit.tile, unit.maxDistance);
            else if (unit.canShoot)
            {
                SetShootMode();
                ShowReachableTiles(unit.tile, unit.shootDistance);
            }
            return;
        }
    
        
        Tile tile = hit.collider.GetComponent<Tile>();
        if (tile != null)
        {
            Debug.Log("Tile clocked");
            if (unitManager.ActiveUnit == null) return;
            
            switch (inputMode)
            {
            case InputMode.Move:
                HandleMove(tile);
                MaybeEndTurn();
                break;
                
            case InputMode.Shoot:
                HandleShoot(tile);
                inputMode = InputMode.Move;
                unit.canMove = false;
                unit.canShoot = false;
                MaybeEndTurn();
                break;
            
            case InputMode.Dialoge:
                HandleDialoge();
                break;
            
            default:
                break;
            }
            return;
        }
    }
    
    void MaybeEndTurn()
    {
        if (unitManager.AllUnitsDoEverything())
        {
            unitManager.EndTurn();
        }
    }
    
    void ShowReachableTiles(Tile tile, int dist)
    {
        reachableTiles = gridManager.GetReachableTiles(tile, dist);
        foreach (Tile t in reachableTiles)
        {
            t.SetReacheble();
        }
    }
    
    void HandleMove(Tile tile)
    {
        Debug.Log("HandleMove");
        currentTile?.SetDefault();
        currentTile = tile;
        currentTile.SetHighlight();
            
        foreach (Tile t in reachableTiles)
        {
            t.SetDefault();
        }
        reachableTiles.Clear();
        unit.MoveTo(gridManager.FindPath(unit.tile, tile), tile);
        ShowReachableTiles(tile, unit.maxDistance);
    }
    
    void HandleShoot(Tile tile)
    {
        Debug.Log("HandleShoot");
        
        Vector3 origin = unit.transform.position + Vector3.up * 0.5f;
        Vector3 target = tile.transform.position + Vector3.up * 0.5f;
        
        Vector3 direction = (target - origin).normalized;
        float dist = Vector3.Distance(origin, target);
        
        RaycastHit hit;
        
        if (Physics.Raycast(origin, direction, out hit, dist))
        {
            if (hit.collider.gameObject.CompareTag("Wall"))
            {
                Debug.Log("Wall hitted");
                Debug.DrawLine(origin, hit.point, Color.red, 2f);
                return;
            }
        }
        
        Debug.Log("Hit");
        Debug.DrawLine(origin, target, Color.green, 2f);
    }
    
    void HandleDialoge()
    {
        Debug.Log("HandleDialoge");
    }
    
    public void SetShootMode()
    {
        inputMode = InputMode.Shoot;
        Debug.Log("Chosen InputMode.Shoot");
    }
}

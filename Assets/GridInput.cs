using UnityEngine;
using Unity.Mathematics;
using System.Collections.Generic;

public class GridInput : MonoBehaviour
{
    private Tile currentTile;
    private List<Tile> reachableTiles = new List<Tile>();
    private List<Tile> shootableTiles = new List<Tile>();
    
    public UnityEngine.UI.Button shootButton;
    
    public InputMode inputMode = InputMode.Move;
    public GridManager gridManager;
    public UnitManager unitManager;
    //public Unit unit;
    private bool isPlayerTurn = true;

    void Start()
    {
        shootButton.onClick.AddListener(SetShootMode);
    }

    void Update()
    {
        if (unitManager.AllEnemyUnitsAreDead())
        {
            // TODO
            Debug.Log("Player win!");
            return;
        }
        if (unitManager.AllPlayerUnitsAreDead())
        {
            // TODO
            Debug.Log("Enemy won!");
            return;
        }
        if (!isPlayerTurn)
        {
            ResetAllTiles();
            // TODO: MAKE ENEMY TURN
            // isPlayerTurn = true;
            return;
        }
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
            if (clickedUnit.unitType == UnitType.Enemy && inputMode == InputMode.Shoot)
            {
                Debug.Log("--- Enemy clicked ---");
                HandleShoot(clickedUnit.tile);
                return;
            }
            if (!clickedUnit.canMove && !clickedUnit.canShoot)
            {
                Debug.Log($"\"{clickedUnit.name}\" can't do anything");
                return;
            }
            unitManager.ChangeUnitColorPlayer();
            unitManager.SelectUnit(clickedUnit);
            
            ResetAllTiles();
            unitManager.ChangeUnitColorSelected();
            if (unitManager.ActiveUnit.canMove)
            {
                inputMode = InputMode.Move;
                ShowReachableTiles(unitManager.ActiveUnit.tile, unitManager.ActiveUnit.maxDistance);
            }
            else if (unitManager.ActiveUnit.canShoot)
            {
                SetShootMode();
                ShowShootableTiles(unitManager.ActiveUnit.tile, unitManager.ActiveUnit.shootDistance);
            }
            return;
        }
    
        
        Tile tile = hit.collider.GetComponent<Tile>();
        if (tile != null)
        {
            //Debug.Log("Tile clocked");
            if (unitManager.ActiveUnit == null) return;
            
            switch (inputMode)
            {
            case InputMode.Move:
                HandleMove(tile);
                break;
                
            case InputMode.Shoot:
                HandleShoot(tile);
                break;
            
            case InputMode.Dialoge:
                HandleDialoge();
                break;
            
            case InputMode.None:
            default:
                Debug.LogError("Input mode is unhandled");
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
            isPlayerTurn = false;
        }
    }
    
    void ResetAllTiles()
    {
        for (int x = 0; x < gridManager.WIDTH; ++x)
        {
            for (int z = 0; z < gridManager.HEIGHT; ++z)
            {
                Tile tile = gridManager.grid[x, z];
                tile.SetDefault();
            }
        }
        reachableTiles.Clear();
        shootableTiles.Clear();
    }
    
    void ShowReachableTiles(Tile tile, int dist)
    {
        reachableTiles = gridManager.GetReachableTiles(tile, dist);
        foreach (Tile t in reachableTiles)
        {
            t.SetReacheble();
        }
    }
    
    void ShowShootableTiles(Tile tile, int dist)
    {
        shootableTiles = gridManager.GetShootableTiles(tile, dist);
        foreach (Tile t in shootableTiles)
        {
            t.SetShootable();
        }
        
    }
    
    void HandleMove(Tile tile)
    {
        //Debug.Log("HandleMove");
        if (!unitManager.ActiveUnit.canMove)
        {
            Debug.Log($"\"{unitManager.ActiveUnit.name}\" can't move on this turn.");
            return;
        }
        currentTile?.SetDefault();
        currentTile = tile;
        currentTile.SetHighlight();
        
        ResetAllTiles();
        unitManager.ActiveUnit.MoveTo(
            gridManager.FindPath(unitManager.ActiveUnit.tile, tile),
            tile);
        ShowShootableTiles(tile, unitManager.ActiveUnit.shootDistance);
        unitManager.ActiveUnit.canMove = false;
        SetShootMode();
        MaybeEndTurn();
    }
    
    void HandleShoot(Tile tile)
    {
        Debug.Log("HandleShoot");
        if (!unitManager.ActiveUnit.canShoot)
        {
            Debug.Log($"\"{unitManager.ActiveUnit.name}\" can't shoot on this turn.");
            return;
        }
        
        int tileDist = 
            math.abs(tile.x - unitManager.ActiveUnit.tile.x) +
            math.abs(tile.z - unitManager.ActiveUnit.tile.z);
        if (tileDist > unitManager.ActiveUnit.shootDistance)
        {
            Debug.Log("Target is to far");
            return;
        }
        
        Vector3 origin = unitManager.ActiveUnit.transform.position + Vector3.up * 0.5f;
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
                goto epiloge;
                //return true;
            }
            if (tile.isOccupied)
            {
                unitManager.DamageUnitAtTile(tile, 9999);
            }
        }
        
        Debug.Log("Hit");
        Debug.DrawLine(origin, target, Color.green, 2f);
        
    epiloge:
        unitManager.ActiveUnit.canMove = unitManager.ActiveUnit.canShoot = false;
        
        isPlayerTurn = unitManager.ChangeActiveUnitToActableOrEndTurn();
        if (isPlayerTurn)
        {
            ResetAllTiles();
            if (unitManager.ActiveUnit.canMove)
            {
                inputMode = InputMode.Move;
                unitManager.ChangeUnitColorSelected();
                ShowReachableTiles(unitManager.ActiveUnit.tile, unitManager.ActiveUnit.maxDistance);
            }
            else if (unitManager.ActiveUnit.canShoot)
            {
                inputMode = InputMode.Shoot;
                unitManager.ChangeUnitColorSelected();
                ShowShootableTiles(unitManager.ActiveUnit.tile, unitManager.ActiveUnit.shootDistance);
            }
            else
            {
                inputMode = InputMode.None;
            }
        }
    }
    
    void HandleDialoge()
    {
        Debug.Log("HandleDialoge");
    }
    
    public void SetShootMode()
    {
        if (!unitManager.ActiveUnit.canShoot)
        {
            Debug.Log($"\"{unitManager.ActiveUnit.name}\" can't shoot.");
            return;
        }
        inputMode = InputMode.Shoot;
        ResetAllTiles();
        ShowShootableTiles(unitManager.ActiveUnit.tile, unitManager.ActiveUnit.shootDistance);
        Debug.Log("Chosen InputMode.Shoot");
    }
}

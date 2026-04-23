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
    //public Unit unit;
    private bool isPlayerTurn = true;

    void Start()
    {
        shootButton.onClick.AddListener(SetShootMode);
    }

    void Update()
    {
        if (!isPlayerTurn)
        {
            ResetAllTiles();
            // TODO: MAKE ENEMY TURN
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
                if (!unitManager.ActiveUnit.canMove)
                {
                    Debug.Log($"\"{unitManager.ActiveUnit.name}\" can't move on this turn.");
                    break;
                }
                HandleMove(tile);
                unitManager.ActiveUnit.canMove = false;
                SetShootMode();
                MaybeEndTurn();
                break;
                
            case InputMode.Shoot:
                HandleShoot(tile);
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
        reachableTiles = gridManager.GetReachableTiles(tile, dist);
        foreach (Tile t in reachableTiles)
        {
            t.SetShootable();
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
        unitManager.ActiveUnit.MoveTo(
            gridManager.FindPath(unitManager.ActiveUnit.tile, tile),
            tile);
        ShowShootableTiles(tile, unitManager.ActiveUnit.shootDistance);
    }
    
    void HandleShoot(Tile tile)
    {
        Debug.Log("HandleShoot");
        
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
        if (!unitManager.ActiveUnit.canShoot)
        {
            Debug.Log($"\"{unitManager.ActiveUnit.name}\" can't shoot.");
            return;
        }
        inputMode = InputMode.Shoot;
        Debug.Log("Chosen InputMode.Shoot");
    }
}

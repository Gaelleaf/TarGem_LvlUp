using UnityEngine;
using System.Collections.Generic;

public class UnitManager : MonoBehaviour
{
    public List<Unit> fiends = new List<Unit>();
    public List<Unit> friends = new List<Unit>();
    //public int activeFriendIdx = 0;
    public Unit ActiveUnit {get; private set; }
    
    public Material playerMaterial;
    public Material selectedMaterial;
    public Material enemyMaterial;
    
    
    
    public void RegisterUnit(Unit unit)
    {
        switch (unit.unitType)
        {
        case UnitType.Player:
            friends.Add(unit);
            unit.name = $"Player unit {friends.Count}";
            ChangeUnitColorPlayer(unit);
            break;
            
        case UnitType.Enemy:
            fiends.Add(unit);
            unit.name = $"Enemy unit {fiends.Count}";
            ChangeUnitColorEnemy(unit);
            break;
            
        default:
            Debug.Log("Unhandeled unit!!!");
            break;
        }
    }

    public void SelectUnit(Unit unit)
    {
        if (unit.unitType != UnitType.Player) return;
        
        if (!unit.canMove && !unit.canShoot)
        {
            Debug.Log("Unit already do everything");
            return;
        }
        ActiveUnit = unit;
    }
    
    // returns true if new ActiveUnit is found; returns false if it's not and turn is ended;
    public bool ChangeActiveUnitToActableOrEndTurn()
    {
        foreach (Unit u in friends)
        {
            if (u != ActiveUnit)
            {
                if (u.isAlive && (u.canMove || u.canShoot))
                {
                    ChangeUnitColorPlayer();
                    SelectUnit(u);
                    return true;
                }
            }
        }
        ChangeUnitColorPlayer();
        EndTurn();
        return false;
    }
    
    public void EndTurn()
    {
        foreach(Unit u in friends)
        {
            u.canMove = false;
            u.canShoot = false;
        }
        
        ActiveUnit = null;
        Debug.Log("Player ends the turn");
    }

    public bool AllUnitsDoEverything()
    {
        foreach (Unit u in friends)
        {
            if (u.isAlive && (u.canMove || u.canShoot))
            {
                return false;
            }
        }
        return true;
    }
    
    public void DamageUnitAtTile(Tile tile, int dmg)
    {
        foreach (Unit u in fiends)
        {
            if (u.tile == tile)
            {
                DamageUnit(u, dmg);
                return;
            }
        }
        foreach (Unit u in friends)
        {
            if (u.tile == tile)
            {
                DamageUnit(u, dmg);
                return;
            }
        }
        Debug.Log($"No unit on {tile.name} found!");
    }
    
    private void DamageUnit(Unit unit, int dmg)
    {
        if (dmg > 0)
        {
            Debug.Log($"{unit.name} is dead");
            unit.isAlive = false;
            unit.tile = null;
            unit.transform.position = new Vector3(1e5f, 1e5f, 1e5f);
        }
    }
    
    public void ChangeUnitColorSelected()
    {
        if (ActiveUnit == null) return;
        Renderer r = ActiveUnit.GetComponentInChildren<Renderer>();
        r.material = selectedMaterial;
    }
    
    private void ChangeUnitColorSelected(Unit unit)
    {   
        Unit u = ActiveUnit;
        ActiveUnit = unit;
        ChangeUnitColorSelected();
        ActiveUnit = u;
    }
    
    public void ChangeUnitColorPlayer()
    {
        if (ActiveUnit == null) return;
        Renderer r = ActiveUnit.GetComponentInChildren<Renderer>();
        r.material = playerMaterial;
    }
    
    private void ChangeUnitColorPlayer(Unit unit)
    {   
        Unit u = ActiveUnit;
        ActiveUnit = unit;
        ChangeUnitColorPlayer();
        ActiveUnit = u;
    }
    
    public void ChangeUnitColorEnemy()
    {
        if (ActiveUnit == null) return;
        Renderer r = ActiveUnit.GetComponentInChildren<Renderer>();
        r.material = enemyMaterial;
    }    
    
    private void ChangeUnitColorEnemy(Unit unit)
    {   
        Unit u = ActiveUnit;
        ActiveUnit = unit;
        ChangeUnitColorEnemy();
        ActiveUnit = u;
    }
    
    public bool AllEnemyUnitsAreDead()
    {
        foreach (Unit u in fiends)
        {
            if (u.isAlive)
            {
                return false;
            }
        }
        return true;
    }
    
    public bool AllPlayerUnitsAreDead()
    {
        foreach (Unit u in friends)
        {
            if (u.isAlive)
            {
                return false;
            }
        }
        return true;
    }
    
    public void MakeEveryUnitActive()
    {
        foreach (Unit u in friends)
        {
            if (u.isAlive)
            {
                u.canMove = u.canShoot = true;
            }
        }
        foreach (Unit u in fiends)
        {
            if (u.isAlive)
            {
                u.canMove = u.canShoot = true;
            }
        }
    }
}

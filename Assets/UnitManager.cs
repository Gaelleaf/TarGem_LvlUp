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
                if (u.canMove || u.canShoot)
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
            if (u.canMove || u.canShoot)
            {
                return false;
            }
        }
        return true;
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
}

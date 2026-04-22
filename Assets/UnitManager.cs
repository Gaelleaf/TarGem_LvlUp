using UnityEngine;
using System.Collections.Generic;

public class UnitManager : MonoBehaviour
{
    public List<Unit> fiends = new List<Unit>();
    public List<Unit> friends = new List<Unit>();
    public int activeFriendIdx = 0;
    public Unit ActiveUnit {get; private set; }
    
    
    public void RegisterUnit(Unit unit)
    {
        switch (unit.unitType)
        {
        case UnitType.Player:
            friends.Add(unit);
            break;
            
        case UnitType.Enemy:
            fiends.Add(unit);
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
            if (!u.canMove && !u.canShoot)
                return false;
        }
        return true;
    }
}

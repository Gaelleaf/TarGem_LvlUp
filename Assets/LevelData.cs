using UnityEngine;

[System.Serializable]
public class UnitSpawnData
{
    public Vector2Int position;
    public UnitType unitType;
    public int moveDist;
    public int shootDist;
}

[CreateAssetMenu(fileName = "LevelData", menuName = "Scriptable Objects/LevelData")]
public class LevelData : ScriptableObject
{
    public int width;
    public int height;
    
    public Vector2Int[] wallPositions;    
    public UnitSpawnData[] units;
}

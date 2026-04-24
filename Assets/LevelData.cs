using UnityEngine;

[System.Serializable]
public class UnitSpawnData
{
    public Vector2Int position;
    public UnitType unitType;
    public int moveDist;
    public int shootDist;
}

[System.Serializable]
public class CameraPosition
{
    public Vector3 pos;
    public Vector3 rotation;
    public int orthographicSize;
}

[CreateAssetMenu(fileName = "LevelData", menuName = "Scriptable Objects/LevelData")]
public class LevelData : ScriptableObject
{
    public CameraPosition camera;
    public int levelNumber;
    
    public int width;
    public int height;
    
    public Vector2Int[] wallPositions;    
    public UnitSpawnData[] units;
}

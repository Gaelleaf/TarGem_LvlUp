using UnityEngine;
using Unity.Mathematics;

public class CameraController : MonoBehaviour
{
    public LevelData levelData;
    
    void Start()
    {
        transform.position = levelData.camera.pos;//new Vector3(5f, 10f, 2f);
        transform.rotation = Quaternion.Euler(levelData.camera.rotation);//Quaternion.Euler(75f, 0f, 0f);
        GetComponent<Camera>().orthographicSize = levelData.camera.orthographicSize;
    }
}

using UnityEngine;
using Unity.Mathematics;

public class CameraController : MonoBehaviour
{
    void Start()
    {
        transform.position = new Vector3(5f, 10f, 4.5f);
        transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        GetComponent<Camera>().orthographicSize = 6;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

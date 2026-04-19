using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.position = new Vector3(5f, 10f, -5f);
        transform.rotation = Quaternion.Euler(45f, 0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

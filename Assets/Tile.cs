using UnityEngine;

public class Tile : MonoBehaviour
{
    private Renderer renderer;
    public Material defaultMaterial;
    public Material highlightMaterial;
    public Material reachableMaterial;
    public Material shootableMaterial;
    
    public int x;
    public int z;
    
    public bool isWall = false;
    public bool isOccupied = false;

    void Awake()
    {
        renderer = GetComponent<Renderer>();
    }

    public void SetDefault()
    {
        renderer.material = defaultMaterial;
    }

    public void SetHighlight()
    {
        renderer.material = highlightMaterial;
    }
    
    public void SetReacheble()
    {
        renderer.material = reachableMaterial;
    }
    
    public void SetShootable()
    {
        renderer.material = shootableMaterial;
    }
}

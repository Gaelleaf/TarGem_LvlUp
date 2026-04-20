using UnityEngine;

public class Tile : MonoBehaviour
{
    private Renderer renderer;
    public Material defaultMaterial;
    public Material highlightMaterial;

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
}

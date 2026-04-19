using UnityEngine;

public class GridManager : MonoBehaviour
{
    public const int WIDTH = 10;
    public const int HEIGHT = 10;
    public const float TILE_SIZE = 1f;
    public Material tileDark;
    public Material tileLight;
    public Material highlightMaterial;


    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        for (int x = 0; x < WIDTH; ++x)
        {
            for (int z = 0; z < HEIGHT; ++z)
            {
                GameObject tile = GameObject.CreatePrimitive(PrimitiveType.Quad);

                tile.transform.position = new Vector3(x * TILE_SIZE, 0, z * TILE_SIZE);
                tile.transform.rotation = Quaternion.Euler(90, 0, 0);

                tile.name = $"Tile_{x}_{z}";
                Tile tileScript = tile.AddComponent<Tile>();
                tileScript.defaultMaterial = ((x + z) % 2 == 0) ? tileLight : tileDark;
                tileScript.highlightMaterial = highlightMaterial;
                tileScript.SetDefault();
            }
        }
    }
}

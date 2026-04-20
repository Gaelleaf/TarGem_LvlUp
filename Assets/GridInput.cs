using UnityEngine;

public class GridInput : MonoBehaviour
{
    private Tile currentTile;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleClick();
        }
    }

    void HandleClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Tile tile = hit.collider.GetComponent<Tile>();
            if (tile == null) return;

            currentTile?.SetDefault();
            currentTile = tile;
            currentTile.SetHighlight();
        }
    }
}

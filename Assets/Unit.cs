using UnityEngine;
using System.Collections.Generic;

public class Unit : MonoBehaviour
{
    public float moveSpeed = 5f;
    public int maxDistance = 4;

    private Vector3 targetPos;
    private Stack<Tile> pathToTarget = new Stack<Tile>();
    private bool isMoving = false;
    
    public Tile tile;

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            Move();
        }
        else if (pathToTarget.Count > 0)
        {
            MoveToNextTile();
        }
    }

    public void MoveTo(Stack<Tile> path)
    {
        if (path == null) return;
        pathToTarget = path;
    }
    
    void MoveToNextTile()
    {
        tile = pathToTarget.Pop();
        targetPos = tile.transform.position;
        targetPos.y = 0.5f;
        isMoving = true;
    }

    void Move()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPos,
            moveSpeed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, targetPos) < 1e-2)
        {
            isMoving = false;
            transform.position = targetPos;
        }
    }
}
using UnityEngine;
using System.Collections.Generic;

public class Unit : MonoBehaviour
{
    public float moveSpeed = 5f;
    public int maxDistance = 4;
    public int shootDistance = 0;
    public UnitType unitType;
    
    
    public bool canShoot = false;
    public bool canMove = false;
    public Tile tile;
    public bool isAlive = true;

    private Vector3 targetPos;
    private Stack<Tile> pathToTarget = new Stack<Tile>();
    private bool isMoving = false;
    


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

    public void MoveTo(Stack<Tile> path, Tile target)
    {
        if (path == null) return;
        tile.isOccupied = false;
        tile = target;
        tile.isOccupied = true;
        pathToTarget = path;
    }
    
    void MoveToNextTile()
    {
        Tile t = pathToTarget.Pop();
        targetPos = t.transform.position;
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
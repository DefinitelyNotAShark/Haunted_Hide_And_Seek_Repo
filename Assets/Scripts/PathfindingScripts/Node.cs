using UnityEngine;
using System.Collections;
using System;

public class Node
{ 
    public bool isWall;
    public Vector3 position;
    public Node parent;
    public int gCost, hCost;
    public int fCost { get { return gCost + hCost; } }

    private int gridX, gridY;

    public Node(bool isWall, Vector3 position, int gridX, int gridY)
    {
        this.isWall = isWall;
        this.position = position;
        this.gridX = gridX;
        this.gridY = gridY;
    }
}



using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

//Grid manager class handles all the grid properties
public class GridManager : MonoBehaviour
{
    [HideInInspector]
    public List<Node> FinalPath;

    [SerializeField]
    private Transform startPosition;

    [SerializeField]
    private LayerMask wallMask;

    [SerializeField]
    private Vector2 gridWorldSize;

    [SerializeField]
    private float nodeRadius, distance;

    private Node[,] grid;

    private float nodeDiameter;
    private int gridSizeX, gridSizeY;

    private void Start()
    {
        nodeDiameter = nodeRadius * 2;

        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

        CreateGrid();
    }

    private void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];

        Vector3 bottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

        for(int x = 0; x < gridSizeX; x++)
        {
            for(int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = bottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool wall = true;

                if(Physics.CheckSphere(worldPoint, nodeRadius, wallMask))
                {
                    wall = false;
                }

                grid[x, y] = new Node(wall, worldPoint, x, y);
            }
        }
    }

    internal List<Node> GetNeighborNodes(Node node)
    {
        List<Node> NeighboringNodes = new List<Node>();

        int xCheck, yCheck;

        //RIGHT SIDE
        xCheck = node.gridX + 1;
        yCheck = node.gridY;
        if(xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                NeighboringNodes.Add(grid[xCheck, yCheck]);
            }
        } 
        
        //LEFT SIDE
        xCheck = node.gridX - 1;//this is the difference. Sub 1 instead of adding
        yCheck = node.gridY;
        if(xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                NeighboringNodes.Add(grid[xCheck, yCheck]);
            }
        }  
        
        //TOP
        xCheck = node.gridX;
        yCheck = node.gridY + 1;
        if(xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                NeighboringNodes.Add(grid[xCheck, yCheck]);
            }
        }

        //BOTTOM
        xCheck = node.gridX;
        yCheck = node.gridY - 1;
        if(xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                NeighboringNodes.Add(grid[xCheck, yCheck]);
            }
        }

        return NeighboringNodes;
    }

    public Node GetNodeFromWorldPosition(Vector3 worldPosition)
    {
        float xPoint = (worldPosition.x + (gridWorldSize.x / 2)) / gridWorldSize.x;
        float yPoint = (worldPosition.z + (gridWorldSize.y / 2)) / gridWorldSize.y;//should it really be .z tho?

        xPoint = Mathf.Clamp01(xPoint);
        yPoint = Mathf.Clamp01(yPoint);

        int x = Mathf.RoundToInt((gridSizeX - 1) * xPoint);
        int y = Mathf.RoundToInt((gridSizeY - 1) * yPoint);

        return grid[x, y];
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

        if(grid != null)
        {
            foreach(Node node in grid)
            {
                if (node.isWall)
                {
                    Gizmos.color = Color.white;
                }
                else
                {
                    Gizmos.color = Color.yellow;
                }

                if (FinalPath != null)
                {
                    if (FinalPath.Contains(node))
                        Gizmos.color = Color.red;
                }

                Gizmos.DrawCube(node.position, Vector3.one * (nodeDiameter - distance));
            }
        }
    }
}

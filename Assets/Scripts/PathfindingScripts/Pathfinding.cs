using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    //[SerializeField]
    //private Transform startPosition;

    //public Transform targetPosition;

    private GridManager grid;

	void Start ()
    {
        grid = GetComponent<GridManager>();
	}

    public void FindPath(Vector3 startPosition, Vector3 targetPosition)
    {
        Node startNode = grid.GetNodeFromWorldPosition(startPosition);
        Node targetNode = grid.GetNodeFromWorldPosition(targetPosition);

        List<Node> OpenList = new List<Node>();
        HashSet<Node> ClosedList = new HashSet<Node>();

        OpenList.Add(startNode);

        while(OpenList.Count > 0)
        {
            Node currentNode = OpenList[0];

            for (int i = 1; i < OpenList.Count; i++)
            {
                if (OpenList[i].fCost < currentNode.fCost || 
                    OpenList[i].fCost == currentNode.fCost &&
                    OpenList[i].hCost < currentNode.hCost)
                {
                    currentNode = OpenList[i];
                }
            }
            OpenList.Remove(currentNode);
            ClosedList.Add(currentNode);

            if(currentNode == targetNode)
            {
                GetFinalPath(startNode, targetNode);
                break;
            }

            foreach(Node NeighborNode in grid.GetNeighborNodes(currentNode))
            { 
                if(!NeighborNode.isWall || ClosedList.Contains(NeighborNode))//if it's not a wall or closed, keep checking
                {
                    continue;//otherwise skip
                }

                int moveCost = currentNode.gCost + GetManhattanDistance(currentNode, NeighborNode);

                if (moveCost < NeighborNode.gCost || !OpenList.Contains(NeighborNode))
                {
                    NeighborNode.gCost = moveCost;
                    NeighborNode.hCost = GetManhattanDistance(NeighborNode, targetNode);
                    NeighborNode.parent = currentNode;

                    if (!OpenList.Contains(NeighborNode))
                    {
                        OpenList.Add(NeighborNode);
                    }
                }
            }
        }
       
    }

    private int GetManhattanDistance(Node nodeA, Node nodeB)
    {
        int iX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int iY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        return iX + iY;
    }

    private void GetFinalPath(Node startNode, Node endNode)
    {
        List<Node> FinalPath = new List<Node>();

        Node currentNode = endNode;

        while(currentNode != startNode)
        {
            FinalPath.Add(currentNode);
            currentNode = currentNode.parent;
        }
        FinalPath.Reverse();
        grid.FinalPath = FinalPath;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    [SerializeField]
    private Transform startPosition, targetPosition;

    private GridManager grid;

	void Start ()
    {
        grid = GetComponent<GridManager>();
	}

	void Update ()
    {
        FindPath(startPosition.position, targetPosition.position);
	}

    private void FindPath(Vector3 startPosition, Vector3 targetPosition)
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
                if (OpenList[i].fCost > currentNode.fCost || 
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
            }

            foreach(Node NeighborNode in grid.GetNeighborNodes(currentNode))
            {

            }
        }
       
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

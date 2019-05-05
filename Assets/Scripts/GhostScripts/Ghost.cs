using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    [SerializeField]
    private GameObject player;//gives us a reference to the player so we can chaseeeee

    public float ghostMoveSpeed, ghostRotationSpeed;

    [SerializeField]
    private GameObject patrolPointsParent;//get the parent of all the points so I can get them in the script

    public List<Transform> patrolPoints;//this'll tell us where the ghost is going next
    
    public int PointListCount
    {
        get{ return patrolPoints.Count; }
    }

    void Start()
    {
        patrolPoints = new List<Transform>();//set all the points to tell us where the ghost is gonna 
        AddPatrolPoints();//adds all the points to the list
    }

    private void AddPatrolPoints()
    {
        foreach (Transform t in patrolPointsParent.GetComponentsInChildren<Transform>())
        {
            if (t != patrolPointsParent.transform)
                patrolPoints.Add(t);//only add it if it's a child and not the parent       
        }
    }

    public Vector3 GetPatrolPoint(int index)//call from other classes to get a point at an index
    {
        return patrolPoints[index].position;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveGhost : MonoBehaviour
{
    [HideInInspector]
    public Transform HidingSpotPosition;

    [SerializeField]
    private GameObject player;//gives us a reference to the player so we can chaseeeee

    public float ghostMoveSpeed, ghostRotationSpeed;

    [SerializeField]
    private GameObject patrolPointsParent;//get the parent of all the points so I can get them in the script

    public List<Transform> patrolPoints;//this'll tell us where the ghost is going next

    public Vector3 PatrolPointPosition
    {
        get  { return patrolPoints[patrolIndex].position; }//will always get the right value in the list so I only have to change the index
    }

    private int patrolIndex;

    void Start()
    {
        patrolIndex = 0;//zero out the index so it starts at the first point
        
        patrolPoints = new List<Transform>();//set all the points to tell us where the ghost is gonna 
        AddPatrolPoints();//adds all the points to the list
    }

    private void AddPatrolPoints()
    {
        foreach(Transform t in patrolPointsParent.GetComponentsInChildren<Transform>())
        {
            if (t != patrolPointsParent.transform)
                patrolPoints.Add(t);//only add it if it's a child and not the parent       
        }
    }

    public void PatrolBetweenPoints()
    {
        //IF AT DESTINATION
        if (Vector3.Distance(transform.position, PatrolPointPosition) < 1)//if we're within the range of our target
        {
            if (patrolIndex == patrolPoints.Count - 1)
                patrolIndex = 0;

            else patrolIndex++;//get a new target
        }

        //ROTATE
        Quaternion targetRotation = Quaternion.LookRotation(PatrolPointPosition - transform.position);//find where it's heading
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * ghostRotationSpeed);//set the rotation to move towards where it's headed
        //MOVE
        transform.Translate(Vector3.forward * Time.deltaTime * ghostMoveSpeed);
    }

    public void GoToPlayerLocation()
    {
        //IF NOT CLOSE ENOUGH TO PLAYER
        if(Vector3.Distance(transform.position, player.transform.position) > 1)
        {
            Vector3.MoveTowards(transform.position, player.transform.position, ghostMoveSpeed * Time.deltaTime);
        }

        //IF CLOSE ENOUGH
        else
        {
            Debug.Log("YOU GOT GOT SON");
        }
    }

}

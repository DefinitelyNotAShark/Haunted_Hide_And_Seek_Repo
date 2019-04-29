using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckHidingSpotState : StateMachineBehaviour
{
    private Ghost ghost;

    //PATHFINDING REFS
    private GameObject pathfindingObject;
    private Pathfinding pathScript;
    private GridManager gridScript;

    private Transform transform;

    private float ghostRotationSpeed, ghostMoveSpeed;
    private int nodeIndex = 0;

    private List<Vector3> hidingSpots;
    private List<Node> path;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        //GET OBJECTS THAT CONTAIN PROPERTIES
        ghost = animator.GetComponent<Ghost>();
        pathfindingObject = GameObject.FindGameObjectWithTag("Pathfinder");

        //GET GHOST PROPERTIES
        transform = ghost.transform;
        hidingSpotPos = ghost.HidingSpotPosition.position;//set the position to look for;
        ghostRotationSpeed = ghost.ghostRotationSpeed;
        ghostMoveSpeed = ghost.ghostMoveSpeed;

        //GET PATHFINDING PROPERTIES
        pathScript = pathfindingObject.GetComponent<Pathfinding>();
        gridScript = pathfindingObject.GetComponent<GridManager>();

        path = new List<Node>();

        //FIND PATH TO HIDING SPOT
        pathScript.FindPath(transform.position, hidingSpotPos);
        path = gridScript.FinalPath;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        //IF THE GHOST HASN"T REACHED THE FINAL DESTINATION
        if (!nodeIsAtDestination())
        {
            //Get new node if you reaached the last one
            if (haveReachedNextNode() && nodeIndex < path.Count - 1)
            {
                Debug.Log("Finding a new node for the hiding spot...");
                nodeIndex++;//get new node
            }

            //ROTATE
            Quaternion targetRotation = Quaternion.LookRotation(path[nodeIndex].position - transform.position);//find where it's heading
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * ghostRotationSpeed);//set the rotation to move towards where it's headed
            //MOVE
            transform.Translate(Vector3.forward * Time.deltaTime * ghostMoveSpeed);
        }
        else animator.SetBool("CheckHidingSpot", false);//it really should go back to the previous state
    }

    private bool nodeIsAtDestination()
    {
        if(nodeIndex == gridScript.FinalPath.Count - 1 &&
            Vector3.Distance(transform.position, path[nodeIndex].position) < 1)//if I'm close enough to the final node
            return true;
        
        else return false;
    }

    private bool haveReachedNextNode()
    {
        if (Vector3.Distance(transform.position, path[nodeIndex].position) < 1)//if we're close enough to the next node
            return true;

        else return false;
    }
}

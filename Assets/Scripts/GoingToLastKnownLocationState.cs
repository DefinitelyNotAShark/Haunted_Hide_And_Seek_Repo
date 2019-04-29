using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoingToLastKnownLocationState : StateMachineBehaviour
{
    private Ghost ghost;

    //PATHFINDING REFS
    private GameObject pathfindingObject;
    private Pathfinding pathScript;
    private GridManager gridScript;
    private Transform transform;

    private Vector3 lastKnownLocation;

    private float ghostRotationSpeed, ghostMoveSpeed;

    private int nodeIndex = 0;
    List<Node> path;//this is our reference to the path that the manager finds

    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        //GET OBJECTS THAT CONTAIN PROPERTIES
        ghost = animator.GetComponent<Ghost>();
        pathfindingObject = GameObject.FindGameObjectWithTag("Pathfinder");

        //GET GHOST PROPERTIES
        transform = ghost.transform;
        ghostRotationSpeed = ghost.ghostRotationSpeed;
        ghostMoveSpeed = ghost.ghostMoveSpeed;

        //GET PATHFINDING PROPERTIES
        pathScript = pathfindingObject.GetComponent<Pathfinding>();
        gridScript = pathfindingObject.GetComponent<GridManager>();

        lastKnownLocation = ghost.gameObject.GetComponentInChildren<LookGhost>().LastKnownLocation;
        path = new List<Node>();

        //FIND PATH TO LAST LOCATION IT SAW PLAYER
        FindPathToPlayer();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        if (!ghostIsAtDestination())
        {
            //Get new node if you reached the last one and it's not the final node
            if (haveReachedNextNode() && nodeIndex < path.Count - 1)
            {
                nodeIndex++;//get new node
            }

            //ROTATE
            Quaternion targetRotation = Quaternion.LookRotation(path[nodeIndex].position - transform.position);//find where it's heading
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * ghostRotationSpeed);//set the rotation to move towards where it's headed
                                                                                                                           //MOVE
            transform.Translate(Vector3.forward * Time.deltaTime * ghostMoveSpeed);
        }
        else
        {
            Debug.Log("I'm at where I last saw the player");
            animator.SetTrigger("GetHidingSpots");
        }
    }

    private bool haveReachedNextNode()
    {
        if (Vector3.Distance(transform.position, path[nodeIndex].position) < 1)//if we're close enough to the next node
            return true;

        else return false;
    }

    private bool ghostIsAtDestination()
    {
        if (nodeIndex == path.Count - 1 &&
            Vector3.Distance(transform.position, path[nodeIndex].position) < 2)//if I'm close enough to the final node
            return true;

        else return false;
    }

    private void FindPathToPlayer()
    {
        nodeIndex = 0;//reset the index
        Vector3 position = lastKnownLocation;
        pathScript.FindPath(transform.position, lastKnownLocation);//find path to the position
        path = gridScript.FinalPath;
    }
}

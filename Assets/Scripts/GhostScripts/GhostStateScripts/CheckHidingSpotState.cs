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
    //GameOver ref
    private GameOverManager gameOver;

    private Transform transform;

    private float ghostRotationSpeed, ghostMoveSpeed;
    private int nodeIndex = 0;

    private List<GameObject> hidingSpots = new List<GameObject>();
    private List<Node> path;

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

        gameOver = GameObject.FindGameObjectWithTag("GameOver").GetComponent<GameOverManager>();

        hidingSpots = animator.GetComponentInChildren<LookGhost>().HidingSpotPositions;//get the list of positions to check

        path = new List<Node>();

        FindPathToHidingSpot();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        if (ghostIsAtDestination())//if we got there...
        {
            if (hidingSpots[0].GetComponentInChildren<HidingSpot>().PlayerIsHiddenHere)//if the player happens to be here...
            {
                Debug.Log("GOT YOU!");
                gameOver.GameOver();//do the gameover
                //Trigger game over
            }


            hidingSpots.Remove(hidingSpots[0]);//remove the hiding spot from the list of spots to check     
            nodeIndex = 0;//just there for a little extra oomph

            if (hidingSpots.Count > 0)//if we still have a path to find...
                FindPathToHidingSpot();//find a path to the next one
            else         
                animator.SetBool("CheckHidingSpot", false);//it really should go back to the previous state
        }

        //IF THE GHOST HASN"T REACHED THE FINAL DESTINATION AND STILL HAS A HIDING SPOT
        else if (!ghostIsAtDestination() && hidingSpots.Count > 0)
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
      
    }

    private bool ghostIsAtDestination()
    {
        if (nodeIndex == path.Count - 1 &&
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

    private void FindPathToHidingSpot()
    {
        //FIND PATH TO HIDING SPOT
        nodeIndex = 0;//reset index each time a new path is found
        pathScript.FindPath(transform.position, hidingSpots[0].transform.position);//find the first one
        path = gridScript.FinalPath;
    }
}

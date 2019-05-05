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
        //GET OBJECTS
        pathfindingObject = GameObject.FindGameObjectWithTag("Pathfinder");

        //GET OTHER SCRIPTS
        ghost = animator.GetComponent<Ghost>();
        gameOver = GameObject.FindGameObjectWithTag("GameOver").GetComponent<GameOverManager>();
        hidingSpots = animator.GetComponentInChildren<LookGhost>().HidingSpotPositions;//get the list of positions to check

        //GET GHOST PROPERTIES
        transform = ghost.transform;
        ghostRotationSpeed = ghost.ghostRotationSpeed;
        ghostMoveSpeed = ghost.ghostMoveSpeed;

        //GET PATHFINDING PROPERTIES
        pathScript = pathfindingObject.GetComponent<Pathfinding>();
        gridScript = pathfindingObject.GetComponent<GridManager>();

        path = new List<Node>();

        if (hidingSpots.Count > 0)//sometimes it goes into this state even if it has no spots to check. Exit the state if that's the case
        {
            SortSpotsByDistance(animator.gameObject);//sorts our hiding spots with the closest one in the front that we look at first
            FindPathToHidingSpot();//find a path to first one
        }
        else
        {
            animator.SetBool("CheckHidingSpot", false);//if there's no spots to check, exit the state
            animator.SetBool("HasSeenPlayer", false);//since there are no more spots, investigation also ceases and goes back to wandering
        }
    }

    

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        if (ghostIsAtDestination())//if we got there...
        {
            if (hidingSpots[0].GetComponent<HidingSpot>().PlayerIsHiddenHere)//if the player happens to be here...
            {
                gameOver.GameOver();//do the gameover
            }

            hidingSpots.Remove(hidingSpots[0]);//remove the hiding spot from the list of spots to check     
            nodeIndex = 0;//just there for a little extra oomph

            if (hidingSpots.Count > 0)//if we still have a path to find...
                FindPathToHidingSpot();//find a path to the next one
            else
            {
                animator.SetBool("CheckHidingSpot", false);//it really should go back to the previous state
                animator.SetBool("HasSeenPlayer", false);
            }
        }

        //IF THE GHOST HASN"T REACHED THE FINAL DESTINATION AND STILL HAS A HIDING SPOT
        else if (!ghostIsAtDestination() && hidingSpots.Count > 0 && path != null)
        {
            //Get new node if you reaached the last one
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
    }

    private bool ghostIsAtDestination()
    {
        if (nodeIndex == path.Count - 1 &&
            Vector3.Distance(transform.position, path[nodeIndex].position) < 2)//if I'm close enough to the final node
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

    private void SortSpotsByDistance(GameObject ghost)
    {
        List<float> distances = new List<float>();

        foreach (GameObject g in hidingSpots)//for every hiding spot, find the distance between us
        {
            float distance = Vector3.Distance(ghost.transform.position, g.transform.position);//find the distance between us
            distances.Add(distance);
        }

        float tempDistance;
        GameObject tempHidingSpot;

        //SORT (BUBBLE)   
        for (int write = 0; write < distances.Count; write++)//Swap both distance and spot object so the index stays the same.
        {
            for (int sort = 0; sort < distances.Count - 1; sort++)
            {
                if (distances[sort] > distances[sort + 1])
                {
                    tempDistance = distances[sort + 1];
                    tempHidingSpot = hidingSpots[sort + 1];//swap them together so the indexes stay the same.

                    distances[sort + 1] = distances[sort];
                    hidingSpots[sort + 1] = hidingSpots[sort];

                    distances[sort] = tempDistance;
                    hidingSpots[sort] = tempHidingSpot;
                }
            }
        }
    }
}

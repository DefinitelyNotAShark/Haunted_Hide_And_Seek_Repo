using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : StateMachineBehaviour
{
    private Ghost ghost;

    //PATHFINDING REFS
    private GameObject pathfindingObject;
    private GameObject player;
    private Pathfinding pathScript;
    private GridManager gridScript;
    private Transform transform;
    private Vector3 playerPosition;
    private float ghostRotationSpeed, ghostMoveSpeed;

    private HidePlayer playerHideScript;
    private GameOverManager gameOver;

    private int nodeIndex = 0;
    private List<Node> path;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        //GET OBJECTS THAT CONTAIN PROPERTIES
        ghost = animator.GetComponent<Ghost>();
        pathfindingObject = GameObject.FindGameObjectWithTag("Pathfinder");
        player = GameObject.FindGameObjectWithTag("Player");

        //GET GHOST PROPERTIES
        transform = ghost.transform;
        ghostRotationSpeed = ghost.ghostRotationSpeed;
        ghostMoveSpeed = ghost.ghostMoveSpeed;

        //GET PATHFINDING PROPERTIES
        pathScript = pathfindingObject.GetComponent<Pathfinding>();
        gridScript = pathfindingObject.GetComponent<GridManager>();

        gameOver = GameObject.FindGameObjectWithTag("GameOver").GetComponent<GameOverManager>();//get the ref to the game over script
        playerHideScript = player.GetComponentInParent<HidePlayer>();//get a reference so we know when the player is hiding (in the parent of the player)
        
        path = new List<Node>();

        //FIND PATH TO HIDING SPOT
        GetNewPoint();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        if (path.Count > 13 || playerHideScript.isHidden)//if you're too far away for the ghost to sense you, or you start hiding
        {
            animator.SetBool("PlayerIsInLineOfSight", false);//it looses track of your exact position
        }

        UpdatePoint();

        if (path.Count > 1)//if the path is long enough to navigate
        {
            if (haveReachedNextNode() && nodeIndex < path.Count - 1)
                nodeIndex++;//get new node 


            //ROTATE        
            Quaternion targetRotation = Quaternion.LookRotation(path[nodeIndex].position - transform.position);//find where it's heading
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * ghostRotationSpeed);//set the rotation to move towards where it's headed 
            //MOVE
            transform.Translate(Vector3.forward * Time.deltaTime * ghostMoveSpeed);
        }

        else//otherwise this means we're close enough to be caught
        {
            gameOver.GameOver();
        }
    }

    private bool haveReachedNextNode()
    {
        if (Vector3.Distance(transform.position, path[nodeIndex].position) < 1)//if we're close enough to the next node
            return true;

        else return false;
    }

    private void UpdatePoint()//checks if the player has moved and gets the new location if yes (stops from constantly updating)
    {
        if (player.transform.position != playerPosition)//if the actual player position is different than the position we're keeping track of    
            GetNewPoint(); 
    }

    private void GetNewPoint()
    {
        nodeIndex = 0;//reset the index
        playerPosition = player.transform.position;
        pathScript.FindPath(transform.position, playerPosition);//find path to the position
        path = gridScript.FinalPath;
    }
}

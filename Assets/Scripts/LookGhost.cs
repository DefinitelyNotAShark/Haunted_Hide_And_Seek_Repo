using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookGhost : MonoBehaviour
{
    [SerializeField]
    private float lengthOfSight;

    private GhostStateManager stateManager;

    private void Start()
    {
        stateManager = GetComponentInParent<GhostStateManager>();//get the state manager?      
    }

    private void OnTriggerEnter(Collider other)//change the state based on what the ghost sees
    {
        if(isHidingSpot(other.gameObject) && shouldCheckHidingSpot())//if it sees a hiding spot and decides to check it,
        {
            stateManager.SetState(GhostStateManager.State.checkingHidingSpot);
            //set the hiding spot in the moving script so that it knows where to go????
        }

        if (isPlayer(other.gameObject))
        {
            stateManager.SetState(GhostStateManager.State.chasing);//if it sees the player, it is chasing
        }
    }

    private void OnTriggerExit(Collider other)//change state based on if ghost loses sight of something?
    {
        if (isPlayer(other.gameObject))
            stateManager.SetState(GhostStateManager.State.seeking);//if we lose sight of the player, we revert to checking every spot in the vicinity
    }

    private bool shouldCheckHidingSpot()//checks hiding spots 50% of the time while wandering and 100% of the time while seeking
    {
        if (stateManager.GhostState == GhostStateManager.State.wandering)//if it's wandering around, 50% chance it checks
        {
            if (Random.Range(0, 2) == 1)//chooses a number between 0 and 1. On the chance that it's a 1, we check the spot. otherwise we don't
            {
                return true;
            }
            else return false;
        }
        else if (stateManager.GhostState == GhostStateManager.State.seeking)//if it's looking in a small area for the player, it always checks
            return true;

        else return false;//it should not have to check a hiding spot if it already knows where the player is
    }

    private bool isHidingSpot(GameObject other)
    {
        if (other.layer == 9)//hiding spot layer
            return true;

        else return false;
    }

    private bool isPlayer(GameObject other)
    {
        if (other.layer == 10)//player layer
            return true;

        else return false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookGhost : MonoBehaviour
{
    [SerializeField]
    private float lengthOfSight;

    [HideInInspector]
    public Vector3 LastKnownLocation;

    [HideInInspector]
    public List<Vector3> HidingSpotPositions;//add to list when deciding to check

    private List<Vector3> HidingSpotsChecked = new List<Vector3>();

    private Animator animator;

    private void OnTriggerEnter(Collider other)//change;the state based on what the ghost sees
    {
        animator = GetComponentInParent<Animator>();

        if (isHidingSpot(other.gameObject) && shouldCheckHidingSpot())//if it sees a hiding spot and decides to check it,
        {
            Debug.Log("I am going to check this hiding spot");

            if (!HidingSpotsChecked.Contains(other.transform.position))//get the hiding spot look point
            {
                Vector3 hidingSpotPositionToCheck = other.gameObject.GetComponentInChildren<hi>().gameObject.transform.position;
            
                HidingSpotPositions.Add(hidingSpotPositionToCheck);//add the transform to the list of spots to check if it hasn't already been added
                HidingSpotsChecked.Add(hidingSpotPositionToCheck);//lets the ghost know not to check that one for a while
            }

            animator.SetBool("CheckHidingSpot", true);
        }        

        if (isPlayer(other.gameObject))
        {
            animator.SetBool("PlayerIsInLineOfSight", true);
        }
    }

    private void OnTriggerStay(Collider other)//if the player is still there, make sure the bool's set to canSeePlayer
    {
        animator = GetComponentInParent<Animator>();

        if (isPlayer(other.gameObject))
        {
            animator.SetBool("PlayerIsInLineOfSight", true);
        }
    }

    private void OnTriggerExit(Collider other)//change state based on if ghost loses sight of something?
    {
        animator = GetComponentInParent<Animator>();

        if (isPlayer(other.gameObject))
        {
            Debug.Log("I know where you were, and am going to check around it");
            animator.SetBool("HasSeenPlayer", true);//remembers that it saw the player for the duration of the seeking state
            animator.SetBool("PlayerIsInLineOfSight", false);//can't directly see the player anymore
            LastKnownLocation = other.transform.position;//this is where we're going to go 
        }
    }

    private bool shouldCheckHidingSpot()//checks hiding spots 50% of the time while wandering and 100% of the time while seeking
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Wandering"))//if it's wandering around, 50% chance it checks
        {
            if (Random.Range(0, 2) == 1)//chooses a number between 0 and 1. On the chance that it's a 1, we check the spot. otherwise we don't
            {
                return true;
            }
            else
            {
                Debug.Log("I decided not to check this hiding spot");
                return false;
            }
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Seeking"))//if it's looking in a small area for the player, it always checks
            return true;

        else
            return false;//it should not have to check a hiding spot if it already knows where the player is
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

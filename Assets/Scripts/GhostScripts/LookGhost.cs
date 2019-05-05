using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookGhost : MonoBehaviour
{
    [HideInInspector]
    public Vector3 LastKnownLocation;

    [HideInInspector]
    public List<GameObject> HidingSpotPositions = new List<GameObject>();//add to list when deciding to check

    private List<GameObject> HidingSpotsChecked = new List<GameObject>();

    private Animator animator;

    private void OnTriggerEnter(Collider other)//change;the state based on what the ghost sees
    {
        animator = GetComponentInParent<Animator>();

        if (isHidingSpot(other.gameObject) && shouldCheckHidingSpot())//if it sees a hiding spot and decides to check it,
        {
            GameObject hidingSpotPositionToCheck = other.gameObject.GetComponentInChildren<HidingSpot>().gameObject;

            if (!HidingSpotsChecked.Contains(hidingSpotPositionToCheck))//get the hiding spot look point
            {
                HidingSpotPositions.Add(hidingSpotPositionToCheck);//add the transform to the list of spots to check 
                HidingSpotsChecked.Add(hidingSpotPositionToCheck);//lets the ghost know not to check that one for a while...clear it when seeking
                animator.SetBool("CheckHidingSpot", true);
            }
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
            //animator.SetBool("PlayerIsInLineOfSight", false);//can't directly see the player anymore, but it can keep it's sense of where it is 
            LastKnownLocation = other.transform.position;//this is where we're going to go 
        }
    }

    private bool shouldCheckHidingSpot()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Wandering"))//if it's wandering around, 20% chance it checks
        {
            if (Random.Range(0, 5) == 1)//chooses a number between 0 and 5. On the chance that it's a 1, we check the spot. otherwise we don't
            {
                return true;
            }
            else return false;
        }
        else return false;//the seeking state should know what spots to check with a radius. Has a specific list to check
    }

    private bool isHidingSpot(GameObject other)
    {
        if (other.layer == 9)//hiding spot layer
            return true;

        else return false;
    }

    private bool isPlayer(GameObject other)//raycasts to the object and sees if it's the player and if it's not hiding behind a wall
    {
        if (other.layer == 10)
            return true;

        else return false;
    }


    /// <summary>
    /// Clears the list of spots already checked. Do this when the ghost is seeking the player
    /// </summary>
    public void ClearCheckedList()
    {
        HidingSpotsChecked.Clear();
    }
}

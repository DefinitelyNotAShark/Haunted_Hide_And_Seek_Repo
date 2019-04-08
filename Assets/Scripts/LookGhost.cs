using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookGhost : MonoBehaviour
{
    [SerializeField]
    private float lengthOfSight;

    private Animator animator;

    private void Start()
    {
    }

    private void OnTriggerEnter(Collider other)//change the state based on what the ghost sees
    {
        animator = GetComponentInParent<Animator>();

        if (isHidingSpot(other.gameObject) && shouldCheckHidingSpot())//if it sees a hiding spot and decides to check it,
        {
            Debug.Log("I am going to check this hiding spot");
            animator.SetTrigger("CheckHidingSpot");
        }

        if (isPlayer(other.gameObject))
        {
            Debug.Log("I know where you are");
            animator.SetTrigger("Chasing");
        }
    }

    private void OnTriggerExit(Collider other)//change state based on if ghost loses sight of something?
    {
        animator = GetComponentInParent<Animator>();

        if (isPlayer(other.gameObject))
        {
            Debug.Log("I know where you were, and am going to check around it");
            animator.SetTrigger("Seeking");
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

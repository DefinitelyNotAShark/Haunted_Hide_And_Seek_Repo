using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetNearbyHidingSpotsState : StateMachineBehaviour
{
    private Vector3 ghostPos;
    private LookGhost lookScript;
    int radius;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        radius = 10;//get all hiding spots in a radius of 10
        ghostPos = animator.gameObject.transform.position;

        lookScript = animator.GetComponentInChildren<LookGhost>();
        lookScript.ClearCheckedList();//clear the list keeping track of all the hiding spots previously checked once this state is entered

        Collider[] spotColliders = Physics.OverlapSphere(ghostPos, radius);//9 = hiding spot layer

        foreach(Collider c in spotColliders)
        {

            if (c.gameObject.layer == 9)
            {
                GameObject hidingSpotPos = c.GetComponentInChildren<HidingSpot>().gameObject;//get that position connected to the coll's child               
                lookScript.HidingSpotPositions.Add(hidingSpotPos);//add that
            }
        }

        animator.SetBool("CheckHidingSpot", true);
    }
}

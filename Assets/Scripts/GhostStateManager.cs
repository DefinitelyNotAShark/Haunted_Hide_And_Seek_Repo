using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostStateManager : MonoBehaviour
{
    private MoveGhost moveScript;

    public enum State
    {
        wandering,//the ghost has no clue where you are and is wandering around aimlessly
        chasing,//the ghost knows where you are and is heading towards your location
        seeking,//the ghost doesn't know your exact location, but knows approx where you are and is looking around there
        checkingHidingSpot//regardless of your location, it is looking in a hiding spot
    }

    public State GhostState { get; private set; }

	void Start ()
    {
        moveScript = GetComponent<MoveGhost>();//a reference to our move script so we can move it remotely
        
        GhostState = State.wandering;//start out without knowing the player's position
	}

    public void SetState(State state)//call this instead of directly setting the value
    {
        GhostState = state;
    }

    private void Update()
    {
        switch (GhostState)
        {
            case State.wandering:
                moveScript.PatrolBetweenPoints();//moves between set points in the game
                break;
            case State.chasing:
                //Go straight for player location
                moveScript.GoToPlayerLocation();
                break;
            case State.seeking:
                //get all waypoints in a certain radius and check all of them before resuming wandering
                break;
            case State.checkingHidingSpot:
                //make a point where ghost was on enter
                //make note of the waypoint where it's headed
                //set move script to diverge to the hiding spot
                break;
        }
    }
}

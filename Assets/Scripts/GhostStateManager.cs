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
        seeking//the ghost doesn't know your exact location, but knows approx where you are and is looking around there
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
                break;
            case State.seeking:
                break;
        }
    }
}

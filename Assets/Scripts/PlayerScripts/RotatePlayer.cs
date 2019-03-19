using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePlayer : MonoBehaviour
{
    private float horizontalValue;
    private float verticalValue;

    private MovePlayer moveScript;

    private void Start()
    {
        moveScript = GetComponentInParent<MovePlayer>();
    }

    private void Update()
    {
        horizontalValue = moveScript.horizontalValue;
        verticalValue = moveScript.verticalValue;
    }

    // Update is called once per frame
    void FixedUpdate ()
    {
        float heading = Mathf.Atan2(horizontalValue, verticalValue) * Mathf.Rad2Deg;

        //only rotate if you're getting input. That way the player doesn't turn to face front any time your fingers leave the keys
        if (Mathf.Abs(horizontalValue) > 0 || Mathf.Abs(verticalValue) > 0)
        {
            transform.localEulerAngles = new Vector3(0, heading, 0);
        }
    }
}

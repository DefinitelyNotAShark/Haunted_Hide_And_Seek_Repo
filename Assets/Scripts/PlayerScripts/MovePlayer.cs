using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    [SerializeField]
    private float speed;

    public float horizontalValue { get; private set; }
    public float verticalValue { get; private set; }

    [HideInInspector]
    public bool canMove;

    private Vector3 moveDirection;
    private Rigidbody rigidbody;

    private void Start()
    {
        canMove = true;
        rigidbody = GetComponent<Rigidbody>();
    }

    void Update ()
    {
        horizontalValue = Input.GetAxis("Horizontal");
        verticalValue = Input.GetAxis("Vertical");

        moveDirection = new Vector3(horizontalValue, 0, verticalValue) * speed * Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (canMove)
            rigidbody.MovePosition(rigidbody.position + transform.TransformDirection(moveDirection));//move
    }
}

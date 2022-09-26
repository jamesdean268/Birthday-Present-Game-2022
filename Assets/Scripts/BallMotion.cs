using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMotion : MonoBehaviour
{

    public bool isHit;
    public Vector3 forceVector;
    private Rigidbody ballRigidBody; 

    // Start is called before the first frame update
    void Start()
    {
        ballRigidBody = GetComponent<Rigidbody>();
        isHit = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isHit)
        {
            ballRigidBody.AddForce(forceVector);
            isHit = false;
        }

        if (Mathf.Abs(ballRigidBody.velocity.magnitude) < 2.0f)
        {
            if (ballRigidBody.position.z > 0.5f)
            {
                //Respawn();
            }
        }

    }

    public void Respawn()
    {
        transform.position = new Vector3(0.0f, 2.0f, 0.0f);
        ballRigidBody.velocity = new Vector3(0.0f, 0.0f, 0.0f);
        ballRigidBody.angularVelocity = new Vector3(0.0f, 0.0f, 0.0f);
        Start();
    }
}

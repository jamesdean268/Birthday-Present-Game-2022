using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchManagerScript : MonoBehaviour
{

    // UI Elements
    public FixedTouchField fixedTouchField;

    // Touch variables
    private float smashFactor;

    // Game Objects
    public GameObject golfBallObject;

    // Start is called before the first frame update
    void Start()
    {
        smashFactor = 15.0f;
    }

    // Force Variables
    private Vector3 forceVector;
    private float forceMagnitude;
    private float pathAngle;

    void FixedUpdate()
    {
        if (fixedTouchField.Pressed)
        {
            forceVector.x += fixedTouchField.TouchDist.x * smashFactor;
            forceVector.y += fixedTouchField.TouchDist.y * smashFactor * 0.3f;
            forceVector.z += fixedTouchField.TouchDist.y * smashFactor;
        } else
        {
            if (Mathf.Abs(forceVector.magnitude) > 0)
            {
                golfBallObject.GetComponent<BallMotion>().isHit = true;
                golfBallObject.GetComponent<BallMotion>().forceVector = forceVector;
            }
            forceVector = Vector3.zero;
        }
        
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchManagerScript : MonoBehaviour
{

    // UI Elements
    public FixedTouchField fixedTouchField;

    // Touch variables
    private float rotationSpeed;

    // Game Objects
    public GameObject golfBallObject;

    // Start is called before the first frame update
    void Start()
    {
        rotationSpeed = 0.15f;
    }

    // Force Variables
    private Vector3 forceVector;
    private float forceMagnitude;
    private float pathAngle;

    void FixedUpdate()
    {
        forceVector.x += fixedTouchField.TouchDist.x * rotationSpeed;
        forceVector.z += fixedTouchField.TouchDist.y * rotationSpeed;

    }

}

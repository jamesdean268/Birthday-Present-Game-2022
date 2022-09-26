using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{

    // Camera movement flags

    // Position and Rotation
    private Vector3 offsetPosRef_m;
    private Vector3 defaultOffsetPosRef_m;
    public Vector3 offsetCameraLook_m; // = new Vector3(0.0f, 0.35f, 0.0f);
    private Vector3 referencePos_m;
    private Vector3 mainMenuPos_m;
    public float smoothTime_s;
    public float smoothTimeXZ_s;
    public float rotateSmoothTime_s;
    public float defaultLookDownAngle_deg;
    private float mainMenuCameraAngle_deg;
    public float mainMenuCameraRadius_m;
    public float mainMenuCameraAltitude_m;
    private float previousTerrainHeight_m;

    // Golfball
    public Rigidbody golfBall;
    public GameObject golfBallObject;

    // GameManger
    //public GameObject gameManager;
    private GameObject lowPolyTerrain;
    private GameObject uiManager;

    // Start is called before the first frame update
    void Start()
    {
        // Get the UI Manager Game Object
        GameObject[] terrains = GameObject.FindGameObjectsWithTag("Terrain");
        foreach (GameObject terrainObject in terrains)
        {
            lowPolyTerrain = terrainObject;
            break;
        }

        // Set Screen Reolution to try reduce rendering workload on Android
        //Screen.SetResolution(640, 480, true);

        smoothTime_s = 0.03f;
        smoothTimeXZ_s = 0.012f;
        rotateSmoothTime_s = 0.00f;
        //offsetCameraLook_m.Set(0.0f, 0.35f, 0.0f);
        offsetCameraLook_m.Set(0.0f, 0.3855f, 0.0f);
        defaultLookDownAngle_deg = 15.0f;
        defaultOffsetPosRef_m.Set(0.0f, 0.65f, -1.0f);
        mainMenuPos_m.Set(0.0f, 500.0f, 0.0f);
        mainMenuCameraAngle_deg = 0.0f;
        mainMenuCameraRadius_m = 200.0f;
        mainMenuCameraAltitude_m = 100.0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        UpdateTranslationAndRotation();
    }

    void UpdateTranslationAndRotation()
    {

        // ------------ TRANSLATION -------------

        // Standard position offset
        offsetPosRef_m = defaultOffsetPosRef_m;

        // Rotate offset position in space based on dirAngle_deg from the ball
        //offsetPosRef_m = Quaternion.Euler(0.0f, golfBallObject.GetComponent<BallMotion>().dirAngle_deg, 0.0f) * offsetPosRef_m;
        offsetPosRef_m = Quaternion.Euler(0.0f, 0.0f, 0.0f) * offsetPosRef_m;

        // Increase offsetPosRef_m by a little bit based on velocity of the ball
        offsetPosRef_m.x *= (golfBall.velocity.magnitude / 20.0f + 1) * (Time.smoothDeltaTime + 1);
        offsetPosRef_m.y *= (golfBall.velocity.magnitude / 100.0f + 1) * (Time.smoothDeltaTime + 1);
        offsetPosRef_m.z *= (golfBall.velocity.magnitude / 20.0f + 1) * (Time.smoothDeltaTime + 1);

        // Follow the golf ball
        referencePos_m *= Time.smoothDeltaTime;
        smoothTime_s = 0.03f / (Mathf.Abs(golfBall.velocity.y) / 70.0f + 1);

        // Add extra y offset if camera is going to go underground
        float terrainHeight_m;
        //lowPolyTerrain.transform.TransformPoint()
        terrainHeight_m = calculateTerrainHeight();

        previousTerrainHeight_m = terrainHeight_m;
        //Debug.Log(terrainHeight_m);
        bool lookDownALittle = false;
        if (golfBall.position.y + offsetPosRef_m.y < terrainHeight_m + offsetCameraLook_m.y)
        {
            lookDownALittle = true;
            offsetPosRef_m.y = terrainHeight_m + offsetCameraLook_m.y - golfBall.position.y;
        }

        // Calculate smoothdamp positions separately for y and xz
        Vector3 smoothDampPosY = Vector3.SmoothDamp(transform.position, golfBall.position + offsetPosRef_m, ref referencePos_m, smoothTime_s);
        Vector3 smoothDampPosXZ = Vector3.SmoothDamp(transform.position, golfBall.position + offsetPosRef_m, ref referencePos_m, smoothTimeXZ_s);

        // Moved to the combined smoothdamp position
        transform.position = new Vector3(smoothDampPosXZ.x, smoothDampPosY.y, smoothDampPosXZ.z);


        // ------------ ROTATION ----------------

        // Calculate the lookrotation quaternion for looking just above the ball
        Quaternion ballRotation = Quaternion.LookRotation(golfBall.position + offsetCameraLook_m - transform.position);

        // Calculate lookdown angle if near terrain or in hole
        float lookDownAngle = defaultLookDownAngle_deg;
        if (lookDownALittle)
        {
            lookDownAngle = ballRotation.eulerAngles.x;
        }

        // Rotate the camera in roll and yaw, but keep pitch the same 
        transform.rotation = Quaternion.Euler(lookDownAngle, ballRotation.eulerAngles.y, ballRotation.eulerAngles.z);

    }

    public float calculateTerrainHeight()
    {

        // Raycast down behind the ball to figure out the real min distance to the terrain. This will determina hasLanded.
        RaycastHit hit;
        Vector3 raycastOffset = new Vector3(0.0f, -0.1f, 0.0f);
        Vector3 inHoleOffset = new Vector3(0.0f, 1.663238f, 0.0f);

        // Cast a ray down from an offset behind the ball
        if (Physics.Raycast(transform.position + raycastOffset, Vector3.down, out hit, 300.0f))
        {
            if (hit.collider.gameObject.tag == "Terrain")
            {
                //Debug.Log("Hit Terrain: " + (transform.position.y - hit.distance));
                return transform.position.y - hit.distance;
            }
        }

        //Debug.Log("No Hit: " + previousTerrainHeight_m);
        // If terrain is not hit, it means we're probably already on the ground, so return 0.0f
        return previousTerrainHeight_m;
    }
    public void UpdateAfterStop()
    {
        FixedUpdate();
    }


}

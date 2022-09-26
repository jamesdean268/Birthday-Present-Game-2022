using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        int size = 16;
        for(int i = -size; i <= size; i++)
        {
            for (int j = 1; j <= size; j++)
            {
                // Create Cube
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.position = new Vector3(i, j, 50.0f);
                var cubeRenderer = cube.GetComponent<Renderer>();
                cubeRenderer.material.SetColor("_Color", Color.red);
                Rigidbody cubeRigidBody = cube.AddComponent<Rigidbody>();
                cubeRigidBody.collisionDetectionMode = CollisionDetectionMode.Continuous;

                // Scale so that they stand up
                float scale = 1 - j / 100 * 0.95f;
                cube.transform.localScale = new Vector3(scale, 0.9995f, scale);
            }
        }
        
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

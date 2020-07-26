using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script must be utlised as the core component on the 'vehicle' obstacle in the frogger game.
/// </summary>
public class VehicleController : MonoBehaviour
{
    /// <summary>
    /// -1 = left, 1 = right
    /// </summary>
    public int moveDirection; //This variabe is to be used to indicate the direction the vehicle is moving in.
    public float speed; //This variable is to be used to control the speed of the vehicle.
    public Vector2 startingPosition; //This variable is to be used to indicate where on the map the vehicle starts (or spawns)
    public Vector2 endPosition; //This variablle is to be used to indicate the final destination of the vehicle.
    float screenEdgeX = 18.5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.left * speed * moveDirection * Time.deltaTime;
        float xPos = transform.position.x;
        if(xPos > screenEdgeX || xPos < -screenEdgeX)
        {
            transform.position = new Vector3(xPos * -1, transform.position.y, transform.position.z);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Enemy")
        {
            print("GOODBYE");
            Destroy(gameObject);
        }
    }

}

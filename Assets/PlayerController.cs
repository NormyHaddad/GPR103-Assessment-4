using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShortcutManagement;
using UnityEngine;

/// <summary>
/// This script must be used as the core Player script for managing the player character in the game.
/// </summary>
public class PlayerController : MonoBehaviour
{
    public string playerName = ""; //The players name for the purpose of storing the high score
   
    public int playerTotalLives; //Players total possible lives.
    public int playerLivesRemaining; //PLayers actual lives remaining.
   
    public bool playerIsAlive = true; //Is the player currently alive?
    public bool playerCanMove = false; //Can the player currently move?

    private GameManager myGameManager; //A reference to the GameManager in the scene.

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
       if (Input.GetKeyDown(KeyCode.W))
       {
           transform.position += Vector3.up;
       }
       if (Input.GetKeyDown(KeyCode.A))
       {
           transform.position += Vector3.left;
       }
       if (Input.GetKeyDown(KeyCode.S))
       {
           transform.position += Vector3.down;
       }
       if (Input.GetKeyDown(KeyCode.D))
       {
            //transform.position += Vector3.right;
            GetComponent<Rigidbody2D>().AddForce(new Vector2(50, 0));
       }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Instantiate(RedPH, transform.position, Quaternion.identity);
        if (gameObject.tag == "Enemy")
        {
            Destroy(this.gameObject);
        }
    }
}

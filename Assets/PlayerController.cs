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

    public GameManager myGameManager; //A reference to the GameManager in the scene.

    // Start is called before the first frame update
    void Start()
    {
        playerLivesRemaining = playerTotalLives;
        gameObject.transform.position = myGameManager.spawnpoint;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) && gameObject.transform.position.y < myGameManager.levelConstraintTop)
        {
            transform.SetParent(null);
            transform.position += Vector3.up;
       }
       if (Input.GetKeyDown(KeyCode.A) && gameObject.transform.position.x > myGameManager.levelConstraintLeft)
       {
            transform.SetParent(null);
            transform.position += Vector3.left;
       }
       if (Input.GetKeyDown(KeyCode.S) && gameObject.transform.position.y > myGameManager.levelConstraintBottom)
       {
            transform.SetParent(null);
            transform.position += Vector3.down;
       }
       if (Input.GetKeyDown(KeyCode.D) && gameObject.transform.position.x < myGameManager.levelConstraintRight)
       {
            transform.SetParent(null);
            transform.position += Vector3.right;
       }
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.GetComponent<VehicleController>() != null) 
        {
            if (playerLivesRemaining > 0)
            {
                //gameObject.SetActive(false);
                gameObject.transform.position = myGameManager.spawnpoint;
                print("DEATH");
                playerLivesRemaining -= 1;
                //gameObject.SetActive(true);
            }
            else
            {
                gameObject.SetActive(false);
                print("Game Over");
            }
        }
        if (col.transform.GetComponent<LogController>() != null)
        {
            transform.SetParent(col.transform);
        }
    }
}

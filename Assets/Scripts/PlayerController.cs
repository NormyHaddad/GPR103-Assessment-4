using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShortcutManagement;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public string playerName; //The players name for the purpose of storing the high score
   
    public int playerTotalLives; //Players total possible lives.
    public int playerLivesRemaining; //PLayers actual lives remaining.
   
    public bool playerIsAlive = true; //Is the player currently alive?
    public bool playerCanMove = false; //Can the player currently move?

    public AudioSource soundController;
    public AudioClip jumpSound;
    public AudioClip deathSound;
    public AudioClip endzoneSound;

    public GameObject deathFxPrefab;
    public GameObject deathFxMini;
    public Sprite winSprite;

    public GameManager myGameManager; //A reference to the GameManager in the scene.

    private int numSafe;
    private int playerScore;
    private int highScore;

    void Start()
    {
        numSafe = 4;
        playerLivesRemaining = playerTotalLives;
        gameObject.transform.position = myGameManager.spawnpoint;
        PlayerPrefs.SetString("PlayerName", playerName);
    }

    void Update()
    {
        if (playerIsAlive)
        {
            if (Input.GetKeyDown(KeyCode.W) && gameObject.transform.position.y < myGameManager.levelConstraintTop)
            {
                transform.SetParent(null);
                transform.position += Vector3.up;
                playSound(jumpSound, 1.2f);
            }
            if (Input.GetKeyDown(KeyCode.A) && gameObject.transform.position.x > myGameManager.levelConstraintLeft)
            {
                transform.SetParent(null);
                transform.position += Vector3.left;
                playSound(jumpSound);
            }
            if (Input.GetKeyDown(KeyCode.S) && gameObject.transform.position.y > myGameManager.levelConstraintBottom)
            {
                transform.SetParent(null);
                transform.position += Vector3.down;
                playSound(jumpSound, 0.8f);
            }
            if (Input.GetKeyDown(KeyCode.D) && gameObject.transform.position.x < myGameManager.levelConstraintRight)
            {
                transform.SetParent(null);
                transform.position += Vector3.right;
                playSound(jumpSound);
            }

            if (numSafe >= 5)
            {
                myGameManager.GameOver();
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.GetComponent<VehicleController>() != null) 
        {
            if (playerLivesRemaining > 0)
            {
                Instantiate(deathFxMini, new Vector3(transform.position.x + 0.5f, transform.position.y + 0.5f, transform.position.z), Quaternion.identity);
                gameObject.transform.position = myGameManager.spawnpoint;
                playerLivesRemaining -= 1;
                playSound(deathSound, 2.0f);
            }
            else
            {
                Instantiate(deathFxPrefab, new Vector3(transform.position.x + 0.5f, transform.position.y + 0.5f, transform.position.z), Quaternion.identity);
                playSound(deathSound);
                playerIsAlive = false;
                gameObject.transform.position = myGameManager.spawnpoint;
                GetComponent<SpriteRenderer>().enabled = false;
                myGameManager.GameOver(false);
            }
        }
        if (col.transform.GetComponent<LogController>() != null)
        {
            transform.SetParent(col.transform);
        }
        if (col.transform.tag == "Endzone")
        {
            if (numSafe < 5)
            {
                numSafe += 1;
                playerScore += 100;
                myGameManager.AddScore(playerScore);
                col.GetComponent<SpriteRenderer>().sprite = winSprite;
                col.GetComponent<BoxCollider2D>().enabled = false;
                gameObject.transform.position = myGameManager.spawnpoint;
                playSound(endzoneSound);
            }
            else if (numSafe >= 5)
            {
                myGameManager.GameOver();
                if (playerScore > highScore)
                {
                    highScore = playerScore;
                    PlayerPrefs.SetInt("HighScore", highScore);
                }
            }
        }
    }

    void playSound(AudioClip sound, float pitch = 1.0f)
    {
        soundController.clip = sound;
        soundController.pitch = pitch;
        soundController.Play();
    }
}

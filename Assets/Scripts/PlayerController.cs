using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{  
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

    public TMP_Text highscoreWindow;

    private int numSafe;
    private int playerScore;
    private int highScore;
    private float timer = 0;

    private bool onLog = false;
    private bool inWater = false;

    void Start()
    {
        PlayerPrefs.DeleteAll();
        numSafe = 0;
        playerLivesRemaining = playerTotalLives;
        gameObject.transform.position = myGameManager.spawnpoint;
        highScore = PlayerPrefs.GetInt("HighScore");
        highscoreWindow.text = "Highscore: " + highScore.ToString();
    }

    void Update()
    {
        timer += 1 * Time.deltaTime;
        if (playerIsAlive)
        {
            if (Input.GetKeyDown(KeyCode.W) && gameObject.transform.position.y < myGameManager.levelConstraintTop)
            {
                transform.SetParent(null);
                transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
                playSound(jumpSound, 1.2f);
            }
            if (Input.GetKeyDown(KeyCode.A) && gameObject.transform.position.x > myGameManager.levelConstraintLeft)
            {
                transform.SetParent(null);
                transform.position = new Vector3(transform.position.x - 1, transform.position.y, transform.position.z);
                playSound(jumpSound);
            }
            if (Input.GetKeyDown(KeyCode.S) && gameObject.transform.position.y > myGameManager.levelConstraintBottom)
            {
                transform.SetParent(null);
                transform.position = new Vector3(transform.position.x, transform.position.y - 1, transform.position.z);
                playSound(jumpSound, 0.8f);
            }
            if (Input.GetKeyDown(KeyCode.D) && gameObject.transform.position.x < myGameManager.levelConstraintRight)
            {
                transform.SetParent(null);
                transform.position = new Vector3(transform.position.x + 1, transform.position.y, transform.position.z);
                playSound(jumpSound);
            }

            if (numSafe >= 5)
            {
                myGameManager.GameOver();
            }

            if (transform.position.x >= myGameManager.levelConstraintRight + 0.5f || transform.position.x <= myGameManager.levelConstraintLeft - 0.5f)
            {
                KillPlayer();
            }
        }

    }

    private void LateUpdate()
    {
        if (playerIsAlive)
        {
            if (!onLog && inWater)
            {
                print("ded");
                KillPlayer();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.GetComponent<VehicleController>()) 
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
        if (col.transform.GetComponent<LogController>())
        {
            onLog = true;
            transform.position = col.transform.position;
            transform.SetParent(col.transform);
        }
        if (col.transform.tag == "Endzone")
        {
            if (numSafe < 5)
            {
                numSafe += 1;
                playerScore += 100 - Mathf.RoundToInt(timer);
                myGameManager.UpdateScore(Mathf.RoundToInt(playerScore));
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
        if (col.transform.tag == "Water")
        {
            print("drown");
            inWater = true;
        }
    }

    private void OnTriggerExit2D(Collider2D Collision)
    {
        if (playerIsAlive)
        {
            if (Collision.GetComponent<LogController>())
            {
                onLog = false;
                print("No longer on log");
                transform.SetParent(null);
            }
            if (Collision.transform.tag == "Water")
            {
                print("not drown");
                inWater = false;
            }
        }
    }

    void playSound(AudioClip sound, float pitch = 1.0f)
    {
        soundController.clip = sound;
        soundController.pitch = pitch;
        soundController.Play();
    }

    void KillPlayer()
    {
        playSound(deathSound);  
        playerIsAlive = false;
        //take damage
        playerCanMove = false;
        Instantiate(deathFxPrefab, transform.position, Quaternion.identity);
        GetComponent<SpriteRenderer>().enabled = false;
        if (!playerIsAlive)
        {
            //turn on the game over window
            myGameManager.GameOver(false);
            myGameManager.endScreen.SetActive(true);
        }
    }
}

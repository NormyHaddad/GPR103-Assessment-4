﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogController : MonoBehaviour
{
    private GameManager myGameManager;

    public int moveDirection; //This variabe is to be used to indicate the direction the vehicle is moving in.
    public float speed; //This variable is to be used to control the speed of the vehicle.
    public Vector2 startPosition; //This variable is to be used to indicate where on the map the vehicle starts (or spawns)
    public Vector2 endPosition; //This variablle is to be used to indicate the final destination of the vehicle.
    // Start is called before the first frame update

    void Start()
    {
        myGameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        transform.position = startPosition;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.right * Time.deltaTime * speed * moveDirection);
        if ((transform.position.x * moveDirection) > (endPosition.x * moveDirection))
        {
            transform.position = startPosition;
        }
    }
}

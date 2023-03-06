using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    byte[] scoreByte = new byte[11];

    // Move the ball
    // Manage the score
    // Manage the turns

    public GameObject ball;
    int score = 0;
    int turnCounter = 0;
    GameObject[] pins;
    public Text scoreUI;

    Vector3[] positions;
    public HighScore highScore;

    public GameObject menu;


    // Start is called before the first frame update
    void Start()
    {
        pins = GameObject.FindGameObjectsWithTag("Pin");
        positions = new Vector3[pins.Length];

        for(int i = 0; i < pins.Length; i++) 
        {
            positions[i] = pins[i].transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        MoveBall();

        if(Input.GetKeyDown(KeyCode.Space) || ball.transform.position.y < 20) 
        {
            CountPinsDown();
            turnCounter++;
            // ResetPins();

            if(turnCounter == 10) 
            {
                menu.SetActive(true);
            }
        }
        if (Input.GetKeyDown(KeyCode.R)) 
        {
            Reset();
        }
    }

    private void ResetPins()
    {
        throw new NotImplementedException();
    }

    void MoveBall()
    {
        Vector3 position = ball.transform.position;
        position += Vector3.right * Input.GetAxis("Horizontal") * Time.deltaTime;
        position.x = Mathf.Clamp(position.x, -0.525f, 0.525f);
        ball.transform.position = position;
        // ball.transform.Translate(Vector3.right * Input.GetAxis("Horizontal")* Time.deltaTime); 
    }

    void CountPinsDown() 
    {
        for(int i = 0; i < pins.Length; i++) 
        {
            if (pins[i].transform.eulerAngles.z > 5 && pins[i].transform.eulerAngles.z < 355 && pins[i].activeSelf)
            {
                score++;
                scoreByte[i] = 1;
                pins[i].SetActive(false);
            }
        }

        scoreUI.text = score.ToString();

        if(score > highScore.highScore) 
        {
            highScore.highScore = score;    
        }

        Debug.Log(highScore.highScore);
    }

    private void Reset()
    {
        CheckByteArray();

       for(int i = 0; i < pins.Length; i++) 
        {
            pins[i].SetActive(true);
            pins[i].transform.position = positions[i];
            pins[i].GetComponent<Rigidbody>().velocity = Vector3.zero;
            pins[i].GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            pins[i].transform.rotation = Quaternion.identity;
        }

        ball.transform.position = new Vector3(0, 0.108F, -9F);
        ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
        ball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        ball.transform.rotation = Quaternion.identity;
    }

    private void CheckByteArray()
    {
        int knockDown = 0;
        for(int i = 0; i < 10; i++) 
        {
            if (scoreByte[i] == 1) 
            {
                knockDown++;
            }   
        }
    }

}


//Step 1
// Change the scoreByte with pins that went down, like scoreByte[5] = 1;

//Step 2
// Analyse the byte array. Check how many "1" you get on first 10 bits. 0, 0, 0, 0, 1, 0 ,1, 0, 1, 0 ,0
// Go through the first 10 entries of the array, check how many of them are 1.

//Step 3
//Show the score 

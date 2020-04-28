﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerSteadyBoat : MonoBehaviour
{

    //I assume this script should inherit from Bastians scripts.

    //Get access to manager
    ourGameManager manager;
    bool experimentalCondition; //false = discrete, true = continuous
    bool inWave;
    public bool controlstate;

    //Doing the Key Sequence. As it stands, T and R does not need to pressed at the same time, only the order matters.
    bool attemptStarted;
    float sequenceInputTime;
    private float timePassed;
    //Create key sequence
    KeyCode[] sequence = new KeyCode[]
    {
        KeyCode.T,
        KeyCode.R,
        KeyCode.W,
        KeyCode.E
    };
    int sequenceIndex = 0;



    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("manager").GetComponent<ourGameManager>();
        experimentalCondition = manager.experimentalCondition;
        sequenceInputTime = manager.sequenceInputTime;
    }

    // Update is called once per frame
    void Update()
    {

        if(inWave == true)
        {
            

            if (experimentalCondition == false) //Discrete version
            {
                steadyingDiscrete();
            }
            else // continuous version
            {
                steadyingContinuous();
            }
        }


        if (attemptStarted == true)
        {
            timePassed += Time.deltaTime;
        }
    }


    bool inputtingSequence()
    {
        //Debug.Log("hey");

        if (Input.GetKeyDown(sequence[0])) //If input is the very first key in the sequence, assume user is trying to start sequence from the beginning, and reset timer
        {
            Debug.Log("Sequence Started");
            timePassed = 0;
            attemptStarted = true;
        }

        if (Input.GetKeyDown(sequence[sequenceIndex])) //If the key pressed is the next key we needed to press in the sequence, check that...
        {
            sequenceIndex += 1;
            Debug.Log("correct");
            if (sequenceIndex == sequence.Length) // sequenceIndex is equal to the length of the sequence array, because if it is, we have correctly done the sequence
            {
                resetSequenceAttempt();
                return true; //return true, so we can do whatever we need to do if we are using either discrete or continuous input.
            }
        }
        else if (Input.anyKeyDown || timePassed > sequenceInputTime) //else, if we input any other key, or spend too long trying to input the sequence, start over.
        {
            Debug.Log("failed");
            resetSequenceAttempt();
        }
        return false;
    }

    void resetSequenceAttempt() //Reset all the stuff we need to check whether correct sequence has been typed.
    {
        sequenceIndex = 0;
        timePassed = 0;
        attemptStarted = false;
    }

    void steadyingDiscrete()
    {
        if(inputtingSequence() == true)
        {
            Debug.Log("This Totally Works");
        }
    }

    void steadyingContinuous()
    {
        if (inputtingSequence() == true)
        {
            Debug.Log("This Totally Works");
        }
    }


    private void OnTriggerStay2D(Collider2D collision) //As long as we are in contact with a wave, allow the player to enter the keysequences.
    {
        if (collision.tag == "wave" && controlstate == true)
        {
            inWave = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) //Once a wave has passed, or we steadied boat to make it go away, reset sequence index and timePassed.
    {
        
        if (collision.tag == "wave")
        {
            resetSequenceAttempt();
            inWave = false;
        }
    }



}
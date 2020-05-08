using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerSteadyBoat : MonoBehaviour
{

    //I assume this script should inherit from Bastians scripts.

    //Get access to manager
    ourGameManager manager;
    public bool controlstate;
    public bool tryingToSteady;
    float currentTilt;
    float maxTilt = -20;
    float tiltSpeed = 10;
    float inputDelay;
    float timer;


    //Doing the Key Sequence. As it stands, T and R does not need to pressed at the same time, only the order matters.
    bool inWave;
    bool experimentalCondition;
    bool sequenceOver;
    float inputAccuracy;
    int numberOfContinuousInputsNeeded;
    bool attemptStarted;
    float sequenceInputTime;
    private float timePassed;
    bool fishingAttemptUsed; //Used to ensure player doesn't continue getting chances to catch fish after input sequence has been correctly entered.
    int correctContinuousInputs;
    bool inputResgisteredCorrectly;
    float roll;
    float accuracyModifier; //Change this to ensure a player catches a fish next time they input correct sequence, or make sure they DON'T catch a fish, in case they've gotten too many in a row.


    //Ensuring it takes exacty 20 attempts to catch 12 fish, IF, and only IF, the player inputs correct sequence at least 12 times.
    int fishStreak;
    int fishStillNeeded = 12;
    int attemptsLeft = 20;

    //Fun Data to Log
    int windowCorrectInputs;
    int windowInputAtteempts;
    float lastKeyPressTime;

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
        sequenceInputTime = manager.sequenceInputTime;
        inputAccuracy = manager.inputAccuracy;
        experimentalCondition = manager.experimentalCondition;
        numberOfContinuousInputsNeeded = manager.numberOfContinuousInputsNeeded;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (controlstate)
        {
            if (inWave)
            {
                inputtingSequence();
            }


            if (attemptStarted == true)
            {
                timePassed += Time.deltaTime;
            }
            
        }

        if (tryingToSteady)
        {
            timer += Time.deltaTime;
            //Debug.Log(transform.eulerAngles.z);
            if (currentTilt < 0 && timer > inputDelay)
            {
                transform.eulerAngles += new Vector3(0, 0, tiltSpeed*3f) * Time.deltaTime;
                currentTilt += tiltSpeed * 3f * Time.deltaTime;
                if(transform.eulerAngles.z < 100)
                {
                    inWave = false;
                    tryingToSteady = false;
                    timer = 0;
                }
            }
        }
    }


    void inputtingSequence()
    {
        //Debug.Log("hey");
        if (Input.anyKeyDown)
        {
            string correctKeyOrNot = "Failed";
            string expectedKey = sequence[sequenceIndex].ToString();


            if (Input.GetKeyDown(sequence[0])) //If input is the very first key in the sequence, assume user is trying to start sequence from the beginning, and reset timer
            {
                Debug.Log("Sequence Started");
                timePassed = 0;
                attemptStarted = true;
            }

            if (Input.GetKeyDown(sequence[sequenceIndex])) //If the key pressed is the next key we needed to press in the sequence, check that...
            {
                correctKeyOrNot = "Correct";
                

                sequenceIndex += 1;
                if (sequenceIndex == sequence.Length) // sequenceIndex is equal to the length of the sequence array, because if it is, we have correctly done the sequence
                {
                    Debug.Log("Correct Sequence Entered");
                    //tryingToSteady = true;
                    inputDelay = Random.value;
                    correctInput();
                    sequenceOver = true;
                }
            }
            else if (Input.anyKeyDown || timePassed > sequenceInputTime) //else, if we input any other key, or spend too long trying to input the sequence, start over.
            {
                Debug.Log("failed");               
                sequenceOver = true;
            }

            manager.keyPressedLog(Input.inputString, correctKeyOrNot, expectedKey, (timePassed - lastKeyPressTime).ToString());

            lastKeyPressTime = timePassed;

            if (sequenceOver)
            {
                //manager.keySequenceCompleteLog();
                sequenceOver = false;               
                resetSequenceAttempt();
            }
        }
        
    }

    void resetSequenceAttempt() //Reset all the stuff we need to check whether correct sequence has been typed.
    {
        lastKeyPressTime = 0;
        sequenceIndex = 0;
        timePassed = 0;
        attemptStarted = false;
    }


    public void correctInput() // called from the playerSteadyBoat script whenever a key sequence has been correctly inputted? Handle it differently based on experimental condition.
    {
        if (experimentalCondition == false) //Discrete condition
        {
            if (rollDice() && fishingAttemptUsed == false) //Catching a fish
            {
                Debug.Log("Input registered, how lucky!");
                tryingToSteady = true;
                inputResgisteredCorrectly = true;
                fishStillNeeded -= 1;

            }
            else if (roll < inputAccuracy)
            {
                tryingToSteady = true;
            }
            fishingAttemptUsed = true; //This is done to only give the user one correct attempt in the discrete version.
        }
        else //Continuous condition
        {
            correctContinuousInputs += 1; //Counting up everytime they do it correcly. Reach some number before we try to roll dice.

            if (correctContinuousInputs >= numberOfContinuousInputsNeeded && rollDice() && fishingAttemptUsed == false)
            {
                tryingToSteady = true;
                inputResgisteredCorrectly = true;
                fishingAttemptUsed = true;

            }
            else if (roll < inputAccuracy)
            {
                tryingToSteady = true;
            }


        }
    }

    bool rollDice()
    {
        roll = Random.value;
        if (roll < inputAccuracy + accuracyModifier)
        {
            return true;
        }

        return false;
    }

    public void waveGoodbye() //Called when a wave has passed the boat. Called from playerSteadyBoat, in the onTriggerExit2D function. Used to let us exit fishing screen if we missed all waves, without trying to input any key sequence.
    {
        correctContinuousInputs = 0;
        tryingToSteady = true;

        if (inputResgisteredCorrectly)//Change accuracyModifer based on whether fish was caught or not.
        {
            Debug.Log("fishCaught");
            accuracyModifier = 0; //Max two fish in a row rule applied here, as well as rest of accuracy modifier when you catch a fish after being guaranteed one from failing earlier.
            fishStreak += 1;
            if (fishStreak == 2)
            {
                accuracyModifier = -1;
                fishStreak = 0;
            }
        }
        else
        {
            fishStreak = 0;
            accuracyModifier = 1;
        }



        attemptsLeft -= 1;
        if (fishStillNeeded >= attemptsLeft)
        {
            accuracyModifier = 1;
        }
        else if (fishStillNeeded == 1 && attemptsLeft > 1)
        {
            accuracyModifier = -1;
        }

        fishingAttemptUsed = false;

        manager.inputWindowOver(inputResgisteredCorrectly); //Update UI and log data.

        inputResgisteredCorrectly = false;
    }

    void tiltBoat()
    {
        if (currentTilt > maxTilt && inWave && tryingToSteady == false)
        {
            transform.eulerAngles += new Vector3(0, 0, -tiltSpeed) * Time.deltaTime;
            currentTilt += -tiltSpeed * Time.deltaTime;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "wave")
        {
            manager.prepareText.SetActive(false);
            manager.steadyText.SetActive(true);
            inWave = true;
            //tryingToSteady = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision) //As long as we are in contact with a wave, allow the player to enter the keysequences.
    {
        if (collision.tag == "wave")
        {           
            tiltBoat();
        }
    }

    private void OnTriggerExit2D(Collider2D collision) 
    {
        
        if (collision.tag == "wave")
        {
            waveGoodbye();
            inputDelay = 0;
            inWave = false;
            resetSequenceAttempt();
        }
    }




}

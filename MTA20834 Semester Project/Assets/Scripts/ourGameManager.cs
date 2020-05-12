using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ourGameManager : MonoBehaviour
{
    //Things we need to log
    [SerializeField]
    private LoggingManager loggingManager;

    string currentCondition;
    int bubbleNumber;
    int fishCaught;
    int instantFrustrationLevel, globalFrustrationLevel;
    

    //This script should be responsible for correctly transitioning between sea screen and fishing screen.
    //Can also use this to update interface images.  
    public bool experimentalCondition; //false = discrete, true = continuous
    
    public float sequenceInputTime; //Time the user has to input the sequence. 1 second in Bastians game
    public float inputAccuracy; //Percentage chance of input being registered. Needs to be value between 0 and 1, with 1 = 100 % chance of input being registered.
    public int numberOfContinuousInputsNeeded; //How many correct inputs need to be registered in continuous output?
    
    //Things we need access to.
    Player playerScript;
    playerFishing fishingScript;
    playerSteadyBoat steadyScript;
    FishAI fishAIScript;
    public GameObject playerBoat;
    public GameObject playerFishing;
    public GameObject playerSteady;
    public QuestSystem QuestSystem;

    int currentScreen;
    bool intro = false;
    public bool questionnaireTime;
    bool frustationQuestionNumber;

    public int currentLocation = 1;

    //Camera
    public Camera theCamera;
    public Transform fishingCloseup;

    //Spawning Fishies
    public Transform fishHoldingPen;
    public GameObject[] fishySpecies;
    public Transform[] fishSpawnLocations;

    GameObject hookedFish;
    public GameObject[] currentFish = new GameObject[3];

    //Spawning Waves and steadying the boat
    public GameObject[] waves;
    int correctContinuousInputs;
    int wavesPassed;

    //Updating UI Elements
    public Image controlsWASD, hookUpDown;
    public Text controlsWASDText, hookUpDownText;
    public GameObject prepareText, steadyText, introScreen, instantfrustation, globalfrustration;

    string[] numberKeys = new string[] //The inputs available when rating frustration, in order from lowest frustration to highest. 0 = 10.
    {
        "1",
        "2",
        "3",
        "4",
        "5",
        "6",
        "7",
        "8",
        "9",
        "0"
    };


    // Start is called before the first frame update
    void Start()
    {

        if (experimentalCondition)
        {
            currentCondition = "Continuous";
        }
        else
        {
            currentCondition = "Discrete";
        }

        //Spawn all the fish we need, and make them inactive until we need them.
        for (int i = 0; i < fishySpecies.Length; i++)
        {
            fishySpecies[i] = Instantiate(fishySpecies[i], fishHoldingPen);
            fishySpecies[i].GetComponent<FishAI>().qst = QuestSystem;
            fishySpecies[i].SetActive(false);
            fishySpecies[i].transform.parent = null;
        }
        //Setup player scripts and stuff
        playerScript = playerBoat.GetComponent<Player>();
        fishingScript = playerFishing.GetComponent<playerFishing>();
        steadyScript = playerSteady.GetComponent<playerSteadyBoat>();

        //Making the player only in control of boat movement at the beginning
        playerScript.controlstate = false;
        fishingScript.controlstate = false;
        steadyScript.controlstate = false;

        //UI setup
        
        
        
        hookUpDownText.enabled = false;
        hookUpDown.enabled = false;

        //KeySequence


        //Potentially useful function that lets us load things from the Assets directly:
        //Resources.Load
        prepareText.SetActive(false);
        steadyText.SetActive(false);
        instantfrustation.SetActive(false);
        globalfrustration.SetActive(false);


        loggingManager.AddNewEvent("Game Started!", currentCondition);

    }

    // Update is called once per frame
    void Update()
    {

        if(intro == false)
        {
            if (Input.GetKeyDown("return"))
            {
                introScreen.SetActive(false);
                playerScript.controlstate = true;
                intro = true;
            }
        }

        if (currentScreen == 0) //This is being called each frame, not efficient, should just be turned into a method and called once when needed. Probably in switch controlstate. 
        {
            controlsWASD.enabled = true;
            controlsWASDText.enabled = true;
            hookUpDownText.enabled = false;
            hookUpDown.enabled = false;
            theCamera.transform.position = new Vector3(playerBoat.transform.position.x+1.2f, playerBoat.transform.position.y, -10);
        } else
        {
            controlsWASDText.enabled = false;
            controlsWASD.enabled = false;
            hookUpDownText.enabled = true;
            hookUpDown.enabled = true;
        }

        if (questionnaireTime)
        {
            if (Input.anyKeyDown)
            {
                if (frustationQuestionNumber == false)
                {
                    if(badWayToCheckKeys() > 0)
                    {
                        instantfrustation.SetActive(false);
                        globalfrustration.SetActive(true);
                        instantFrustrationLevel = badWayToCheckKeys();
                        frustationQuestionNumber = true;
                    }
                    
                }
                else
                {
                    if(badWayToCheckKeys() > 0)
                    {
                        globalfrustration.SetActive(false);
                        globalFrustrationLevel = badWayToCheckKeys();
                        questionnaireTime = false;
                        playerScript.controlstate = true;
                        frustationQuestionNumber = false;
                        Debug.Log(instantFrustrationLevel + " and " + globalFrustrationLevel);
                        loggingManager.logFrustrationLevels(currentCondition, fishCaught.ToString(), bubbleNumber.ToString(), instantFrustrationLevel.ToString(), globalFrustrationLevel.ToString());
                    }
                }
            }
        }

    }


    //Public functions accessed by others

    public void switchControlState(int currentState) //Change control images and so on in here.
    {

        switch (currentState)
        {
            case 0: //Changing from travel screen to fishing screen, where the player gains control of the hook.
                playerScript.controlstate = false;
                fishingScript.controlstate = true;
                fishingScript.lowerKeyLetGo = false;
                currentScreen = 1;
                bubbleNumber += 1;
                theCamera.transform.position = new Vector3(fishingCloseup.position.x, fishingCloseup.position.y, -10);

                spawnFishies();

                
                break;

            case 1: //Changing controls when the player has hooked a fish, so they will now have to keep boat steady against waves. Start spawning waves.
                fishingScript.controlstate = false;
                steadyScript.controlstate = true;
                steadyScript.tryingToSteady = false;
                spawnWaves();
                break;

            case 2: //After keeping boat steady, or failing to do so, change back to travel screen, lose control of fish and steadying, and gain control of boat in travel screen.

                if (questionnaireTime)
                {
                    instantfrustation.SetActive(true);
                }
                else
                {                   
                    playerScript.controlstate = true;
                }
                steadyScript.controlstate = false;
                currentScreen = 0;
                hookedFishReset();

                //Also check to see if next fish should be a guaranteed catch or not, assuming correct input is entered.

                break;
        }

    }

    public void spawnFishies()
    {
        switch (currentLocation)
        {
            case 1: //Starting area
                spawn(0);
                break;
            case 2: //Rain Area
                spawn(3);
                break;
            case 3: // Ice area
                spawn(6);
                break;
        }
    }

    void spawn(int addition) //Used in the above function
    {
        for (int i = addition; i <= addition + 2; i++) //We simply have an array of the fish species, and then we go through that array and position them on the fishing screen. So if addition = 1, we spawn fishspecies 1, 2 and 3. If addition = 0, we spawn fishies 0, 1 and 2.
        {
            currentFish[i - addition] = fishySpecies[i];
            currentFish[i - addition].transform.position = fishSpawnLocations[i - addition].transform.position;
            currentFish[i - addition].GetComponent<FishAI>().checkOutline();
            currentFish[i - addition].SetActive(true);
        }
    }


    //Call this function to make fish start moving towards the fishing hook.
    public void startFishMovement()
    {
        for(int i = 0; i < currentFish.Length; i++)
        {
            fishAIScript = currentFish[i].GetComponent<FishAI>();
            fishAIScript.move = true;
        }
    }

    //Called when a fish has collided with the hook
    public void fishOnHook(GameObject fishOnHook)
    {
        hookedFish = fishOnHook;
        fishAIScript = hookedFish.GetComponent<FishAI>();
        fishingScript.somethingOnHook = true;
        switchControlState(1);

    }

    void spawnWaves()
    {
        prepareText.SetActive(true);
        if(experimentalCondition == false) //Discrete wave
        {
            waves[0].SetActive(true);
        }
        else //Continuous waves
        {
            waves[1].SetActive(true);
        }
    }


    void letFishGo() //Called when player fails to keep boat steady. Makes it "escaped", meaning it won't attach to hook again, releases it from hook, and makes it move forward again.
    {
        fishAIScript.escaped = true;
        SoundManager.PlaySound(SoundManager.Sound.fishCaughtFailed);
        hookedFish.transform.parent = null;
        fishAIScript.move = true;
        Debug.Log("Missed Waves, or input not registered");
    }

    void hookedFishReset() //Called after fishCaught, so hookedFish still available
    {       
        fishAIScript.escaped = false;
        fishAIScript.move = false;
        hookedFish.transform.parent = null;
        hookedFish.SetActive(false);
        hookedFish = null;
    }


    public void keyPressedLog(string keyPressed, string correctKey, string keyExpected,  string timeSinceLastKey)
    {
        loggingManager.newKeyInput(currentCondition, currentLocation.ToString(), fishAIScript.fishtype.ToString(), fishCaught.ToString(), bubbleNumber.ToString(), keyPressed, correctKey, keyExpected, timeSinceLastKey);
    }

    public void keySequenceCompleteLog(string sequenceResult, string sequenceTime)
    {
        loggingManager.sequenceComplete(sequenceResult, currentCondition, currentLocation.ToString(), fishAIScript.fishtype.ToString(), fishCaught.ToString(), bubbleNumber.ToString(), sequenceTime);
    }

    public void inputWindowOver(bool success, int correctSequencesEntered, int sequencesFailed, int correctSequencesDiscarded, int totalAttemptsInBubble)
    {
        string fishGot;
        if (success)
        {
            fishingScript.inputSequenceOver = true;
            QuestSystem.updateFishUI(fishAIScript.fishtype); //Updating UI.
            fishCaught += 1;
            fishGot = "Got Fish";
        }
        else
        {
            letFishGo();
            fishingScript.somethingOnHook = false;
            fishingScript.inputSequenceOver = true;
            fishGot = "Fish Escaped";
        }
        //Log Things here using new Logging system from Bastian
        loggingManager.inputWindowOverLog(currentCondition, currentLocation.ToString(), fishAIScript.fishtype.ToString(), fishCaught.ToString(), bubbleNumber.ToString(), 
            correctSequencesEntered.ToString(), sequencesFailed.ToString(), correctSequencesDiscarded.ToString(), totalAttemptsInBubble.ToString(), fishGot);
        steadyText.SetActive(false);
    }

    int badWayToCheckKeys() //Check which number has been pressed, and return the correct value.
    {

        int frustrationValue = 0;

        if (Input.anyKeyDown)
        {
            for(int i = 0; i<numberKeys.Length; i++)
            {
                if (Input.inputString == numberKeys[i].ToString())
                {
                    frustrationValue = i+1;
                }
            }
        }

        return frustrationValue;
    }


    void OnApplicationQuit()
    {
        loggingManager.LogToDisk();
    }
}

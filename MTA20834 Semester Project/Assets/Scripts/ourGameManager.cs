using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ourGameManager : MonoBehaviour
{
    //This script should be responsible for correctly transitioning between sea screen and fishing screen.
    //Can also use this to update interface images.  
    public bool experimentalCondition; //false = discrete, true = continuous
    
    public float sequenceInputTime; //Time the user has to input the sequence. 1 second in Bastians game
    public float inputAccuracy; //Percentage chance of input being registered. Needs to be value between 0 and 1, with 1 = 100 % chance of input being registered.
    public int numberOfContinuousInputsNeeded; //How many correct inputs need to be registered in continuous output?
    float originalInputAccuracy; //Saving the original input accuracy, in case we increase it at some point to ensure they catch some fish and need to reset it afterwards.
    float accuracyModifier; //Change this to ensure a player catches a fish next time they input correct sequence, or make sure they DON'T catch a fish, in case they've gotten too many in a row.
    //Things we need access to.
    Player playerScript;
    playerFishing fishingScript;
    playerSteadyBoat steadyScript;
    Logger loggyboi;
    FishAI fishAIScript;
    public GameObject loggerObject;
    public GameObject playerBoat;
    public GameObject playerFishing;
    public GameObject playerSteady;
    public QuestSystem QuestSystem;

    int currentScreen;

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
    bool fishingAttemptUsed; //Used to ensure player doesn't continue getting chances to catch fish after input sequence has been correctly entered.
    int correctContinuousInputs;
    int wavesPassed;
    bool inputResgisteredCorrectly;
    float roll;

    //Updating UI Elements
    public Image controlsWASD, TRWE, hookUpDown;
    public Text controlsWASDText, TRWEText, hookUpDownText;
    public GameObject prepareText, steadyText;


    //Ensuring it takes exacty 20 attempts to catch 12 fish, IF, and only IF, the player inputs correct sequence at least 12 times.
    int fishStreak;
    int fishStillNeeded = 12;
    int attemptsLeft = 20;



    // Start is called before the first frame update
    void Start()
    {

        loggyboi = loggerObject.GetComponent<Logger>();
        loggyboi.writeCondition(experimentalCondition);
        //Spawn all the fish we need, and make them inactive until we need them.
        for(int i = 0; i < fishySpecies.Length; i++)
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
        playerScript.controlstate = true;
        fishingScript.controlstate = false;
        steadyScript.controlstate = false;

        //UI setup
        
        TRWE.enabled = false;
        TRWEText.enabled = false;
        hookUpDownText.enabled = false;
        hookUpDown.enabled = false;
        
        originalInputAccuracy = inputAccuracy;
        //KeySequence

        //Potentially useful function that lets us load things from the Assets directly:
        //Resources.Load
        prepareText.SetActive(false);
        steadyText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentScreen == 0)
        {
            controlsWASD.enabled = true;
            TRWE.enabled = false;
            TRWEText.enabled = false;
            hookUpDownText.enabled = false;
            hookUpDown.enabled = false;
            theCamera.transform.position = new Vector3(playerBoat.transform.position.x+1.2f, playerBoat.transform.position.y, -10);
        } else
        {
            controlsWASDText.enabled = false;
            controlsWASD.enabled = false;
            TRWE.enabled = true;
            TRWEText.enabled = true;
            hookUpDownText.enabled = true;
            hookUpDown.enabled = true;
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
                steadyScript.controlstate = false;
                playerScript.controlstate = true;

                currentScreen = 0;
                fishingAttemptUsed = false;
                hookedFishReset();
                //controls.sprite = controlImages[0];

                //Also check to see if next fish should be a guaranteed catch or not, assuming correct input is entered.
                attemptsLeft -= 1;
                if (fishStillNeeded >= attemptsLeft)
                {
                    accuracyModifier = 1;
                } else if(fishStillNeeded == 1 && attemptsLeft > 1)
                {
                    accuracyModifier = -1;
                }

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
            for(int i = 0; i < waves.Length; i++)
            {
                waves[i].SetActive(true);
            }
        }
    }


    public void correctInput() // called from the playerSteadyBoat script whenever a key sequence has been correctly inputted? Handle it differently based on experimental condition.
    {
        if(experimentalCondition == false) //Discrete condition
        {
            if (rollDice() && fishingAttemptUsed == false)
            {
                steadyScript.tryingToSteady = true;
                inputResgisteredCorrectly = true;
            } else if (roll < inputAccuracy)
            {
                steadyScript.tryingToSteady = true;
            }
            fishingAttemptUsed = true; //This is done to only give the user one correct attempt in the discrete version.
        }
        else //Continuous condition
        {
            correctContinuousInputs += 1; //Counting up everytime they do it correcly. Reach some number before we try to roll dice.
            
            if (correctContinuousInputs >= numberOfContinuousInputsNeeded && rollDice() && fishingAttemptUsed == false)
            {
                steadyScript.tryingToSteady = true;
                inputResgisteredCorrectly = true;
                fishingAttemptUsed = true;
            }
            else if (roll < inputAccuracy)
            {
                steadyScript.tryingToSteady = true;
            }
            

        }
    }

    bool rollDice()
    {
        roll = Random.value;
        if(roll < inputAccuracy + accuracyModifier)
        {
            return true;
        }

        return false;
    }

    void letFishGo() //Called when player fails to keep boat steady. Makes it "escaped", meaning it won't attach to hook again, releases it from hook, and makes it move forward again.
    {
        fishAIScript.escaped = true;
        SoundManager.PlaySound(SoundManager.Sound.fishCaughtFailed);
        hookedFish.transform.parent = null;
        fishAIScript.move = true;
        accuracyModifier = 1;
        fishStreak = 0;
        Debug.Log("letFishGo");
    }

    void hookedFishReset() //Called after fishCaught, so hookedFish still available
    {
        fishAIScript.escaped = false;
        fishAIScript.move = false;
        hookedFish.transform.parent = null;
        hookedFish.SetActive(false);
        hookedFish = null;
    }

    public void fishCaught()
    {
        Debug.Log("fishCaught");
        QuestSystem.updateFishUI(fishAIScript.fishtype); //Updating UI.
        accuracyModifier = 0; //Max two fish in a row rule applied here, as well as rest of accuracy modifier when you catch a fish after being guaranteed one from failing earlier.
        fishStreak += 1;
        if (fishStreak == 2)
        {
            accuracyModifier = -1;
            fishStreak = 0;
        }
        
    }

    public void waveGoodbye() //Called when a wave has passed the boat. Called from playerSteadyBoat, in the onTriggerExit2D function. Used to let us exit fishing screen if we missed all waves, without trying to input any key sequence.
    {
        wavesPassed += 1;
        if(wavesPassed >= waves.Length || experimentalCondition == false)
        {
            if (inputResgisteredCorrectly)
            {
                fishStillNeeded -= 1;
                Debug.Log("Input registered, how lucky!");
                fishingScript.inputSequenceOver = true;
            }
            else
            {
                //fishingAttemptUsed = true;
                fishingScript.somethingOnHook = false;
                fishingScript.inputSequenceOver = true;
                letFishGo();
                Debug.Log("Missed Waves, or input not registered");
            }
            loggyboi.NewLog(Mathf.Abs(attemptsLeft - 21), Mathf.Abs(fishStillNeeded - 12), fishingAttemptUsed, inputResgisteredCorrectly);
            wavesPassed = 0;
            correctContinuousInputs = 0;
            inputResgisteredCorrectly = false;
            steadyScript.tryingToSteady = true;
            steadyScript.inWave = 0;
            steadyText.SetActive(false);
        }
    }


}

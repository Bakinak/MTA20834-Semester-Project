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
    FishAI fishAIScript;
    public GameObject playerBoat;
    public GameObject playerFishing;
    public GameObject playerSteady;
    

    int currentScreen;

    int currentLocation = 1;

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

    //Updating UI Elements
    public Sprite[] controlImages; //For this to work, boat controls has to be first sprite in array, hook controls second, and boat steady third.
    public Image controls;



    public Text test;

    // Start is called before the first frame update
    void Start()
    {
        //Spawn all the fish we need, and make them inactive until we need them.
        for(int i = 0; i < fishySpecies.Length; i++)
        {
            fishySpecies[i] = Instantiate(fishySpecies[i], fishHoldingPen);
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
        controls.sprite = controlImages[0];
        originalInputAccuracy = inputAccuracy;
        //KeySequence
        
        //Potentially useful function that lets us load things from the Assets directly:
        //Resources.Load
    }

    // Update is called once per frame
    void Update()
    {
        if (currentScreen == 0)
        {
            theCamera.transform.position = new Vector3(playerBoat.transform.position.x+1.2f, playerBoat.transform.position.y, -10);
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

                currentScreen = 1;

                theCamera.transform.position = new Vector3(fishingCloseup.position.x, fishingCloseup.position.y, -10);

                spawnFishies();

                controls.sprite = controlImages[1];
                break;

            case 1: //Changing controls when the player has hooked a fish, so they will now have to keep boat steady against waves. Start spawning waves.
                fishingScript.controlstate = false;
                steadyScript.controlstate = true;
                spawnWaves();
                break;

            case 2: //After keeping boat steady, or failing to do so, change back to travel screen, lose control of fish and steadying, and gain control of boat in travel screen
                steadyScript.controlstate = false;
                playerScript.controlstate = true;

                currentScreen = 0;
                fishingAttemptUsed = false;
                hookedFishReset();
                //controls.sprite = controlImages[0];
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
                spawn(1);
                break;
            case 3: // Ice area
                spawn(2);
                break;
        }
    }

    void spawn(int addition) //Used in the above function
    {
        for (int i = addition; i <= addition + 2; i++) //We simply have an array of the fish species, and then we go through that array and position them on the fishing screen. So if addition = 1, we spawn fishspecies 1, 2 and 3. If addition = 0, we spawn fishies 0, 1 and 2.
        {
            currentFish[i - addition] = fishySpecies[i];
            currentFish[i - addition].transform.position = fishSpawnLocations[i - addition].transform.position;
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
                inputResgisteredCorrectly = true;
            }
            fishingAttemptUsed = true; //This is done to only give the user one correct attempt in the discrete version.
        }
        else //Continuous condition
        {
            correctContinuousInputs += 1; //Counting up everytime they do it correcly. Reach some number before we try to roll dice.
            if (correctContinuousInputs >= numberOfContinuousInputsNeeded && fishingAttemptUsed == false)
            {
                fishingAttemptUsed = true;
                if (rollDice())
                {
                    inputResgisteredCorrectly = true;
                }
            }
        }
    }

    bool rollDice()
    {
        if(Random.value < inputAccuracy + accuracyModifier)
        {
            return true;
        }

        return false;
    }

    void letFishGo() //Called when player fails to keep boat steady. Makes it "escaped", meaning it won't attach to hook again, releases it from hook, and makes it move forward again.
    {
        fishAIScript.escaped = true;
        hookedFish.transform.parent = null;
        fishAIScript.move = true;
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

        //UPDATE UI / QUEST MANAGER THINGY HERE, TO SUCCESFULLY HAVE CAUGHT FISH, MAYBE PLAY HAPPY SOUND, WHO KNOWS.
    }

    public void waveGoodbye() //Called when a wave has passed the boat. Called from playerSteadyBoat, in the onTriggerExit2D function. Used to let us exit fishing screen if we missed all waves, without trying to input any key sequence.
    {
        wavesPassed += 1;
        if(wavesPassed >= waves.Length || experimentalCondition == false)
        {
            if (inputResgisteredCorrectly)
            {
                Debug.Log("Input registered, how lucky!");
                fishingScript.inputSequenceOver = true;
            }
            else
            {
                fishingAttemptUsed = true;
                fishingScript.somethingOnHook = false;
                fishingScript.inputSequenceOver = true;
                letFishGo();
                Debug.Log("Missed Waves, or input not registered");
            }
            wavesPassed = 0;
            correctContinuousInputs = 0;
            inputResgisteredCorrectly = false;

        }
    }


}

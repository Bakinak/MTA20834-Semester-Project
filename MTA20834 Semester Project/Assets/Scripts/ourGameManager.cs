using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ourGameManager : MonoBehaviour
{
    //This script should be responsible for correctly transitioning between sea screen and fishing screen.
    //Can also use this to update interface images. 
    public bool experimentalCondition; //false = discrete, true = continuous
    public float sequenceInputTime;

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
        //steadyScript.controlstate = false;

        //UI setup
        controls.sprite = controlImages[0];
        
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

            case 1: //Changing controls when the player has hooked a fish, so they will now have to keep boat steady against waves
                fishingScript.controlstate = false;
                break;

            case 2: //After keeping boat steady, or failing to do so, change back to travel screen, lose control of fish and steadying, and gain control of boat in travel screen
                playerScript.controlstate = true;
                //fishingScript.controlstate = false;

                currentScreen = 0;
                controls.sprite = controlImages[0];
                break;


        }

    }

    public void spawnFishies()
    {
        switch (currentLocation)
        {
            case 1:
                spawn(0);
                break;
            case 2:
                spawn(3);
                break;
        }
    }

    void spawn(int addition) //Used in the above function
    {
        for (int i = addition; i <= addition + 2; i++)
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
        fishingScript.somethingOnHook = true;
        hookedFish = fishOnHook;

        //Start spawning waves?
        switchControlState(1);

        
    }

}

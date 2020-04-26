using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ourGameManager : MonoBehaviour
{
    //This script should be responsible for correctly transitioning between sea screen and fishing screen.
    //Can also use this to update interface images. 

    //Things we need access to.
    Player playerScript;
    playerFishing fishingScript;
    FishAI fishAIScript;
    public GameObject playerBoat;
    public GameObject playerFishing;

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
            fishySpecies[i].GetComponent<FishAI>().manager = this.gameObject.GetComponent<ourGameManager>();
            fishySpecies[i].SetActive(false);
            fishySpecies[i].transform.parent = null;
        }

        playerScript = playerBoat.GetComponent<Player>();
        fishingScript = playerFishing.GetComponent<playerFishing>();


        //UI setup
        controls.sprite = controlImages[0];
        
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
        if(currentState == 0)
        {
            playerScript.controlstate = 1;
            fishingScript.controlstate = 0;

            currentScreen = 1;

            theCamera.transform.position = new Vector3(fishingCloseup.position.x, fishingCloseup.position.y, -10);

            spawnFishies();

            controls.sprite = controlImages[1];
        }
        else
        {
            playerScript.controlstate = 0;
            fishingScript.controlstate = 1;

            currentScreen = 0;
            controls.sprite = controlImages[0];
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


    public void fishOnHook(GameObject fishOnHook)
    {
        fishingScript.somethingOnHook = true;
        hookedFish = fishOnHook;

        //Start spawning waves?
    }

}

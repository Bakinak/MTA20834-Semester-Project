using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ourGameManager : MonoBehaviour
{
    //This script should be responsible for correctly transitioning between sea screen and fishing screen.
    //Can also use this to update interface images. 

    //Things we need access to.
    Player playerScript;
    playerFishing fishingScript;
    public GameObject playerBoat;
    public GameObject playerFishing;

    int currentScreen;

    int currentLocation = 1;

    //Camera
    public Camera theCamera;
    public Transform fishingCloseup;

    //Spawning Fishies
    public Transform fishSpawnLocation;
    public GameObject[] fishySpecies;
    public Transform[] fishSpawnLocations;


    // Start is called before the first frame update
    void Start()
    {
        playerScript = playerBoat.GetComponent<Player>();
        fishingScript = playerFishing.GetComponent<playerFishing>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentScreen == 0)
        {
            theCamera.transform.position = new Vector3(playerBoat.transform.position.x, playerBoat.transform.position.y, -10);
        }
    }


    //Public functions accessed by others

    public void switchControlState(int currentState)
    {
        if(currentState == 0)
        {
            playerScript.controlstate = 1;
            fishingScript.controlstate = 0;

            currentScreen = 1;

            theCamera.transform.position = new Vector3(fishingCloseup.position.x, fishingCloseup.position.y, -10);

            spawnFishies();
        }
        else
        {
            playerScript.controlstate = 0;
            fishingScript.controlstate = 1;

            currentScreen = 0;
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

    void spawn(int addition)
    {
        for(int i = addition; i <= addition+2; i++)
        {
            Instantiate(fishySpecies[i], fishSpawnLocations[i-addition]);
        }
    }


}

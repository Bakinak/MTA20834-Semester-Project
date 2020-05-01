using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerFishing : MonoBehaviour
{
    //Access to our game manager
    ourGameManager manager;

    //Creating fishing line
    public Transform lineStart;
    public Transform lineEnd;
    public LineRenderer line;

    public bool somethingOnHook;
    public bool inputSequenceOver;

    public bool lowerKeyLetGo;
    bool hookLowered;

    public bool controlstate;

    Vector3 startPosition;

    int currentHookPosition = 0;
    float moveSpeed = 7;
    float startMoveSpeed;
    // Start is called before the first frame update
    void Start()
    {
        startMoveSpeed = moveSpeed;
        inputSequenceOver = false;
        manager = GameObject.FindGameObjectWithTag("manager").GetComponent<ourGameManager>();
        startPosition = transform.position;
        line.SetPosition(0, lineStart.position);
        lineStart.transform.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (controlstate == true) //Only allow movement of hook while in this control state.
        {
            movement();
            if(Input.GetAxisRaw("Vertical") == 0)
            {
                lowerKeyLetGo = true;
            }
        }
        else if (inputSequenceOver) //When the input sequence is over, do something based on whether or not the player succesfully caught the fish. That means this should only run AFTER keeping boat steady.
        {
            moveSpeed = 3;
            hookLowered = false;
            transform.position = Vector3.MoveTowards(transform.position, startPosition, moveSpeed * Time.deltaTime); //Move hook back up out of water. Once there, do something depending on wheteh fish was catched or not.
            if (transform.position == startPosition)
            {
                currentHookPosition = 0;
                if (somethingOnHook == true)
                {
                    somethingOnHook = false;
                    manager.fishCaught();
                }
                moveSpeed = startMoveSpeed;
                inputSequenceOver = false;
                manager.switchControlState(2);
                
            }
        }
        
        fishingLine();

    }

    //Move Fishing Hook
    void movement()
    {
        if (somethingOnHook == false && hookLowered == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, manager.fishSpawnLocations[currentHookPosition].position.y, 0), moveSpeed * Time.deltaTime);

            if (transform.position.y == manager.fishSpawnLocations[currentHookPosition].position.y)
            {
                if (Input.GetAxisRaw("Vertical") < 0f && currentHookPosition < 2)
                {
                    currentHookPosition += 1;
                }

                if (Input.GetAxisRaw("Vertical") > 0f && currentHookPosition > 0)
                {
                    currentHookPosition -= 1;
                }
            }
        }
        else if(somethingOnHook == false)
        {
            transform.position = Vector3.MoveTowards(transform.position, startPosition, moveSpeed * Time.deltaTime);

            if (Input.GetAxisRaw("Vertical") < 0f && lowerKeyLetGo == true)
            {
                hookLowered = true;
                manager.startFishMovement();
            }
        }
    }

    void fishingLine()
    {
        line.SetPosition(1, lineEnd.position);
    }


}

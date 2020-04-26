using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerFishing : MonoBehaviour
{
    //Access to our game manager
    public ourGameManager manager;

    //Creating fishing line
    public Transform lineStart;
    public Transform lineEnd;
    public LineRenderer line;

    public bool somethingOnHook;

    bool hookLowered;

    public int controlstate = 1;

    Vector3 startPosition;

    int currentHookPosition = 0;
    float moveSpeed = 6;
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        line.SetPosition(0, lineStart.position);
        lineStart.transform.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (controlstate == 0)
        {
            test();
            movement();
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

            if (Input.GetAxisRaw("Vertical") < 0f)
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

    //For testing purposes
    void test()
    {
        if (Input.GetKeyDown("space"))
        {
            manager.switchControlState(1);
        }

        if (Input.GetKeyDown("return"))
        {
            
        }

    }
}

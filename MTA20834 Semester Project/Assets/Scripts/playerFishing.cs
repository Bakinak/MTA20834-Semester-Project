using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerFishing : MonoBehaviour
{
    //Access to our game manager
    public ourGameManager manager;

    public int controlstate = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (controlstate == 0)
        {
            test();
        }
    }

    void test()
    {
        if (Input.GetKeyDown("space"))
        {
            manager.switchControlState(1);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerSteadyBoat : KeySquenceInput
{

    //I assume this script should inherit from Bastians scripts.

    //Get access to manager
    ourGameManager manager;

    public bool controlstate;
    

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("manager").GetComponent<ourGameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(controlstate == true)
        {
            steadying();
        }
    }


    void steadying()
    {

    }

    public void test()
    {
        Debug.Log("something");
    }
}

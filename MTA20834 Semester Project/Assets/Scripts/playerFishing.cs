using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerFishing : MonoBehaviour
{
    //Access to our game manager
    public ourGameManager manager;
    public MenuObjective MenOb;

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

    public void catchFish()
    {
        if (MenOb.isActive)
        {
            MenOb.goal.FishCollected();
            if (MenOb.goal.IsReached())
            {
                MenOb.Complete();
            }
        }
    }
}

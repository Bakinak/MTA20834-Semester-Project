﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishAI : MonoBehaviour
{
    public bool move;
    public int fishtype;


    ourGameManager manager;


    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("manager").GetComponent<ourGameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(move == true)
        {
            transform.position += new Vector3(1, 0, 0) * Time.deltaTime;
        } 

    }



    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "fishBoundary") //If collision with boundary, stop moving and set fish to being inactive, where it will wait until needed to be used again. When set to active again, position will reset to where it needs to be.
        {
            move = false;
            gameObject.SetActive(false);
        }

        if (other.tag == "hook") //If collision with hook, stop forward movement, attach fish to hook, and tell GameManager what fish has been hooked. 
        {
            move = false;
            transform.parent = other.transform;
            manager.fishOnHook(gameObject);
        }


    }


}
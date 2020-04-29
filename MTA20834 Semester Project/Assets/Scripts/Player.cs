﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Access to our game manager
    public ourGameManager manager;
    public QuestSystem qst;

    //Fish
    GameObject fishToSpawn;


    public GameObject noEnter;
    private float timeWhenDisappear;
    public float timeToDisappear = 2f;


    //Movement
    public float moveSpeed;
    public float preMoveDistance;
    public float distanceBetweenTiles = 2;
    Transform movePoint;
    public LayerMask block;


    
    public int controlstate;
    
    // Start is called before the first frame update
    void Start()
    {
        movePoint = gameObject.transform.GetChild(0);
        movePoint.parent = null;

        noEnter.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);

        if (controlstate == 0)
        {
            gridMovement();
        }

        if(noEnter.activeSelf && (Time.time >= timeWhenDisappear))
        {
            noEnter.SetActive(false);
        }
    }

    void gridMovement()
    {
        
        if (Vector3.Distance(transform.position, movePoint.position) <= preMoveDistance)
        {
            if(Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
            {
                if(!Physics2D.OverlapCircle(movePoint.position + new Vector3(Input.GetAxisRaw("Horizontal")*(distanceBetweenTiles), 0f, 0f), .2f, block))
                {
                    movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal")*distanceBetweenTiles, 0f, 0f);
                } 
            } else if(Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
            {
                if(!Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, Input.GetAxisRaw("Vertical")*(distanceBetweenTiles), 0f), .2f, block))
                {
                    movePoint.position += new Vector3(0f, Input.GetAxisRaw("Vertical")*distanceBetweenTiles, 0f);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "fishSchool")
        {
            //Remove bubble spot so we don't hit it again
            collision.gameObject.SetActive(false);
            //Select and spawn fish
            //fishToSpawn = collision.gameObject.GetComponent<fishSpot>().fish;

            //Remove control of ship and move camera position
            manager.switchControlState(0);
        }

        if (collision.gameObject.tag == "entranceTile")
        {
            //Debug.Log("hit");
            //deactivate the arrowindicator when entering level 2
            qst.arrowIndicatorLevel1.SetActive(false);
            qst.arrowIndicatorLevel2.SetActive(false);
        }

        if (collision.gameObject.tag == "invisibleEntrance" && qst.updateEel != 2 && qst.updateCarb !=2)
        {
            noEnter.SetActive(true);
            timeWhenDisappear = Time.time + timeToDisappear;
        } else if (collision.gameObject.tag == "invisibleEntrance" && qst.updateCarbQuest2 != 2 && qst.updateCod != 2)
        {
            noEnter.SetActive(true);
            timeWhenDisappear = Time.time + timeToDisappear;
        }
    }
}

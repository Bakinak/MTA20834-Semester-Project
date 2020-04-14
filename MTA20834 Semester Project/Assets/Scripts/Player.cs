using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    //Camera
    public Camera theCamera;
    public Transform fishingCloseup;
    private Vector3 currentLocation;

    //Movement
    public float moveSpeed;
    public float preMoveDistance;
    public float distanceBetweenTiles = 2;
    public Transform movePoint;
    public LayerMask block;



    int controlstate;

    // Start is called before the first frame update
    void Start()
    {
        movePoint.parent = null;
        currentLocation = theCamera.transform.position;
        Debug.Log(currentLocation.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);

        if (controlstate == 0)
        {
            gridMovement();
            theCamera.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
        }

        if(controlstate == 1)
        {
            if (Input.GetKey("space"))
            {
                theCamera.transform.position = currentLocation;
                
                //Debug.Log(currentLocation.ToString());
                controlstate = 0;
            }
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
            controlstate = 1;
            collision.gameObject.SetActive(false);
            currentLocation = theCamera.transform.position;
            theCamera.transform.position = fishingCloseup.position;
        }
    }


}

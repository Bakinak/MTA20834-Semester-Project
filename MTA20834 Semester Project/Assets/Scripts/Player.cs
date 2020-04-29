using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Access to our game manager
    ourGameManager manager;

    //Fish
    GameObject fishToSpawn;



    //Movement
    public float moveSpeed;
    public float preMoveDistance;
    public float distanceBetweenTiles = 2;
    Transform movePoint;
    public LayerMask block;


    
    public bool controlstate;
    
    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("manager").GetComponent<ourGameManager>();
        movePoint = gameObject.transform.GetChild(0);
        movePoint.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);

        if (controlstate == true)
        {
            gridMovement();
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
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Access to our game manager
    public ourGameManager manager;

    //Fish Skal måske ikke bruges mere
    GameObject fishToSpawn;



    //Movement
    //Public kan ændres i inspector i Unity og tilgåes i andre scripts
    public float moveSpeed;
    public float preMoveDistance;
    public float distanceBetweenTiles = 2; //sørger for vi altid rammer midt af vandet
    Transform movePoint;
    public LayerMask block;


    //Hvornår man kan styre båden, bliver deaktiveret ved BCI spil
    public int controlstate;

    // Start is called before the first frame update
    void Start()
    {
        movePoint = gameObject.transform.GetChild(0);
        movePoint.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);

        if (controlstate == 0)
        {
            gridMovement();
        }

        
    }


    void gridMovement()
    {
        
        if (Vector3.Distance(transform.position, movePoint.position) <= preMoveDistance) //Hvis 0 kan man bevæge den, så det passer til tiles
        {
            if(Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
            {
                if(!Physics2D.OverlapCircle(movePoint.position + new Vector3(Input.GetAxisRaw("Horizontal")*(distanceBetweenTiles), 0f, 0f), .2f, block)) //hvis der hvor vi er nu +2 tiles frem ikke er noget der blokere (bliver blokeret ud fra laget), så videre
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

    private void OnTriggerEnter2D(Collider2D collision) //tiggerenter er bobler
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

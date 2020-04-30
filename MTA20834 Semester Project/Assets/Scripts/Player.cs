using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Access to our game manager
    public ourGameManager manager;
    public QuestSystem qst;

    //Fish Skal måske ikke bruges mere
    GameObject fishToSpawn;

    public Camera minimap;
    public GameObject noEnter, noEnterLevel2, level2, level3;
    private float timeWhenDisappear;
    public float timeToDisappear = 2f;


    //Movement
    //Public kan ændres i inspector i Unity og tilgåes i andre scripts
    public float moveSpeed = 4;
    public float preMoveDistance;
    public float distanceBetweenTiles = 2; //sørger for vi altid rammer midt af vandet
    Transform movePoint;
    public LayerMask block;
    public Animator animator;



    public bool controlstate;

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("manager").GetComponent<ourGameManager>();
        movePoint = gameObject.transform.GetChild(0);
        movePoint.parent = null;

        noEnter.SetActive(false);
        noEnterLevel2.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);

        if (controlstate == true)
        {
            gridMovement();
        }

        if (noEnter.activeSelf && (Time.time >= timeWhenDisappear))
        {
            noEnter.SetActive(false);
        }
        else if (noEnterLevel2.activeSelf && (Time.time >= timeWhenDisappear))
        {
            noEnterLevel2.SetActive(false);
        }
    }

    void gridMovement()
    {
        if (Vector3.Distance(transform.position, movePoint.position) <= preMoveDistance) //Hvis 0 kan man bevæge den, så det passer til tiles
        {
            if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
            {
                if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(Input.GetAxisRaw("Horizontal") * (distanceBetweenTiles), 0f, 0f), .2f, block)) //hvis der hvor vi er nu +2 tiles frem ikke er noget der blokere (bliver blokeret ud fra laget), så videre
                {
                    if (Input.GetAxisRaw("Horizontal") == 1)
                    {
                        animator.SetInteger("Direction", 0);
                        GetComponent<SpriteRenderer>().flipX = true;
                    }
                    else if (Input.GetAxisRaw("Horizontal") == -1)
                    {
                        animator.SetInteger("Direction", 0);
                        GetComponent<SpriteRenderer>().flipX = false;
                    }
                    movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal") * distanceBetweenTiles, 0f, 0f);

                }
            }
            else if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
            {
                if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, Input.GetAxisRaw("Vertical") * (distanceBetweenTiles), 0f), .2f, block))
                {
                    if (Input.GetAxisRaw("Vertical") == 1)
                    {
                        animator.SetInteger("Direction", 1);
                    }
                    else if (Input.GetAxisRaw("Vertical") == -1)
                    {
                        animator.SetInteger("Direction", -1);
                    }
                    movePoint.position += new Vector3(0f, Input.GetAxisRaw("Vertical") * distanceBetweenTiles, 0f);
                }
            }

            //float x = Input.GetAxisRaw("Horizontal");
            //float y = Input.GetAxisRaw("Vertical");

            //if (y < 0)
            //{
            //    if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(y * (distanceBetweenTiles), 0f, 0f), .2f, block)) //hvis der hvor vi er nu +2 tiles frem ikke er noget der blokere (bliver blokeret ud fra laget), så videre
            //    {
            //        animator.SetInteger("Direction", (int)y);
            //        movePoint.position += new Vector3(0f, y * distanceBetweenTiles, 0f);
            //    }
            //}
            //else if (y > 0)
            //{
            //    if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, y * (distanceBetweenTiles), 0f), .2f, block))
            //    {
            //        animator.SetInteger("Direction", (int)y);
            //        movePoint.position += new Vector3(0f, y * distanceBetweenTiles, 0f);
            //    }
            //}
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

        if (collision.gameObject.tag == "entranceTile")
        {
            //Debug.Log("hit");
            //deactivate the arrowindicator when entering level 2 and 3
            qst.arrowIndicatorLevel1.SetActive(false);
            qst.arrowIndicatorLevel2.SetActive(false);
        }

        //if the quest in first level is not complete, show the text if player tries to move to next level
        if (collision.gameObject.tag == "invisibleEntrance" && qst.updateEel != 2 && qst.updateCarb != 2)
        {
            Debug.Log("You hit me");
            timeWhenDisappear = Time.time + timeToDisappear;
            noEnter.SetActive(true);
        }
        //if the quest in second level is not complete, show the text if player tries to move to next level
        else if (collision.gameObject.tag == "invisibleEntrance2" && qst.updateCarbQuest2 != 2 && qst.updateCod != 2)
        {
            timeWhenDisappear = Time.time + timeToDisappear;
            noEnterLevel2.SetActive(true);
        }

        //if player has completed the quest, and moves on to next area, set minimap camera to the following level
        if (collision.gameObject.tag == "invisibleEntrance" && qst.updateEel == 2 && qst.updateCarb == 2)
        {
            minimap.transform.position = new Vector3(level2.transform.position.x, level2.transform.position.y, -10);
        }
        else if (collision.gameObject.tag == "invisibleEntrance2" && qst.updateCarbQuest2 == 2 && qst.updateCod == 2)
        {
            minimap.transform.position = new Vector3(level3.transform.position.x, level3.transform.position.y, -10);
        }
    }
}

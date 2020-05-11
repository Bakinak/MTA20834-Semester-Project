using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Access to our game manager
    public ourGameManager manager;
    public QuestSystem qst;
    public playerFishing fishPlayer;

    //Fish Skal måske ikke bruges mere
    GameObject fishToSpawn;

    public Camera minimap;
    public GameObject noEnter, noEnterLevel2, level1, level2, level3;
    public AudioSource rain;
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

    GameObject previousFishSpot;

    public bool controlstate;

    //Reset position in case we get stuck during experiment
    KeyCode[] sequence = new KeyCode[]
{
        KeyCode.J,
        KeyCode.U,
        KeyCode.K,
};
    int sequenceIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        
        manager = GameObject.FindGameObjectWithTag("manager").GetComponent<ourGameManager>();
        movePoint = gameObject.transform.GetChild(0);
        movePoint.parent = null;

        rain.enabled = false;
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

            if (Input.GetKeyDown(sequence[sequenceIndex]))
            {
                if (++sequenceIndex == sequence.Length)
                {
                    sequenceIndex = 0;
                    transform.position = new Vector3(0, 0, 0);
                    movePoint.position = new Vector3(0, 0, 0); 
                    // sequence typed
                }
            }
            else if (Input.anyKeyDown) sequenceIndex = 0;

        }
    }

    private void OnTriggerEnter2D(Collider2D collision) //tiggerenter er bobler
    {
        if (collision.gameObject.tag == "fishSchool")
            
        {
            //set the reel sound to false again, when entering a fish school
            fishPlayer.soundplayed = false;
            //Remove bubble spot so we don't hit it again. Also, bring back the previous spot we hit, so the player can never run out of fish.
            if(previousFishSpot != null)
            {
                previousFishSpot.SetActive(true);

            }
            SoundManager.PlaySound(SoundManager.Sound.splash);
            collision.gameObject.SetActive(false);
            previousFishSpot = collision.gameObject;
            
            //Select and spawn fish
            //fishToSpawn = collision.gameObject.GetComponent<fishSpot>().fish;



            //Remove control of ship and move camera position
            manager.switchControlState(0);

            
        }

        if (collision.gameObject.tag == "entranceTile" || collision.gameObject.tag == "entranceTileLevel3")
        {
            //deactivate the arrowindicator when entering level 2 and 3
            qst.arrowIndicatorLevel1.SetActive(false);
            qst.arrowIndicatorLevel2.SetActive(false);
        }

        //if the quest in first level is not complete, show the text if player tries to move to next level
        if (collision.gameObject.tag == "entranceTile" && qst.updateCarb != 2 && qst.updateCod != 2)
        {
            Debug.Log("You hit me");
            timeWhenDisappear = Time.time + timeToDisappear;
            noEnter.SetActive(true);
        }
        //if the quest in second level is not complete, show the text if player tries to move to next level
        else if (collision.gameObject.tag == "entranceTileLevel3" && qst.updateEel != 2 && qst.updateRainbow != 2)
        {
            timeWhenDisappear = Time.time + timeToDisappear;
            noEnterLevel2.SetActive(true);
        }

        //if player has completed the quest, and moves on to next area, set minimap camera to the following level
        if (collision.gameObject.tag == "invisibleEntrance" && qst.updateCarb == 2 && qst.updateCod == 2)
        {
            minimap.transform.position = new Vector3(level2.transform.position.x, level2.transform.position.y, -20);
            manager.currentLocation = 2;
            rain.enabled = true;
        }
        else if (collision.gameObject.tag == "entranceTile" && manager.currentLocation == 2)
        {
            minimap.transform.position = new Vector3(level1.transform.position.x, level1.transform.position.y, -20);
            manager.currentLocation = 1;
            rain.enabled = false;
        }
        else if (collision.gameObject.tag == "invisibleEntrance2" && qst.updateEel == 2 && qst.updateRainbow == 2)
        {
            minimap.transform.position = new Vector3(level3.transform.position.x, level3.transform.position.y, -20);
            manager.currentLocation = 3;
            rain.enabled = false;
        }
        else if (collision.gameObject.tag == "entranceTileLevel3" && manager.currentLocation == 3)
        {
            minimap.transform.position = new Vector3(level2.transform.position.x, level2.transform.position.y, -20);
            manager.currentLocation = 2;
            rain.enabled = true;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waves : MonoBehaviour
{
    Vector3 startPosition;
    public float moveSpeed;
    bool boatContact;
    bool waveBroken;
    Transform distanceToBoat;

    // Start is called before the first frame update
    void Start()
    {
        distanceToBoat = GameObject.FindGameObjectWithTag("Player").transform;
        startPosition = transform.position;
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(moveSpeed, 0, 0) * Time.deltaTime;

        if (boatContact)
        {
            if(waveBroken == false && Vector2.Distance(transform.position, distanceToBoat.transform.position) < 2)
            {
                //Do something in animator controller so it breaks against the ship
                Debug.Log("The wave crashed into the boat!, Splooosh!!!");
                waveBroken = true;
                
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "fishBoundary") 
        {
            transform.position = startPosition;
            boatContact = false;
            Debug.Log("I hit the boundary");
            gameObject.SetActive(false);
        }


        if(other.tag == "Player")
        {
            boatContact = true;
        }
    }
}

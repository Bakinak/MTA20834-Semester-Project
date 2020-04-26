using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishAI : MonoBehaviour
{
    public bool move;
    public bool hooked;
    public int fishtype;
    public float moveSpeed;
    //public float timeToMove;
    Vector3 hookPosition;

    // Start is called before the first frame update
    void Start()
    {
        //startPosition = transform.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(move == true)
        {
            transform.position += new Vector3(moveSpeed, 0, 0) * Time.deltaTime;
        } else if (hooked == true)
        {
            transform.position = hookPosition;
        }

    }



    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "fishBoundary")
        {
            move = false;
            hooked = false;
            gameObject.SetActive(false);
        }

        if (other.tag == "hook")
        {
            move = false;
            hookPosition = other.transform.position;
            hooked = true;
        }


    }


}

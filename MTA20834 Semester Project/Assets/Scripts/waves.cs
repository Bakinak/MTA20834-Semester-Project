using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waves : MonoBehaviour
{
    Vector3 startPosition;
    public float moveSpeed;
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
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "fishBoundary")
        {
            transform.position = startPosition;
            gameObject.SetActive(false);
        }
    }
}

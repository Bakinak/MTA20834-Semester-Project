using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    
    public float moveSpeed;
    public float preMoveDistance;
    public float distanceBetweenTiles = 2;
    public Transform movePoint;
    public LayerMask collision;


    // Start is called before the first frame update
    void Start()
    {
        movePoint.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        gridMovement();
    }


    void gridMovement()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, movePoint.position) <= preMoveDistance)
        {
            if(Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
            {
                if(!Physics2D.OverlapCircle(movePoint.position + new Vector3(Input.GetAxisRaw("Horizontal")*(distanceBetweenTiles/2), 0f, 0f), .2f, collision))
                {
                    movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal")*distanceBetweenTiles, 0f, 0f);
                } 
            } else if(Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
            {
                if(!Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, Input.GetAxisRaw("Vertical")*(distanceBetweenTiles / 2), 0f), .2f, collision))
                {
                    movePoint.position += new Vector3(0f, Input.GetAxisRaw("Vertical")*distanceBetweenTiles, 0f);
                }
            }
        }
    }
}

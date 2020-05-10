using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotationTest : MonoBehaviour
{


    public Transform to;
    float speed = 0.1f;

    float maxTilt = 20;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, to.rotation, speed);

        if(transform.rotation == to.rotation)
        {
            maxTilt *= -1;
            to.eulerAngles = new Vector3 (0,0, maxTilt);
        }




        /*rotation = transform.eulerAngles.z;

        transform.eulerAngles += new Vector3(0, 0, tiltSpeed)*Time.deltaTime;

        if (transform.eulerAngles.z < 340 && transform.eulerAngles.z >= 200 && direction == true)
        {
            tiltSpeed *= -1;
            direction = false;
        } else if (transform.eulerAngles.z > 20 && transform.eulerAngles.z <= 100 && direction == false)
        {
            tiltSpeed *= -1;
            direction = true;
        }*/

    }
}

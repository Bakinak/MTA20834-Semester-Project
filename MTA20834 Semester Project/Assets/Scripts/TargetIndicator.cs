using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetIndicator : MonoBehaviour
{
    public Transform Target;
    public float hideDistance;


    // Update is called once per frame
    void Update()
    {
        var dir = Target.position - transform.position;

        if(dir.magnitude < hideDistance)
        {
            SetChildActive(false);
        }
        else
        {
            SetChildActive(true);

            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    void SetChildActive(bool val)
    {
        foreach(Transform child in transform)
        {
            child.gameObject.SetActive(val);
        }
    }
}

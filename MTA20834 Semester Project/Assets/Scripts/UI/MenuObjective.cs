using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MenuObjective
{
    public bool isActive;

    public ItemUpgradeGoal goal;

    public void Complete()
    {
        isActive = false;
        Debug.Log("yo i am completed");
    }
}

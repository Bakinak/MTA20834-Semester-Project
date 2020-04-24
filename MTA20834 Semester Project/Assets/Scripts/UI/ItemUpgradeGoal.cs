using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemUpgradeGoal
{
    public GoalType goalType;

    public int requiredAmount;
    public int currentAmount;

    public bool IsReached()
    {
        return (currentAmount >= requiredAmount);
    }

    public void FishCollected()
    {
        if(goalType == GoalType.Collect)
            currentAmount++;
    }
    
}

public enum GoalType
{
    Collect
}

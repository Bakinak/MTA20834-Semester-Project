using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectFish : MonoBehaviour
{
    

    private void OnTriggerEnter2D(Collider2D other)
    {
        QuestSystem.currentAmount += 1;

    }
}

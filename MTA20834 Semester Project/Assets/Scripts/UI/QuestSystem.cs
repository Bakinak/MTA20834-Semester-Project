using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestSystem : MonoBehaviour
{
    public Image fishPole, fishPoleComplete, Umbrella, umbrellaComplete, lockSpriteQuest2, lockSpriteQuest3;

    public GameObject eelText, carbText, carbTextQuest2, codTextQuest2, codTextQuest3, rainbowTextQuest3;

    public int updateEel, updateCarb, updateCarbQuest2, updateCod, updateCodQuest3, updateRainbow;

    private void Start()
    {
        codTextQuest2.SetActive(false);
        carbTextQuest2.SetActive(false);
        codTextQuest3.SetActive(false);
        rainbowTextQuest3.SetActive(false);

        fishPoleComplete.enabled = false;
        Umbrella.enabled = false;
        umbrellaComplete.enabled = false;

    }


    void Update()
    {
       //updating the text values on the UI
       eelText.GetComponent<Text>().text = updateEel + "/2 eel";
       carbText.GetComponent<Text>().text = updateCarb + "/2 carb";
       carbTextQuest2.GetComponent<Text>().text = updateCarbQuest2 + "/2 carb";
       codTextQuest2.GetComponent<Text>().text = updateCod + "/2 cod";
       codTextQuest3.GetComponent<Text>().text = updateCodQuest3 + "/2 cod";
       rainbowTextQuest3.GetComponent<Text>().text = updateRainbow + "/2 rainbow fish";

       if (Input.GetKeyDown(KeyCode.Space))
        {
            updateEel += 1;
            updateCarb += 1;

            quest1Check();
        }


       if(codTextQuest2.activeInHierarchy == true)
        {        
            if(Input.GetKeyDown(KeyCode.Return))
            {
                updateCarbQuest2 += 1;
                updateCod += 1;

                quest2Check();
            }
        }

        if (rainbowTextQuest3.activeInHierarchy == true)
        {
            if(Input.GetKeyDown(KeyCode.KeypadEnter))
                
                updateRainbow += 1;
                updateCodQuest3 += 1;
                
        }
    }

    public void quest1Check()
    {
        //add some logic to the if statement (if Umbrella == false then u can do this, but if active u cannot)
        if (updateEel >= 2 && updateCarb >= 2)
        {
            if(fishPoleComplete.enabled == true)
            {
                return;
            } 
            else { 

            eelText.SetActive(false); carbText.SetActive(false);
            fishPole.enabled = false;

            
            Umbrella.enabled = true; fishPoleComplete.enabled = true;
            codTextQuest2.SetActive(true); carbTextQuest2.SetActive(true);
            }
        }
    }

    public void quest2Check()
    {
        if(updateCarb >= 2 && updateCod >= 2)
        {
            if(umbrellaComplete.enabled == true)
            {
                return;
            } 
            else { 

            codTextQuest2.SetActive(false); carbTextQuest2.SetActive(false);
            Umbrella.enabled = false;

            umbrellaComplete.enabled = true;
            codTextQuest3.SetActive(true); rainbowTextQuest3.SetActive(true);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestSystem : MonoBehaviour
{
    public Image fishPole, fishPoleComplete, Umbrella, umbrellaComplete, lockSpriteQuest2, lockSpriteQuest3, isryder, isryderComplete;

    public GameObject codText, carbText, eelText, rainbowfishText, clownfishText, catfishText, arrowIndicatorLevel1, arrowIndicatorLevel2, 
                      entranceLevel2, entranceLevel3;

    public int updateEel, updateCarb, updateCod, updateRainbow, updateCatfish, updateClownfish;

    private void Start()
    {
        eelText.SetActive(false);
        rainbowfishText.SetActive(false);
        catfishText.SetActive(false);
        clownfishText.SetActive(false);

        arrowIndicatorLevel1.SetActive(false);
        arrowIndicatorLevel2.SetActive(false);

        fishPoleComplete.enabled = false;
        Umbrella.enabled = false;
        umbrellaComplete.enabled = false;
        isryder.enabled = false;
        isryderComplete.enabled = false;
        
    }


    void Update()
    {
        //updating the text values on the UI
        carbText.GetComponent<Text>().text = updateCarb + "/2 carb";
        codText.GetComponent<Text>().text = updateCod + "/2 cod";
        eelText.GetComponent<Text>().text = updateEel + "/2 eel";
        rainbowfishText.GetComponent<Text>().text = updateRainbow + "/2 rainbowfish";
        catfishText.GetComponent<Text>().text = updateCatfish + "/2 catfish";
        clownfishText.GetComponent<Text>().text = updateClownfish + "/2 clownfish";
    }

    public void updateFishUI(int fish)
    {
        if(fish == 0)
        {
            updateCod++;
        } else if (fish == 1)
        {
            updateCarb++;
        } else if(fish == 2)
        {
            updateEel++;
        } else if (fish == 3)
        {
            updateRainbow++;
        } else if(fish == 4)
        {
            updateCatfish++;
        } else if(fish == 5)
        {
            updateClownfish++;
        }

        quest1Check();
        quest2Check();
        quest3Check();
    }

    public void quest1Check()
    {
        //add some logic to the if statement (if Umbrella == false then u can do this, but if active u cannot)
        if (updateCarb >= 2 && updateCod >= 2)
        {
            if (fishPoleComplete.enabled == true)
            {
                return;
            }
            else
            {
                carbText.SetActive(false); codText.SetActive(false);
                fishPole.enabled = false;
                SoundManager.PlaySound(SoundManager.Sound.questComplete);
                entranceLevel2.SetActive(false);
                arrowIndicatorLevel1.SetActive(true);
                Umbrella.enabled = true; fishPoleComplete.enabled = true;
                eelText.SetActive(true); rainbowfishText.SetActive(true);
            }
        }
    }


    public void quest2Check()
    {
        if (updateEel >= 2 && updateRainbow >= 2)
        {
            if (umbrellaComplete.enabled == true)
            {
                return;
            }
            else
            {
                eelText.SetActive(false); rainbowfishText.SetActive(false);
                Umbrella.enabled = false;

                //entranceLevel3.SetActive(false);
                arrowIndicatorLevel2.SetActive(true);
                umbrellaComplete.enabled = true;
                isryder.enabled = true;
                clownfishText.SetActive(true); catfishText.SetActive(true);
            }
        }
    }

    public void quest3Check()
    {
        if (updateClownfish >= 2 && updateCatfish >= 2)
        {
            clownfishText.SetActive(false); catfishText.SetActive(false);
            isryderComplete.enabled = true;
        }
    }
}

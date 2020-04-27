using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestSystem : MonoBehaviour
{
    public GameObject eelText;
    public GameObject CarbText;
    public GameObject FishXText;
    public GameObject FishYText;
    public GameObject FishZText;

    public static int updateEel;
    public static int updateCarb;

    void Update()
    {
        eelText.GetComponent<Text>().text = updateEel + "/2 eel";

       CarbText.GetComponent<Text>().text = updateCarb + "/2 carb";

        // FishXText.GetComponent<Text>().text = currentAmount + "/2 fishX";

       // FishYText.GetComponent<Text>().text = currentAmount + "/2 fishY";

       // FishZText.GetComponent<Text>().text = currentAmount + "/2 fishZ";

    }
}

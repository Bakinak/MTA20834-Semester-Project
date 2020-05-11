using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishAI : MonoBehaviour
{
    public bool move;
    public bool escaped;
    public int fishtype;

    public QuestSystem qst;
    ourGameManager manager;


    // Start is called before the first frame update
    void Start()
    {
        escaped = false;
        manager = GameObject.FindGameObjectWithTag("manager").GetComponent<ourGameManager>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(move == true)
        {
            transform.position += new Vector3(1.5f, 0, 0) * Time.deltaTime;
        } 

    }

    public void checkOutline()
    {
        switch (fishtype)
        {
            case 0:
                if(qst.updateCod < 2)
                {
                    gameObject.transform.GetChild(0).gameObject.SetActive(true);
                }
                else
                {
                    gameObject.transform.GetChild(0).gameObject.SetActive(false);
                }
                break;

            case 1:
                if(qst.updateCarb < 2)
                {
                    gameObject.transform.GetChild(0).gameObject.SetActive(true);
                }
                else
                {
                    gameObject.transform.GetChild(0).gameObject.SetActive(false);
                }
                break;

            case 2:
                if (qst.updateEel < 2)
                {
                    gameObject.transform.GetChild(0).gameObject.SetActive(true);
                }
                else
                {
                    gameObject.transform.GetChild(0).gameObject.SetActive(false);
                }
                break;

            case 3:
                if (qst.updateRainbow < 2)
                {
                    gameObject.transform.GetChild(0).gameObject.SetActive(true);
                }
                else
                {
                    gameObject.transform.GetChild(0).gameObject.SetActive(false);
                }
                break;

            case 4:
                if (qst.updateCatfish < 2)
                {
                    gameObject.transform.GetChild(0).gameObject.SetActive(true);
                }
                else
                {
                    gameObject.transform.GetChild(0).gameObject.SetActive(false);
                }
                break;

            case 5:
                if (qst.updateClownfish < 2)
                {
                    gameObject.transform.GetChild(0).gameObject.SetActive(true);
                }
                else
                {
                    gameObject.transform.GetChild(0).gameObject.SetActive(false);
                }
                break;

            default:

                break;
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "fishBoundary") //If collision with boundary, stop moving and set fish to being inactive, where it will wait until needed to be used again. When set to active again, position will reset to where it needs to be.
        {
            move = false;
            escaped = false;
            gameObject.SetActive(false);
        }

        if (other.tag == "hook" && escaped == false) //If collision with hook, stop forward movement, attach fish to hook, and tell GameManager what fish has been hooked. 
        {
            SoundManager.PlaySound(SoundManager.Sound.fishBiteHook);
            move = false;
            transform.parent = other.transform;
            manager.fishOnHook(gameObject);
        }


    }


}

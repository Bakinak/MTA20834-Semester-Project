
using System.IO;
using UnityEngine;

public class Logger : MonoBehaviour
{
    private string filePath;
    string sequenceString, recognizedString;

    // Start is called before the first frame update
    void Start()
    {
        filePath = Application.persistentDataPath + "/log.txt";

        StreamWriter writer = new StreamWriter(filePath, true);
        writer.WriteLine("");
        writer.Close();
    }

    public void writeCondition(bool condition)
    {
        if (condition)
        {
            StreamWriter writer = new StreamWriter(filePath, true);
            writer.WriteLine("Continuos Condition");
            writer.Close();
        }
        else
        {
            StreamWriter writer = new StreamWriter(filePath, true);
            writer.WriteLine("Discrete Condition");
            writer.Close();
        }
    }

    public void NewLog(int attemptNumber, int fishCaught, bool sequence, bool recognized)
    {
        Debug.Log("LoggedSomething?");
        if (sequence)
        {
            sequenceString = "CoSeq"; //Correct sequence
        }
        else
        {
            sequenceString = "FaSeq"; //Failed sequence
        }

        if (recognized)
        {
            recognizedString = "CoRecog";
        }
        else
        {
            recognizedString = "FaRecog";
        }

        string logString = attemptNumber + ", " + fishCaught + ", " + sequenceString + ", " + recognizedString + ", " + Time.realtimeSinceStartup;

        //Write log string text to the log.txt file
        StreamWriter writer = new StreamWriter(filePath, true);
        writer.WriteLine(logString);
        writer.Close();
    }

}
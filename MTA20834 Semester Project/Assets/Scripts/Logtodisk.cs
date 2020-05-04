using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEditor;
using System;
using System.Linq;
using UnityEngine.Events;

public class Logtodisk : MonoBehaviour
{
    [SerializeField]
    private string filepath = Application.persistentDataPath + "/log.txt";

    public StreamWriter writer;

    public string sep = ";";

    private string Strheader;

    [SerializeField]
    private Dictionary<string, List<string>> logCollection = new Dictionary<string, List<string>>();

    [SerializeField]
    private bool overwriteLog = false;



    void Start()
    {
        //add list in dictionary 
        logCollection.Add("DateTime", new List<string>());
        logCollection.Add("TrialNumber", new List<string>());
        logCollection.Add("CorrrectSequence", new List<string>());
        logCollection.Add("FishCaught", new List<string>());
        //logCollection.Add("Current Trial", new List<string>());


        //create the file + add the headers inside if the file doesn't exist 
        if (!File.Exists(filepath))
        {
            // Collection of string  
            string[] strheaders = { "DateTime", "TrialNumber", "CorrrectSequence", "FishCaught" };
            // Create a List and add a collection  
            List<string> hearderList = new List<string>();
            hearderList.AddRange(strheaders);
            Strheader = String.Join(sep, hearderList.Select(x => x.ToString()).ToArray());
            File.WriteAllText(filepath, Strheader + "\n");
        }

    }

    public void logevent(Dictionary<string, List<string>> dico)
    {
        logCollection["DateTime"].Add(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss:fff"));

        foreach (KeyValuePair<string, List<string>> pair in logCollection)
        {
            if (pair.Key == "DateTime")
            {
                continue;
            }

            if (dico.ContainsKey(pair.Key))
            {
                logCollection[pair.Key].Add(String.Join("\n", dico[pair.Key]));
            }
            else
            {
                logCollection[pair.Key].Add("");
            }
        }
    }

    void OnApplicationQuit()
    {
        logdate();

    }

    public void logdate()
    {


        if (string.IsNullOrEmpty(filepath))
        {
            Debug.LogError("Filepath was not set!");
        }

        //when the checkbox is on true , the file is overwritted
        if (overwriteLog)
        {

            if (File.Exists(filepath))
            {
                Debug.LogWarning("Overwriting CSV file: " + filepath);
                File.Delete(filepath);
                string[] keys = new string[logCollection.Keys.Count];
                logCollection.Keys.CopyTo(keys, 0);
                string dbCols = string.Join(sep, keys).Replace("\n", string.Empty);

                using (StreamWriter writer = File.AppendText(filepath))
                {
                    writer.WriteLine(dbCols);
                }
            }
        }

        List<string> dataString = new List<string>();
        // Create a string with the data
        for (int i = 0; i < logCollection["DateTime"].Count; i++)
        {
            List<string> row = new List<string>();
            foreach (string key in logCollection.Keys)
            {
                row.Add(logCollection[key][i]);
            }
            dataString.Add(string.Join(sep, row.ToArray()) + sep);
        }

        foreach (var log in dataString)
        {
            using (StreamWriter writer = File.AppendText(filepath))
            {
                writer.WriteLine(log.Replace("\n", string.Empty));
            }
        }

        Debug.Log("Data logged to: " + filepath);
    }

}
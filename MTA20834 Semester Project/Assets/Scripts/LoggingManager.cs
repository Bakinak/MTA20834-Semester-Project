using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.IO;
using System;

public class LoggingManager : MonoBehaviour
{

    string filepath;
    string filename = "keysequencedata";
    string sep = ",";

    private Dictionary<string, List<string>> logCollection;

    // Start is called before the first frame update
    void Awake()
    {
        filepath = Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
        logCollection = new Dictionary<string, List<string>>();
        logCollection["Date"] = new List<string>();
        logCollection["Timestamp"] = new List<string>();
        logCollection["Event"] = new List<string>();
        logCollection["currentCondition"] = new List<string>();
    }

    public void AddNewEvent(string theEvent, string currentCondition) {
        logCollection["Event"].Add(theEvent);
        logCollection["Date"].Add(System.DateTime.Now.ToString("yyyy-MM-dd"));
        logCollection["Timestamp"].Add(System.DateTime.Now.ToString("HH:mm:ss.ffff"));
        logCollection["currentCondition"].Add(currentCondition);
    }


    public void FillKeys() {
       foreach (string key in logCollection.Keys)
        {
            if (logCollection[key].Count < logCollection["Event"].Count) {
                string value;
                if (logCollection[key].Count > 0) {
                    value = logCollection[key][logCollection[key].Count-1];
                } else {
                    value = "NA";
                }
                var amount = logCollection["Event"].Count - logCollection[key].Count;
                if (amount > 0) {
                    for(int i = 0; i < amount; i++)
                    {
                        logCollection[key].Add(value);
                    }
                }
            }
        }
    }

    public void SendLogs() {
        LogToDisk();
        // TODO: Send to MySQL server.
    }

    public void LogToDisk() {
        if (logCollection["Event"].Count == 0) {
            Debug.Log("Nothing to log, returning..");
            return;
        }

        Debug.Log("Saving " + logCollection["Event"].Count + " Rows to " + filepath);
        string dest = filepath + "\\" + filename + "_" + System.DateTime.Now.ToString("yyyy-MM-dd-HH_mm_ss") + ".csv";

        // Log Header
        string[] keys = new string[logCollection.Keys.Count];
        logCollection.Keys.CopyTo(keys, 0);
        string dbCols = string.Join(sep, keys).Replace("\n", string.Empty);

        using (StreamWriter writer = File.AppendText(dest))
        {
            writer.WriteLine(dbCols);
        }

        // Create a string with the data
        List<string> dataString = new List<string>();
        for (int i = 0; i < logCollection["Event"].Count; i++)
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
            using (StreamWriter writer = File.AppendText(dest))
            {
                writer.WriteLine(log.Replace("\n", string.Empty));
            }
        }

        // Clear logCollection
       foreach (string key in logCollection.Keys)
        {
            
            logCollection[key].Clear();
        }
    }




}

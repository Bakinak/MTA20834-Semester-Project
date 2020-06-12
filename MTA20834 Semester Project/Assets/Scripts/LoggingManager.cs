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
        filepath = Application.dataPath;
        //filepath = Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
        logCollection = new Dictionary<string, List<string>>();

        logCollection["Date"] = new List<string>();
        logCollection["Timestamp"] = new List<string>();
        logCollection["currentCondition"] = new List<string>();
        logCollection["Event"] = new List<string>();
        logCollection["WorldID"] = new List<string>();
        logCollection["FishID"] = new List<string>();
        logCollection["TotalFishCaught"] = new List<string>();
        logCollection["BubbleNumber"] = new List<string>();
        logCollection["KeyCode"] = new List<string>();
        logCollection["CorrectKey"] = new List<string>();
        logCollection["KeyExpected"] = new List<string>();
        logCollection["TimeSinceLastKey"] = new List<string>();
        logCollection["SequenceCompleteTime"] = new List<string>();
        logCollection["CorrectSequencesEntered"] = new List<string>();
        logCollection["SequencesFailed"] = new List<string>();
        logCollection["CorrectSequencesDiscarded"] = new List<string>();
        logCollection["TotalAttemptsInBubble"] = new List<string>();
        logCollection["GotFish"] = new List<string>();
        logCollection["InstantFrustration"] = new List<string>();
        logCollection["PerceivedControl"] = new List<string>();
    }

    public void AddNewEvent(string theEvent, string currentCondition) //When the game is started
    {
        logCollection["Event"].Add(theEvent);
        logCollection["Date"].Add(System.DateTime.Now.ToString("yyyy-MM-dd"));
        logCollection["Timestamp"].Add(System.DateTime.Now.ToString("HH:mm:ss.ffff"));
        logCollection["currentCondition"].Add(currentCondition);

        fillKeyColumns();

        logCollection["InstantFrustration"].Add("NA");
        logCollection["PerceivedControl"].Add("NA");

        //logCollection[""].Add();
    }

    public void newKeyInput(string currentCondition, string worldID, string FishID, string TotalFishCaught, string BubbleNumber, string keyCode, string correctKey, string keyExpected,
        string timeSinceLastKey) //Whenever a key is pressed
    {
        logCollection["Event"].Add("KeyDown");
        logCollection["Date"].Add(System.DateTime.Now.ToString("yyyy-MM-dd"));
        logCollection["Timestamp"].Add(System.DateTime.Now.ToString("HH:mm:ss.ffff"));
        logCollection["currentCondition"].Add(currentCondition);

        logCollection["WorldID"].Add(worldID);
        logCollection["FishID"].Add(FishID);
        logCollection["TotalFishCaught"].Add(TotalFishCaught);
        logCollection["BubbleNumber"].Add(BubbleNumber);

        logCollection["KeyCode"].Add(keyCode);
        logCollection["CorrectKey"].Add(correctKey);
        logCollection["KeyExpected"].Add(keyExpected);
        logCollection["TimeSinceLastKey"].Add(timeSinceLastKey);
        logCollection["SequenceCompleteTime"].Add("NA");
        logCollection["GotFish"].Add("NA");
        logCollection["CorrectSequencesEntered"].Add("NA");
        logCollection["SequencesFailed"].Add("NA");
        logCollection["CorrectSequencesDiscarded"].Add("NA");
        logCollection["TotalAttemptsInBubble"].Add("NA");

        logCollection["InstantFrustration"].Add("NA");
        logCollection["PerceivedControl"].Add("NA");
    }

    public void sequenceComplete(string currentEvent, string currentCondition, string worldID, string FishID, string TotalFishCaught, string BubbleNumber, string sequenceCompleteTime) //Whenever a key sequence is failed or completed
    {
        logCollection["Event"].Add(currentEvent);
        logCollection["Date"].Add(System.DateTime.Now.ToString("yyyy-MM-dd"));
        logCollection["Timestamp"].Add(System.DateTime.Now.ToString("HH:mm:ss.ffff"));
        logCollection["currentCondition"].Add(currentCondition);

        logCollection["WorldID"].Add(worldID);
        logCollection["FishID"].Add(FishID);
        logCollection["TotalFishCaught"].Add(TotalFishCaught);
        logCollection["BubbleNumber"].Add(BubbleNumber);

        logCollection["SequenceCompleteTime"].Add(sequenceCompleteTime);

        logCollection["KeyCode"].Add("NA");
        logCollection["CorrectKey"].Add("NA");
        logCollection["KeyExpected"].Add("NA");
        logCollection["TimeSinceLastKey"].Add("NA");
        logCollection["GotFish"].Add("NA");
        logCollection["CorrectSequencesEntered"].Add("NA");
        logCollection["SequencesFailed"].Add("NA");
        logCollection["CorrectSequencesDiscarded"].Add("NA");
        logCollection["TotalAttemptsInBubble"].Add("NA");

        logCollection["InstantFrustration"].Add("NA");
        logCollection["PerceivedControl"].Add("NA");
    }

    public void inputWindowOverLog(string currentCondition, string worldID, string FishID, string TotalFishCaught, string BubbleNumber, 
        string CorrectSequencesEntered, string SequencesFailed, string CorrectSequencesDiscarded, string TotalAttemptsInBubble, string GotFish) //Whenever a wave has passed
    {
        logCollection["Event"].Add("InputWindowOver");
        logCollection["Date"].Add(System.DateTime.Now.ToString("yyyy-MM-dd"));
        logCollection["Timestamp"].Add(System.DateTime.Now.ToString("HH:mm:ss.ffff"));
        logCollection["currentCondition"].Add(currentCondition);

        logCollection["WorldID"].Add(worldID);
        logCollection["FishID"].Add(FishID);
        logCollection["TotalFishCaught"].Add(TotalFishCaught);
        logCollection["BubbleNumber"].Add(BubbleNumber);

        logCollection["CorrectSequencesEntered"].Add(CorrectSequencesEntered);
        logCollection["SequencesFailed"].Add(SequencesFailed);
        logCollection["CorrectSequencesDiscarded"].Add(CorrectSequencesDiscarded);
        logCollection["TotalAttemptsInBubble"].Add(TotalAttemptsInBubble);
        logCollection["GotFish"].Add(GotFish);

        logCollection["KeyCode"].Add("NA");
        logCollection["CorrectKey"].Add("NA");
        logCollection["KeyExpected"].Add("NA");
        logCollection["TimeSinceLastKey"].Add("NA");
        logCollection["SequenceCompleteTime"].Add("NA");

        logCollection["InstantFrustration"].Add("NA");
        logCollection["PerceivedControl"].Add("NA");
    }

    public void logFrustrationLevels(string currentCondition, string TotalFishCaught, string BubbleNumber, string InstantFrustration, string CorrectSequencesEntered, string SequencesFailed, string CorrectSequencesDiscarded, string TotalAttemptsInBubble)
    {
        logCollection["Event"].Add("Quest Complete");
        logCollection["Date"].Add(System.DateTime.Now.ToString("yyyy-MM-dd"));
        logCollection["Timestamp"].Add(System.DateTime.Now.ToString("HH:mm:ss.ffff"));
        logCollection["currentCondition"].Add(currentCondition);

        logCollection["TotalFishCaught"].Add(TotalFishCaught);
        logCollection["BubbleNumber"].Add(BubbleNumber);

        logCollection["CorrectSequencesEntered"].Add(CorrectSequencesEntered);
        logCollection["SequencesFailed"].Add(SequencesFailed);
        logCollection["CorrectSequencesDiscarded"].Add(CorrectSequencesDiscarded);
        logCollection["TotalAttemptsInBubble"].Add(TotalAttemptsInBubble);

        logCollection["InstantFrustration"].Add(InstantFrustration);


        logCollection["WorldID"].Add("NA");
        logCollection["FishID"].Add("NA");
        logCollection["KeyCode"].Add("NA");
        logCollection["CorrectKey"].Add("NA");
        logCollection["KeyExpected"].Add("NA");
        logCollection["TimeSinceLastKey"].Add("NA");
        logCollection["SequenceCompleteTime"].Add("NA");
        logCollection["GotFish"].Add("NA");
        logCollection["PerceivedControl"].Add("NA");
    }

    public void logSessionSummary(string currentCondition, string TotalFishCaught, string BubbleNumber, string InstantFrustration, string CorrectSequencesEntered, string SequencesFailed, string CorrectSequencesDiscarded, string TotalAttemptsInBubble, string PerceivedControl)
    {
        logCollection["Event"].Add("Session Summary");
        logCollection["Date"].Add(System.DateTime.Now.ToString("yyyy-MM-dd"));
        logCollection["Timestamp"].Add(System.DateTime.Now.ToString("HH:mm:ss.ffff"));
        logCollection["currentCondition"].Add(currentCondition);

        logCollection["TotalFishCaught"].Add(TotalFishCaught);
        logCollection["BubbleNumber"].Add(BubbleNumber);

        logCollection["CorrectSequencesEntered"].Add(CorrectSequencesEntered);
        logCollection["SequencesFailed"].Add(SequencesFailed);
        logCollection["CorrectSequencesDiscarded"].Add(CorrectSequencesDiscarded);
        logCollection["TotalAttemptsInBubble"].Add(TotalAttemptsInBubble);

        logCollection["InstantFrustration"].Add(InstantFrustration);
        logCollection["PerceivedControl"].Add(PerceivedControl);

        logCollection["WorldID"].Add("NA");
        logCollection["FishID"].Add("NA");
        logCollection["KeyCode"].Add("NA");
        logCollection["CorrectKey"].Add("NA");
        logCollection["KeyExpected"].Add("NA");
        logCollection["TimeSinceLastKey"].Add("NA");
        logCollection["SequenceCompleteTime"].Add("NA");
        logCollection["GotFish"].Add("NA");
    }

    void fillKeyColumns()
    {

        logCollection["WorldID"].Add("NA");
        logCollection["FishID"].Add("NA");
        logCollection["TotalFishCaught"].Add("NA");
        logCollection["BubbleNumber"].Add("NA");
        logCollection["KeyCode"].Add("NA");
        logCollection["CorrectKey"].Add("NA");
        logCollection["KeyExpected"].Add("NA");
        logCollection["TimeSinceLastKey"].Add("NA");
        logCollection["SequenceCompleteTime"].Add("NA");
        logCollection["GotFish"].Add("NA");
        logCollection["CorrectSequencesEntered"].Add("NA");
        logCollection["SequencesFailed"].Add("NA");
        logCollection["CorrectSequencesDiscarded"].Add("NA");
        logCollection["TotalAttemptsInBubble"].Add("NA");


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

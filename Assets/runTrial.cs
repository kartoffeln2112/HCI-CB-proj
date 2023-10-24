using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class runTrial : MonoBehaviour
{
    public int preTrialTime;
    public float interruptTime;
    public int trialTimeOutTime;
    public int numberTrials;  // for dev purposes
    public static sceneModifier sm;
    private static GameObject screenBlock;
    private static bool inTrial;
    private static bool preTrial;
    private DateTime trialStartTime;

    private int currTrial;
    private int[] trialVals;


    // Start is called before the first frame update
    void Start()
    {
        screenBlock = GameObject.Find(this.name).transform.GetChild(0).gameObject;
        screenBlock.SetActive(true);
        sm = GameObject.Find("TrialObjs").GetComponent<sceneModifier>();
        currTrial = 0;

        inTrial = false;
        preTrial = false;

        // randomize trials
        System.Random rnd = new System.Random();
        trialVals = new int[]{0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11};
        trialVals = trialVals.OrderBy(x => rnd.Next()).ToArray();
    }


    // Update is called once per frame
    void Update()
    {
        // start initial trial
        if (Input.GetKeyDown(KeyCode.Space) && (preTrial == false) && (currTrial < numberTrials))
        {
            preTrial = true;
            StartCoroutine(startTrial(preTrialTime));
        }

        // in trials
        if ((currTrial < numberTrials) && (inTrial == true))
        {
            // wait for end of trial
            DateTime currTime = DateTime.Now;
            //Debug.Log(currTime + "   " + trialStartTime + "   " + (currTime - trialStartTime).TotalSeconds);
            if ((currTime - trialStartTime).TotalSeconds >= trialTimeOutTime)
            {
                Debug.Log("trial " + currTrial + "." + trialVals[currTrial] + " timeout");
                trialFinish(currTime, trialVals[currTrial]);
            }

            if (sm.finTime != null)
            {
                trialFinish(sm.finTime, trialVals[currTrial]);
            }
        }
        // continue to next trial after trial ends
        if ((currTrial < numberTrials) && (inTrial == false))
        {
            preTrial = false;
        }
    }


    // starts the a trial instance for the connected scene
    private IEnumerator startTrial(int waitTime)
    {
        // pre trial scene observation
        screenBlock.SetActive(false);
        Debug.Log("preTrial wait");
        yield return new WaitForSeconds(waitTime);
        Debug.Log("starting trial: " + currTrial + "." + trialVals[currTrial]);
        
        // change environment
        screenBlock.SetActive(true);
        yield return new WaitForSeconds(interruptTime);
        sm.setSceneForTrial(trialVals[currTrial]);
        screenBlock.SetActive(false);
        trialStartTime = DateTime.Now;
        inTrial = true;
    }


    // finishes and resets after a trial
    private void trialFinish(DateTime? time, int trialVal)
    {
        screenBlock.SetActive(true);
        outputResults(time, trialVal);
        sm.resetScene(trialVals[currTrial]);
        currTrial++;
        inTrial = false;
    }


    // outputs trial instance results to csv file
    private void outputResults(DateTime? time, int trialVal)
    {
        using (StreamWriter sw = File.AppendText("results.csv"))
        {
            sw.WriteLine(currTrial + "," + SceneManager.GetActiveScene().name + "," + (time - trialStartTime) + "," + sm.getTrialType(trialVals[currTrial]) + "," + trialVal);
        }
    }
}

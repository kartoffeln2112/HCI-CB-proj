using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class runTrial : MonoBehaviour
{
    public int preTrialTime;
    public int interTrialTime;
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
        trialVals = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
        //System.Random rand = new System.Random();
        //rand.Shuffle(trialVals);

    }

    // Update is called once per frame
    void Update()
    {
        // start initial trial
        if (Input.GetKeyDown(KeyCode.Space) && (preTrial == false))
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
        // continue to next trial
        if ((currTrial < numberTrials) && (inTrial == true))
        {
            startTrial(interTrialTime);
        }

        
    }

    private IEnumerator startTrial(int waitTime)
    {
        // pre trial scene observation
        screenBlock.SetActive(false);
        Debug.Log("preTrial wait");
        yield return new WaitForSeconds(waitTime);
        Debug.Log("starting trial: " + currTrial + "." + trialVals[currTrial]);
        
        // change environment
        screenBlock.SetActive(true);
        yield return new WaitForSeconds(1);
        sm.setSceneForTrial(trialVals[currTrial]);
        screenBlock.SetActive(false);
        trialStartTime = DateTime.Now;
        inTrial = true;
    }

    private void trialFinish(DateTime? time, int trialVal)
    {
        screenBlock.SetActive(true);
        outputResults(time, trialVal);
        sm.resetScene(trialVals[currTrial]);
        currTrial++;
        screenBlock.SetActive(false);
        inTrial = false;
    }

    private void outputResults(DateTime? time, int trialVal)
    {
        using (StreamWriter sw = new StreamWriter("results.csv"))
        {
            sw.WriteLine(currTrial + "," + SceneManager.GetActiveScene().name + "," + (time - trialStartTime) + "," + sm.getTrialType(trialVals[currTrial]) + "," + trialVal);
        }
    }
}

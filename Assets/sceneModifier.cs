using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sceneModifier : MonoBehaviour
{
    public DateTime? finTime;

    private static int[] tTypes;
    private static GameObject[] tObjs;

    // Start is called before the first frame update
    void Start()
    {
        tTypes = new int[12];
        tObjs = new GameObject[12];
        //int i = 0;

        finTime = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // resets all trial objects
    public void resetScene(int i)
    {
        gameObject.transform.GetChild(i).GetComponent<objectModifier>().endTrial();
        finTime = null;
    }

    // sets a trial object to manipulate into trial state
    public void setSceneForTrial(int i)
    {
        Debug.Log(gameObject.transform.GetChild(i).name);
        gameObject.transform.GetChild(i).GetComponent<objectModifier>().startTrial();
    }

    // returns the type of trial that occurs at the index
    public int getTrialType(int index)
    {
        return gameObject.transform.GetChild(index).GetComponent<objectModifier>().tType;
    }
}

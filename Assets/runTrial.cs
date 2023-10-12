using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class runTrial : MonoBehaviour
{
    private GameObject screenBlock;
    private bool inTrial;
    public Timer preTrialTime;
    public Timer trialTimeOutTime;
    
    // Start is called before the first frame update
    void Start()
    {
        screenBlock = GameObject.Find(this.name).transform.GetChild(0).gameObject;
        screenBlock.SetActive(true);
        inTrial = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space") && inTrial == false)
        {
            screenBlock.SetActive(false);
        }
    }
}

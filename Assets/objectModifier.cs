using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/*
    Trial types:
        0 - relocation
        1 - removal/addition
        2 - replacement
        3 - recolor
*/

public class objectModifier : MonoBehaviour
{
    public int tType;
    public Material trialMat;
    public Vector3 trialPos;
    private bool objInTrial;

    private Vector3 ogPos;
    private Material ogMat;

    // Start is called before the first frame update
    void Start()
    {
        objInTrial = false;
        
        // get local position
        ogPos = transform.position;
        // Debug.Log(ogPos);
        ogMat = gameObject.GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {   
        
    }

    // set finish time in parent on mouse click
    public void OnMouseDown()
    {
        Debug.Log(gameObject.name + " clicked on");

        if (objInTrial == true)
        {
            gameObject.GetComponentInParent<sceneModifier>().finTime = DateTime.Now;
        }
    }

    // manipulate object according to trial designation
    public void startTrial()
    {
        objInTrial = true;
        doTrial();
    }

    // revert object to original state
    public void endTrial()
    {
        objInTrial = false;
        doTrial();
    }

    // perform specified trial
    private void doTrial()
    {
        switch (tType)
        {
            case 0:
                relocateTrial();
                break;
            case 1:
                removeTrial();
                break;
            case 2:
                replaceTrial();
                break;
            case 3:
                recolorTrial();
                break;
            default:
                Debug.Log("ERROR in objectModifier on" + gameObject.name + ": invalid trial type");
                break;
        }
    }

    private void relocateTrial()
    {
        if (objInTrial)
        {
            Debug.Log("setting to trial position: " + trialPos + " from pos " + ogPos);
            transform.position = trialPos;
        }
        else
        {
            Debug.Log("setting to original position: " + ogPos);
            // expects GLOBAL position, act accordingly
            transform.position = ogPos;
        }
    }

    private void removeTrial()
    {
        // TODO
    }

    private void replaceTrial()
    {
        // TODO
    }

    private void recolorTrial()
    {
        // TODO
    }
}

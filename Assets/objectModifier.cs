using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/*
    Trial types:
        0 - relocation
        1 - removal
        2 - replacement
        3 - addition
*/

public class objectModifier : MonoBehaviour
{
    public int tType;
    public Material trialMat;
    public Vector3 trialPos;
    public Vector3 trialRotate;
    public GameObject trialObj;


    private bool objInTrial;
    private Vector3 ogPos;
    private Vector3 ogRot;
    private Material ogMat;

    // Start is called before the first frame update
    void Start()
    {
        objInTrial = false;
        
        // get local position
        ogPos = transform.position;
        ogRot = transform.rotation.eulerAngles;
        // Debug.Log(ogPos);
        ogMat = gameObject.GetComponent<Renderer>().material;

        // make sure object is not visible if its an addition trial
        if (tType == 3)
        {
            gameObject.GetComponent<Renderer>().enabled = false;
        }
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
                additionTrial();
                break;
            default:
                Debug.Log("ERROR in objectModifier on" + gameObject.name + ": invalid trial type");
                break;
        }
    }

    private void relocateTrial()
    {
        Vector3 toRotate = trialRotate - ogRot;
        if (objInTrial)
        {
            Debug.Log("setting to trial position: " + trialPos + " from pos " + ogPos);
            transform.position = trialPos;
            transform.Rotate(toRotate.x, toRotate.y, toRotate.z, Space.Self);
        }
        else
        {
            Debug.Log("setting to original position: " + ogPos);
            // expects GLOBAL position, act accordingly
            transform.position = ogPos;
            transform.Rotate(-toRotate.x, -toRotate.y, -toRotate.z, Space.Self);
        }
    }

    private void removeTrial()
    {
        if (objInTrial)
        {
            Debug.Log("remove trial begin");
            gameObject.GetComponent<Renderer>().enabled = false;
        }
        else
        {
            Debug.Log("remove trial end");
            gameObject.GetComponent<Renderer>().enabled = true;
        }
    }

    private void replaceTrial()
    {
        if (objInTrial)
        {
            Debug.Log("replacement trial begin");
            gameObject.GetComponent<Renderer>().enabled = false;
            trialObj.SetActive(true);
        }
        else
        {
            Debug.Log("replacement trial end");
            gameObject.GetComponent<Renderer>().enabled = true;
            trialObj.SetActive(false);
        }
    }

    private void additionTrial()
    {
        if (objInTrial)
        {
            Debug.Log("addition trial begin");
            gameObject.GetComponent<Renderer>().enabled = true;
        }
        else
        {
            Debug.Log("addition trial end");
            gameObject.GetComponent<Renderer>().enabled = false;
        }
    }
}

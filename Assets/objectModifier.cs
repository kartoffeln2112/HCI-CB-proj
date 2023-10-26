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
    public Vector3 trialPos;
    public Vector3 trialRotate;
    public GameObject trialObj;


    private bool objInTrial;
    private Vector3 ogPos;
    private Vector3 ogRot;

    // Start is called before the first frame update
    void Start()
    {
        objInTrial = false;
        
        // get local position
        ogPos = transform.position;
        ogRot = transform.rotation.eulerAngles;
        // Debug.Log(ogPos);

        // make sure object is not visible if its an addition trial
        if (tType == 3)
        {
            switchVisibilityState(false);
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
            switchVisibilityState(false);
        }
        else
        {
            Debug.Log("remove trial end");
            switchVisibilityState(true);
        }
    }

    private void replaceTrial()
    {
        if (objInTrial)
        {
            Debug.Log("replacement trial begin");
            switchVisibilityState(false);
            trialObj.SetActive(true);
        }
        else
        {
            Debug.Log("replacement trial end");
            switchVisibilityState(true);
            trialObj.SetActive(false);
        }
    }

    private void additionTrial()
    {
        if (objInTrial)
        {
            Debug.Log("addition trial begin");
            switchVisibilityState(true);
        }
        else
        {
            Debug.Log("addition trial end");
            switchVisibilityState(false);
        }
    }


    // switches visibility state (using renderer) for an object or group of objects
    private void switchVisibilityState(bool state)
    {
        if (TryGetComponent<Renderer>(out Renderer render))
        {
            render.enabled = state;
            if (gameObject.transform.childCount > 0)
            {
                for (int i = 0; i < gameObject.transform.childCount; i++)
                {
                    gameObject.transform.GetChild(i).GetComponent<Renderer>().enabled = state;
                }
            }
        }
        else
        {
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                gameObject.transform.GetChild(i).GetComponent<Renderer>().enabled = state;
            }
        }
    }
}

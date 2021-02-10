using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalLogic : Goal
{
    private bool wait = false;
    public Rays rays;
    
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    { 
    }

    public void InitRotation()
    {
        msc.DestroyMaze();
        rays = FindObjectOfType<Rays>();
        //Activate a function from rays
        rays.InitRays();
    }

    public void RotationReset()
    {
        if (!end)
        {
            msc.InitNextMaze();
            ChangeMovementWireframe();
            // move the player
            ResetUser();
            // change the Locomotion method

            sg.Reset(end);
        }
        else
        {
            ResetUser();
            im.activateEndMessage(0);
        }
        
        
    }

    public void ResetTutorial()
    {
        sg.Reset(end);
        im.isTutorial = false;
        im.deactivateEndMessage();
        FindObjectOfType<TutorialInit>().activateTutorialMessage();
    }
    public void enableRotation()
    {
        isRotation = true ;
        Debug.Log(isRotation);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.tag == "XRRig" && uim.canChange() && !isRotation)
        {
            wait = true;
            uim.setLastGoal(Time.fixedTime);
            sg.check = false;
            ResetUser();
            
            if (!sg.tutorial)
            {
                // Generate new maze
                ResetMaze(!end);
                // move the player
                
                // change the Locomotion method
                ChangeMovementWireframe();
                sg.Reset(end);

            }
            else
            {
                im.isTutorial = true;
                im.activateEndMessage(1);
                FindObjectOfType<TutorialInit>().deactivateTutorialMessage();
                
            }

        }

        if (other.tag == "XRRig" && isRotation)
        {
            InitRotation();
            transform.GetComponent<Renderer>().enabled = false;
        }
    }

}

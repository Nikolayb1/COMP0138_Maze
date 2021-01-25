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
        ResetMaze(false);
        rays = FindObjectOfType<Rays>();
        //Activate a function from rays
        rays.InitRays();
    }

    public void RotationReset()
    {
        ResetMaze(!end);
        // move the player
        ResetUser();
        // change the Locomotion method
        ChangeMovementWireframe();
        sg.Reset();
    }

    public void ResetTutorial()
    {
        sg.Reset();
        im.isTutorial = false;
        im.deactivateEndMessage();
        if (uim.GetMovementMode() == UIManager.MovementType.Fog)
        {
            im.SetFog(true);
        }
    }


    protected override void OnTriggerEnter(Collider other)
    {
        if (other.tag == "XRRig" && uim.canChange())
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
                sg.Reset();

            }
            else
            {
                im.isTutorial = true;
                im.SetFog(false);
                im.activateEndMessage(1);
            }

        }
    }

}

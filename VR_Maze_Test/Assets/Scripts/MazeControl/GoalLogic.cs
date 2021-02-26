using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        //msc.DestroyMaze();
        rays = FindObjectOfType<Rays>();
        //Activate a function from rays
        rays.InitRays();
    }

    public void RotationReset()
    {
        if (!end)
        {
            msc.DestroyMaze();
            msc.InitNextMaze();
            ChangeMovementWireframe();
            // move the player
            ResetUser();
            // change the Locomotion method

            sg.Reset(end);
        }
        else
        {
            msc.DestroyMaze();
            ResetUser();
            im.activateEndMessage(0);
        }
        
        
    }

    public void progressTutorial()
    {
        if (!end)
        {
            sg.Reset(end);
            FindObjectOfType<TutorialInit>().nextMovementMethod();
            im.isTutorial = false;
            im.deactivateEndMessage();
            FindObjectOfType<TutorialInit>().activateTutorialMessage();
        }
        else
        {
            SceneManager.LoadScene("QuickMaze", LoadSceneMode.Single);
        }
        
        
    }

    public void progressExperiment()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (SceneManager.sceneCount > nextSceneIndex)
        {
            SceneManager.LoadScene(nextSceneIndex);
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

    public void endMazeEvent()
    {
        string val = gl.dataToJson();
        l.LogEvent("EM", val);
        gl.clearLog();
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
                endMazeEvent();
                ResetMaze(!end);
                // move the player
                
                // change the Locomotion method
                ChangeMovementWireframe();
                sg.Reset(end);

            }
            else
            {
                im.isTutorial = true;
                
                if (!end)
                {
                    im.canChangeTutorial = true;
                    im.activateEndMessage(1);
                }
                else
                {
                    im.canChaneScene = true;
                    im.activateEndMessage(2);
                }
                
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

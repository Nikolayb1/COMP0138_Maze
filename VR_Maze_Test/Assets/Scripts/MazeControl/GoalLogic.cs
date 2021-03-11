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
        endMazeEvent();
        ResetUser();
        ResetMaze();
        
        // move the player

        // change the Locomotion method

        sg.Reset(end);
        Destroy(gameObject);
        /*if (!end)
        {
            ResetUser();
            ResetMaze();
            // move the player

            // change the Locomotion method

            sg.Reset(end);
            Destroy(transform);
        }
        else
        {
            ResetUser();
            ResetMaze();
            im.activateEndMessage(3);
        }*/


    }

    public void progressTutorial()
    {
        if (!end)
        {
            Player.transform.rotation = Quaternion.Euler(0, -33, 0);
            sg.Reset(end);
            FindObjectOfType<TutorialInit>().nextMovementMethod();
            im.isTutorial = false;
            im.deactivateEndMessage();
            FindObjectOfType<TutorialInit>().activateTutorialMessage();
        }
        else
        {
            progressExperiment();
        }

        Destroy(gameObject);


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
        im.ChangeMovement();
        FindObjectOfType<TutorialInit>().activateTutorialMessage();
        Destroy(gameObject);
    }
    public void enableRotation()
    {
        isRotation = true ;
        gameObject.SetActive(true);
    }

    public void endMazeEvent()
    {
        string val = gl.dataToJson();
        l.LogEvent("EM", val);
        l.LogEvent("ID", CrossSceneData.CrossSceneId.ToString());
        if (isRotation)
        {
            l.LogEvent("MA", "Rotation Maze");
        }
        else
        {
            l.LogEvent("MA", "Back and Forth Maze");

        }
        
        switch (uim.GetMovementMode())
        {
            case UIManager.MovementType.Teleport:
                l.LogEvent("MT", "Teleport");
                break;
            case UIManager.MovementType.Dash:
                l.LogEvent("MT", "Dash");
                break;
            case UIManager.MovementType.Walk:
                l.LogEvent("MT", "Blur");
                break;

            case UIManager.MovementType.Fog:
                l.LogEvent("MT", "Fog");
                break;
        }
        
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
                ResetMaze();
                // move the player
                
                // change the Locomotion method
                sg.Reset(end);
                Destroy(gameObject);

            }
            else
            {
                im.isTutorial = true;
                
                if (uim.GetMovementMode() != UIManager.MovementType.Fog)
                {
                    im.canChangeTutorial = true;
                    im.activateEndMessage(1);
                }
                else if(uim.GetMovementMode() == UIManager.MovementType.Fog)
                {
                    im.canChaneScene = true;
                    im.canChangeTutorial = true;
                    im.activateEndMessage(2);
                    im.SetFog(false);
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

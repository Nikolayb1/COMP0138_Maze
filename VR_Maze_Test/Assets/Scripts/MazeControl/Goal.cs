using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Goal : MonoBehaviour
{
    public MazeSpawner ms;
    public GridLogger gl;
    public Logger l;
    public GameObject Player;
    public InputManager im;
    public UIManager uim;
    public GameObject msgo;
    public StartGoal sg;
    public MazeList msc;
    public GameObject HMD;
    public bool end;
    public bool isRotation;
    
    // Start is called before the first frame update
    protected virtual void Start()
    {

        gl = FindObjectOfType<GridLogger>();
        l = FindObjectOfType<Logger>();
        end = false;
        msc = FindObjectOfType<MazeList>();
        Player = GameObject.FindGameObjectWithTag("XRRig");
        HMD = Player.GetComponent<XRRig>().cameraGameObject;
        ms = FindObjectOfType<MazeSpawner>();
        im = FindObjectOfType<InputManager>();
        uim = FindObjectOfType<UIManager>();
        msgo = GameObject.FindGameObjectWithTag("Maze");
        sg = FindObjectOfType<StartGoal>();
        transform.GetComponent<Renderer>().enabled = true;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    protected virtual void OnTriggerEnter(Collider other) {
        //ChangeMovementWireframe();
    }


    public void ResetMaze()
    {
        if (ms == null)
        {
            ms = FindObjectOfType<MazeSpawner>();
        }
        // change to showing a message;
        ms.ShowIntermission();
    }

    public void ResetUser()
    {
        //Player.GetComponent<TeleportationProvider>().StopMovement();
        //Player.GetComponent<TeleportationProvider>().StopAllCoroutines();
        uim.setPosTime(Time.fixedTime);
        Player.transform.position = new Vector3(0f, 0.55f, -2.5f);
        if (sg.tutorial)
        {
            Player.transform.rotation = Quaternion.Euler(0, 0 - HMD.transform.localRotation.eulerAngles.y, 0);
            
        }
        else
        {
            Player.transform.rotation = Quaternion.Euler(0, 90 - HMD.transform.localRotation.eulerAngles.y, 0);
        }
        

        sg.check = false;
    }
}

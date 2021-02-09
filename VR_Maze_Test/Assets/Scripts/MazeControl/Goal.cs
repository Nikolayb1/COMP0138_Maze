﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Goal : MonoBehaviour
{
    public MazeSpawner ms;
    public GameObject Player;
    public InputManager im;
    public UIManager uim;
    public GameObject msgo;
    public StartGoal sg;
    public MazeList msc;
    public bool end;
    public bool isRotation;
    
    // Start is called before the first frame update
    protected virtual void Start()
    {
        end = false;
        //isRotation = false;
        msc = FindObjectOfType<MazeList>();
        Player = GameObject.FindGameObjectWithTag("XRRig");
        ms = FindObjectOfType<MazeSpawner>();
        im = FindObjectOfType<InputManager>();
        uim = FindObjectOfType<UIManager>();
        msgo = GameObject.FindGameObjectWithTag("Maze");
        sg = FindObjectOfType<StartGoal>();
        
              
        if(uim.GetMovementMode() == UIManager.MovementType.Fog)
        {
            end = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    protected virtual void OnTriggerEnter(Collider other) {
        //ChangeMovementWireframe();
    }

    public void ChangeMovementWireframe()
    {
        if (end)
        {
            im.activateEndMessage(0);
        }
        else
        {
            
            uim.ChangeMovementType();
            im.ChangeMovement();

            if (uim.GetMovementMode() == UIManager.MovementType.Walk)
            {
                uim.SetWireframeMode(UIManager.WireframeMode.Auto);
                im.ChangeWireframe();
            }
            else
            {
                uim.SetWireframeMode(UIManager.WireframeMode.Off);
                im.ChangeWireframe();
            }

            
        }
        
    }

    public void ResetMaze(bool newMaze)
    {
        if (ms == null)
        {
            ms = FindObjectOfType<MazeSpawner>();
        }
        ms.GenerateMaze(newMaze);
    }

    public void ResetUser()
    {
        //Player.GetComponent<TeleportationProvider>().StopMovement();
        //Player.GetComponent<TeleportationProvider>().StopAllCoroutines();
        uim.setPosTime(Time.fixedTime);
        Player.transform.position = new Vector3(0f, 0.55f, -2.5f);
        
        sg.check = false;
        Debug.Log("Should Move now");
    }
}

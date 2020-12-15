using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GoalLogic : Goal
{
    private bool wait = false;
    private float timeOffsetVal = 1f;
    private float timeOffset;
    private float timer;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (wait)
        {
            if (timer > timeOffset)
            {
                wait = false;
            }
            else
            {
                timer += Time.deltaTime;
            }
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.tag == "XRRig" && !wait)
        {
            wait = true;
            Debug.Log("Your are done");
            // Destroy Maze


            // Generate new maze
            ms.GenerateMaze();
            // move the player
            Player.GetComponent<TeleportationProvider>().StopMovement();
            Player.transform.position = new Vector3(0f, 0.5f, -2.5f);
            // change the Locomotion method
            ChangeMovementWireframe();
            sg.Reset();
        }
    }

}

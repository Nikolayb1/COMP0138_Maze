using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GoalLogic : MonoBehaviour
{
    private MazeSpawner ms;
    private GameObject Player;
    public InputManager im;
    public UIManager uim;
    private bool wait = false;
    public GameObject msgo;
    private float timeOffsetVal = 1f;
    private float timeOffset;
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("XRRig");
        ms = FindObjectOfType<MazeSpawner>();
        im = FindObjectOfType<InputManager>();
        uim = FindObjectOfType<UIManager>();
        msgo = GameObject.FindGameObjectWithTag("Maze");
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

    public void OnTriggerEnter(Collider other)
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
            Debug.Log(Player.transform.position);
            // change the Locomotion method
            uim.ChangeMovementType();
            im.ChangeMovement();

            if(uim.GetMovementMode() == UIManager.MovementType.Walk)
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

}

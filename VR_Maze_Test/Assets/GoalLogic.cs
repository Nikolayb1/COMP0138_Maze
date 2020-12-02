using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalLogic : MonoBehaviour
{
    private MazeSpawner ms;
    private Transform Player;
    public InputManager im;
    public UIManager uim;
    private bool wait = false;
    // Start is called before the first frame update
    void Start()
    {
        GameObject temp = GameObject.Find("XRRig");
        Player = temp.transform;
        ms = FindObjectOfType<MazeSpawner>();
        im = FindObjectOfType<InputManager>();
        uim = FindObjectOfType<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.name == "XRRig" && !wait)
        {
            wait = true;
            Debug.Log("Your are done");
            // Destroy Maze
            foreach (Transform child in this.transform)
            {
                Destroy(child.gameObject);
            }

            // Generate new maze
            ms.GenerateMaze();
            // move the player
            Player.position = new Vector3(0f, 0.5f, 0f);
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

    public void OnTriggerExit(Collider other)
    {
        wait = false;
    }
}

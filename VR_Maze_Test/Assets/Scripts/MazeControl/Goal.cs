using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public MazeSpawner ms;
    public GameObject Player;
    public InputManager im;
    public UIManager uim;
    public GameObject msgo;
    public StartGoal sg;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        Player = GameObject.FindGameObjectWithTag("XRRig");
        ms = FindObjectOfType<MazeSpawner>();
        im = FindObjectOfType<InputManager>();
        uim = FindObjectOfType<UIManager>();
        msgo = GameObject.FindGameObjectWithTag("Maze");
        sg = FindObjectOfType<StartGoal>();
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

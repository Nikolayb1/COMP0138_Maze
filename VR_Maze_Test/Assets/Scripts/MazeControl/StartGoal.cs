using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGoal : MonoBehaviour
{
    private int[] locations = { 0,2,4,6,8};
    public enum ending{
        goal,
        goalAtStart,
        orientationCheck
    }
    public GameObject EndObject;
    public GameObject marker;
    public GameObject spawnedMarker;
    public GameObject spawnedGoal;
    private Collider StartObjectCollider;
    private Collider PlayerCollider;
    private bool goalCreated;
    public bool tutorial;
    public GoalLogic GL;
    public UIManager uim;
    public GameObject Player;
    public bool check;

    private int numberOfSpawns;
    public int spawnLimit;
    public ending end;
    private bool markerSpawned;

    // Start is called before the first frame update
    void Start()
    {
        uim = FindObjectOfType<UIManager>();
        goalCreated = false;
        markerSpawned = false;
        numberOfSpawns = 0;
    }
    //spawn a marker at a random point
    public void Spawn()
    {
        int x = Random.Range(0, 6);
        int y = Random.Range(5, 10);
        while (x <= 2 && y <= 2)
        {
            x = Random.Range(0, 5);
            y = Random.Range(0, 5);
        }
        Debug.Log("X: " + x + "; Y: " + y);
        spawnedMarker = Instantiate(marker, new Vector3(x*2, 1f, y*2), Quaternion.identity);
        markerSpawned = true;
        numberOfSpawns++;
    }
    public void Reset(bool e)
    {
        if (e)
        {
            Destroy(spawnedGoal);
        }
        else
        {
            goalCreated = false;
            markerSpawned = false;
            numberOfSpawns = 0;
            Destroy(spawnedGoal);
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        
        if (!check)
        {
            if (!uim.didSet(Player.transform.position))
            {

                Player.transform.position = new Vector3(0f, 0.55f, -2.5f);

            }
            else
            {
                check = true;
            }
        }
        
        if (!spawnedMarker)
        {
            markerSpawned = false;
        }
        if (!markerSpawned && numberOfSpawns < spawnLimit){
            Spawn();
        }
        if(numberOfSpawns >= spawnLimit && !goalCreated && markerSpawned == false)
        {
            Debug.Log("this is the end");
            switch (end)
            {
                case ending.goal:
                    spawnedGoal = Instantiate(EndObject, new Vector3(10f, 1f, 20f), Quaternion.identity);
                    goalCreated = true;
                    break;
                case ending.goalAtStart:
                    spawnedGoal = Instantiate(EndObject, new Vector3(0f, 1f, 0f), Quaternion.identity);
                    goalCreated = true;
                    break;
                case ending.orientationCheck:
                    // Send a message to a funciton which deletes the maze and activates the ray
                    spawnedGoal = Instantiate(EndObject, new Vector3(3f, 1f, 3.5f), Quaternion.identity);
                    goalCreated = true;
                    GL = spawnedGoal.GetComponent<GoalLogic>();
                    //spawnedGoal.SetActive(false);

                    GL.enableRotation();
                    
                    break;
            }
        }
    }

}

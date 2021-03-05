using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGoal : MonoBehaviour
{
    private int[] locations = { 0,2,4,6,8};
    private int[,] spawnLocations = { { 3,5}, { 1, 4 }, {3 ,4}, { 3,4} };
    private int[,] tutorialLocations = { { 1, 1 }, { 3, 3 } };
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
    public bool BaF;
    private int spawnI;
    public GoalLogic GL;
    public UIManager uim;
    public InputManager im;
    public GameObject Player;
    public bool check;
    public Logger l;
    public GameObject movementWarning;
    public GameObject endMassage;
    public GameObject pointer;

    private int numberOfSpawns;
    public int spawnLimit;
    public ending end;
    private bool markerSpawned;
    public GameObject[] markerMessages = new GameObject[4];
    private int markerId;
    private bool markerMessageEnabled;
    private float markerMessageStart;
    private float markerMessageEnd;

    // Start is called before the first frame update
    void Start()
    {
        markerMessageEnabled = false;
        movementWarning.SetActive(false);
        spawnI = 0;
        im = FindObjectOfType<InputManager>();
        uim = FindObjectOfType<UIManager>();
        goalCreated = false;
        markerSpawned = false;
        numberOfSpawns = 0;
        im.canChaneScene = false;
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
        spawnedMarker.GetComponent<Marker>().setTutorial(tutorial);
        markerSpawned = true;
        numberOfSpawns++;
    }

    public void Spawn(int i)
    {
        markerId = i;
        spawnedMarker = Instantiate(marker, new Vector3(spawnLocations[i,0] * 2, 1f, spawnLocations[i, 1] * 2), Quaternion.identity);
        spawnedMarker.GetComponent<Marker>().setTutorial(tutorial);
        markerSpawned = true;
        numberOfSpawns++;
    }

    public void SpawnTutorial(int i)
    {
        spawnedMarker = Instantiate(marker, new Vector3(tutorialLocations[i, 0] * 2, 1f, tutorialLocations[i, 1] * 2), Quaternion.identity);
        spawnedMarker.GetComponent<Marker>().setTutorial(tutorial);
        markerSpawned = true;
        numberOfSpawns++;
    }

    public void enableMarkerMessage()
    {
        markerMessages[markerId].SetActive(true);
        markerMessageEnabled = true;
        markerMessageStart = Time.fixedTime;
        markerMessageEnd = markerMessageStart + 5;
    }

    public void disableMarkerMessage()
    {
        markerMessageEnabled = false;
        markerMessages[markerId].SetActive(false);
    }

    public void Reset(bool e)
    {
        if (!e)
        {
            goalCreated = false;
            markerSpawned = false;
            numberOfSpawns = 0;
            Destroy(spawnedGoal);
        }
        else
        {
            goalCreated = false;
            markerSpawned = false;
            numberOfSpawns = 0;
            Destroy(spawnedGoal);
            im.canChaneScene = true;
            if (!tutorial)
            {
                l.UploadLogs();
            }
            
        }
        
    }
    // Update is called once per frame
    void Update()
    {

        if (markerMessageEnabled)
        {
            if (Time.fixedTime > markerMessageEnd)
            {
                disableMarkerMessage();
            }
        }

        if (!check)
        {
            if (!uim.didSet(Player.transform.position))
            {
                if (endMassage.activeSelf)
                {
                    movementWarning.SetActive(false);
                }
                else
                {


                    movementWarning.SetActive(true);
                }
                Player.transform.position = new Vector3(0f, 0.55f, -2.5f);

            }
            else
            {
                // disable the warning.
                movementWarning.SetActive(false);
                check = true;
            }
        }
        
        if (!spawnedMarker)
        {
            markerSpawned = false;
        }
        if (!markerSpawned && numberOfSpawns < spawnLimit){
            if (BaF)
            {
                Spawn(spawnI);
            }
            else if (tutorial)
            {
                SpawnTutorial(numberOfSpawns);
            }
            else
            {
                Spawn();
            }
            
        }
        if(numberOfSpawns >= spawnLimit && !goalCreated && markerSpawned == false)
        {
            Debug.Log("this is the end");
            switch (end)
            {
                case ending.goal:
                    spawnedGoal = Instantiate(EndObject, new Vector3(10f, 1f, 12f), Quaternion.identity);
                    goalCreated = true;
                    break;
                case ending.goalAtStart:
                    spawnedGoal = Instantiate(EndObject, new Vector3(0f, 1f, 0f), Quaternion.identity);
                    goalCreated = true;
                    spawnI += 1;
                    break;
                case ending.orientationCheck:
                    // Send a message to a funciton which deletes the maze and activates the ray
                    spawnedGoal = Instantiate(EndObject, new Vector3(-2f, 1f, 15f), Quaternion.identity);
                    pointer.SetActive(false);
                    goalCreated = true;
                    GL = spawnedGoal.GetComponent<GoalLogic>();
                    //spawnedGoal.SetActive(false);

                    GL.enableRotation();
                    
                    break;
            }
        }
    }

}

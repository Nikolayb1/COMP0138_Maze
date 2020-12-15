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

    private int numberOfSpawns;
    public int spawnLimit;
    public ending end;
    private bool markerSpawned;

    // Start is called before the first frame update
    void Start()
    {
        goalCreated = false;
        markerSpawned = false;
        numberOfSpawns = 0;
    }
    //spawn a marker at a random point
    public void Spawn()
    {
        int x = Random.Range(0, 5);
        int y = Random.Range(0, 5);
        Debug.Log("X: " + x + "; Y: " + y);
        spawnedMarker = Instantiate(marker, new Vector3(locations[x], 1f, locations[y]), Quaternion.identity);
        markerSpawned = true;
        numberOfSpawns++;
    }
    public void Reset()
    {
        goalCreated = false;
        markerSpawned = false;
        numberOfSpawns = 0;
        Destroy(spawnedGoal);
    }
    // Update is called once per frame
    void Update()
    {
        if (!spawnedMarker)
        {
            markerSpawned = false;
        }
        if (!markerSpawned && numberOfSpawns < spawnLimit){
            Spawn();
        }
        if(numberOfSpawns >= spawnLimit && !goalCreated && markerSpawned == false)
        {
            switch (end)
            {
                case ending.goal:
                    spawnedGoal = Instantiate(EndObject, new Vector3(8f, 1f, 9f), Quaternion.identity);
                    goalCreated = true;
                    break;
                case ending.goalAtStart:
                    spawnedGoal = Instantiate(EndObject, new Vector3(0f, 1f, 0f), Quaternion.identity);
                    goalCreated = true;
                    break;
                case ending.orientationCheck:
                    goalCreated= true;
                    break;
            }
        }
    }

}

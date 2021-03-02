using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeList : MonoBehaviour
{

    public GameObject[] mazes;
    public GameObject[] messages;
    private int counter;
    private GameObject maze;
    private GameObject message;
    // Start is called before the first frame update
    void Start()
    {
        counter = -1;
        InitNextMaze();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitNextMaze()
    {
        counter++;
        maze = Instantiate(mazes[counter], new Vector3(-2f, 0.5f, 15f), Quaternion.Euler(-90f, 180f, 0f));
        message = Instantiate(messages[counter]);
        maze.transform.parent = transform;
    }

    public void DestroyMaze()
    {
        if (maze != null)
        {
            Destroy(maze);
            Destroy(message);
        }
        
    }
}

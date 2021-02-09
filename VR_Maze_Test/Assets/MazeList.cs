using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeList : MonoBehaviour
{

    public GameObject[] mazes;
    private int counter;
    private GameObject maze;
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
        maze = Instantiate(mazes[counter], new Vector3(3f, 0.5f, 3.5f), Quaternion.Euler(-90f, -90f, 0f));
        maze.transform.parent = transform;
    }

    public void DestroyMaze()
    {
        if (maze != null)
        {
            Destroy(maze);
        }
        
    }
}

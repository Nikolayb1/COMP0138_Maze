using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallOverlap : MonoBehaviour
{
    private GameObject[] walls;
    private int n = 0;
    private bool del = false;
    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if (n == 0)
        {
            findWalls();
            del = false;
        }
        if (n > 0 && !del)
        {
            Debug.Log(n);
            for (int i = 0; i < n; i++)
            {
                for (int j = i + 1; j < n; j++)
                {

                    if (walls[i].transform.position == walls[j].transform.position)
                    {

                        Destroy(walls[j]);
                        continue;
                    }
                }
            }
            del = true;
            findWalls();
        }
    }

    void findWalls()
    {
        walls = GameObject.FindGameObjectsWithTag("wall");
        n = walls.Length;
    }

}

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
        
    }

    void FindWalls()
    {
        Debug.Log("Looking for walls");
        walls = GameObject.FindGameObjectsWithTag("wall");
        n = walls.Length;
        Debug.Log("Found " + n + " walls");
    }

    public void RemoveDuplicateWalls(List<GameObject> oldWalls)
    {
        FindWalls();
        del = false;
        int delwalls = 0;
        Debug.Log("Rmoving Duplicates");
        if (n > 0 && !del)
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = i + 1; j < n; j++)
                {
                    bool found = false;
                    for (int z = 0; z < oldWalls.Count; z++)
                    {
                        if(walls[i] == oldWalls[z] || walls[j] == oldWalls[z])
                        {
                            found = true;
                            break;
                        }
                    }
                    if (found)
                    {
                        continue;
                    }
                    if (walls[i].transform.position == walls[j].transform.position)
                    {

                        Destroy(walls[j]);
                        delwalls++;
                        continue;
                    }
                }
            }
            del = true;
        }

        Debug.Log("Removed " + delwalls + " walls");

    }

}

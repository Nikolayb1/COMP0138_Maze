using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridLogger : MonoBehaviour
{
    /*
     * when exit add to the csv file
     * 
     */

    public GameObject gridCell;
    private List<LogCell> gridCells = new List<LogCell>();
    Dictionary<int, float> enteredCells = new Dictionary<int, float>();
    // Start is called before the first frame update
    void Start()
    {
        for(float i = 0; i<10; i += 2)
        {
            for (float j = 0; j < 10; j += 2)
            {
                gridCells.Add(Instantiate(gridCell, new Vector3(i, 1.5f, j), Quaternion.Euler(0, 0, 0)).GetComponent<LogCell>()); ;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < gridCells.Count; i++)
        {
            if (gridCells[i].entered)
            {
                if (!enteredCells.ContainsKey(i))
                {
                    enteredCells.Add(i, Time.fixedTime);
                }
            }
        }
        List<int> temp = new List<int>();
        foreach (KeyValuePair<int, float> e in enteredCells)
        {
            if (!gridCells[e.Key].entered)
            {
                Debug.Log(e.Key + " : " + (Time.fixedTime - e.Value));
                temp.Add(e.Key);
            }
        }

        foreach(int k in temp)
        {
            enteredCells.Remove(k);
        }
    }
}

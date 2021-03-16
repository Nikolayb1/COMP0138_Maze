using System;
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
    public MazeSpawner ms;
    private List<LogCell> gridCells = new List<LogCell>();
    public List<LogCell> gridCellsSmall = new List<LogCell>();
    Dictionary<int, float> enteredCells = new Dictionary<int, float>();
    public MyCollection myCollection = new MyCollection();
    // Start is called before the first frame update
    void Start()
    {
        for (float i = 0; i < 30; i += 2)
        {
            for (float j = 0; j < 34; j += 2)
            {
                GameObject cell = Instantiate(gridCell, new Vector3(i-16, 1.5f, j), Quaternion.Euler(0, 0, 0));
                gridCells.Add(cell.GetComponent<LogCell>());
                if (i < 28 && i > 14)
                {
                    if (j < 12)
                    {
                        // add to another list;
                        gridCellsSmall.Add(cell.GetComponent<LogCell>());
                    }
                }
            }
        }
        
        
    }

    // Update is called once per frame
    void Update()
    {
        // If circle use the other big one if small use the other one.
        if (ms.isCircle)
        {
            for (int i = 0; i < gridCells.Count; i++)
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
                    //Debug.Log(e.Key + " : " + (Time.fixedTime - e.Value));
                    myCollection.gridLogData.Add(new GridLogData(e.Key, Time.fixedTime, Time.fixedTime - e.Value));
                    temp.Add(e.Key);
                }
            }

            foreach (int k in temp)
            {
                enteredCells.Remove(k);
            }
        }
        else
        {
            for (int i = 0; i < gridCellsSmall.Count; i++)
            {
                if (gridCellsSmall[i].entered)
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
                if (!gridCellsSmall[e.Key].entered)
                {
                    //Debug.Log(e.Key + " : " + (Time.fixedTime - e.Value));
                    myCollection.gridLogData.Add(new GridLogData(e.Key, Time.fixedTime, Time.fixedTime - e.Value));
                    temp.Add(e.Key);
                }
            }

            foreach (int k in temp)
            {
                enteredCells.Remove(k);
            }
        }
        
    }

    public string dataToJson()
    {
        string json = JsonUtility.ToJson(myCollection);
        Debug.Log(json);
        return json;
    }

    public void clearLog()
    {
        myCollection.gridLogData.Clear();
        enteredCells.Clear();
        for (int i = 0; i < gridCells.Count; i++)
        {
            if (gridCells[i].entered)
            {
                gridCells[i].entered = false;
            }
        }
        for (int i = 0; i < gridCellsSmall.Count; i++)
        {
            if (gridCellsSmall[i].entered)
            {
                gridCellsSmall[i].entered = false;
            }
        }
    }
}

[Serializable]
public class GridLogData
{
    public int cellId;
    public float timeElapsed;
    public float timeInBox;
    public GridLogData(int aCellId, float aTimeElapsed, float aTimeInBox)
    {
        cellId = aCellId;
        timeElapsed = aTimeElapsed;
        timeInBox = aTimeInBox;
    }
}

[Serializable]
public class MyCollection
{
    [SerializeField]
    public List<GridLogData> gridLogData = new List<GridLogData>();
}
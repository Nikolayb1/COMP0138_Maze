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
    public bool isRotation;
    private List<LogCell> gridCells = new List<LogCell>();
    Dictionary<int, float> enteredCells = new Dictionary<int, float>();
    public MyCollection myCollection = new MyCollection();
    // Start is called before the first frame update
    void Start()
    {
        if (!isRotation)
        {
            for (float i = 0; i < 12; i += 2)
            {
                for (float j = 0; j < 12; j += 2)
                {
                    GameObject cell = Instantiate(gridCell, new Vector3(i, 1.5f, j), Quaternion.Euler(0, 0, 0));
                    gridCells.Add(cell.GetComponent<LogCell>());
                }
            }
        }
        else
        {
            for (float i = 0; i < 30; i += 2)
            {
                for (float j = 0; j < 34; j += 2)
                {
                    GameObject cell = Instantiate(gridCell, new Vector3(i-16, 1.5f, j), Quaternion.Euler(0, 0, 0));
                    gridCells.Add(cell.GetComponent<LogCell>());
                }
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
                myCollection.gridLogData.Add(new GridLogData(e.Key, Time.fixedTime, Time.fixedTime - e.Value));
                temp.Add(e.Key);
            }
        }

        foreach(int k in temp)
        {
            enteredCells.Remove(k);
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitCircleMaze : MonoBehaviour
{
    public GameObject outerCell;
    public GameObject middleCell;
    public GameObject innerCell;
    // Start is called before the first frame update
    void Start()
    {
        GameObject tmp;
        for (int i = 0; i < 5; i++)
        {
            tmp = Instantiate(outerCell, new Vector3(0, 0, 0), Quaternion.Euler(0, 365/5 * i, 0));
            tmp.transform.parent = transform;
            tmp = Instantiate(middleCell, new Vector3(0, 0, 0), Quaternion.Euler(0, 365 / 5 * i, 0));
            tmp.transform.parent = transform;
            tmp = Instantiate(innerCell, new Vector3(0, 0, 0), Quaternion.Euler(0, 365 / 5 * i, 0));
            tmp.transform.parent = transform;
        }

        transform.position = new Vector3(-20, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

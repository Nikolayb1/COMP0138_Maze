using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGoal : MonoBehaviour
{ 
    public GameObject EndObject;

    private Collider StartObjectCollider;
    private Collider PlayerCollider;
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(EndObject, new Vector3(8f, 1f, 9f), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}

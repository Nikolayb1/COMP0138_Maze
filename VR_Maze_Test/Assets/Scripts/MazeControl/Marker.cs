using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marker : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    // Spawns the marker in a random cell
    
    // When Marker was entered
    protected virtual void OnTriggerEnter(Collider other)
    {
        if(other.name == "XRRig")
        {
            Destroy(this.gameObject);
        }
    }
}

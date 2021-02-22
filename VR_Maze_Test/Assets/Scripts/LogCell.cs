using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogCell : MonoBehaviour
{
    public bool entered;
    // Start is called before the first frame update
    void Start()
    {
        entered = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "XRRig")
        {
            entered = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "XRRig")
        {
            entered = false;
        }
    }
}

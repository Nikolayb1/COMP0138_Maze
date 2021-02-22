using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marker : MonoBehaviour
{
    private bool isTutorial;
    public GridLogger gl;
    public Logger l;

    // Start is called before the first frame update
    void Start()
    {
        gl = FindObjectOfType<GridLogger>();
        l = FindObjectOfType<Logger>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setTutorial(bool b)
    {
        isTutorial = b;
    }
    // Spawns the marker in a random cell

    private void markerFoundEvent()
    {
        string val = gl.dataToJson();
        l.LogEvent("MF", val);
        gl.clearLog();
    }

    // When Marker was entered
    protected virtual void OnTriggerEnter(Collider other)
    {
        if(other.name == "XRRig")
        {
            Destroy(this.gameObject);
            if (!isTutorial)
            {
                markerFoundEvent();
            }
        }
    }
}

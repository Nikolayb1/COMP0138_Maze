using UnityEngine;

public class IdManager : MonoBehaviour
{
    public Logger l;
    // Start is called before the first frame update
    void Start()
    {


        CrossSceneData.CrossSceneId = 0;
        //l.LogEvent("ID", CrossSceneData.CrossSceneId.ToString());

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IdManager : MonoBehaviour
{
    public Text idText;
    // Start is called before the first frame update
    void Start()
    {
        idText.text = CrossSceneData.CrossSceneId.ToString();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

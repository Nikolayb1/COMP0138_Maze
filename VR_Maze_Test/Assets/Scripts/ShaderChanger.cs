using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderChanger : MonoBehaviour
{
    public Shader wireframe;
    public Material rocks;
    public bool isMaterial1;
    Renderer rend;
    // Start is called before the first frame update
    void Start()
    {
        isMaterial1 = true;
        rend = GetComponent<Renderer>();
        rend.material.shader = wireframe;
        rend.material = rocks;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeMaterial()
    {
        if (isMaterial1)
        {
            rend.material = rocks;
            isMaterial1 = false;
        }
        else
        {
            rend.material.shader = wireframe;
            isMaterial1 = true;
        }
    }
}

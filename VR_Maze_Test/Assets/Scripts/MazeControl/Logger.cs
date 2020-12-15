using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
public class Logger : MonoBehaviour
{
    // Start is called before the first frame update
    public void addRecord(string transformX, string transformY, string transformZ, string rotateX, string rotateY, string rotateZ)
    {
        try
        {
            using(StreamWriter file = new StreamWriter(@"try.csv", true))
            {
                file.WriteLine(transformX, "," + transformY + "," + transformZ + "," + rotateX + "," + rotateY + "," + rotateZ);
            }
        }
        catch(Exception ex)
        {
            throw new ApplicationException("Oppsie :", ex);
        }
    }
}

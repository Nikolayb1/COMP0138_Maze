﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*TODO:
 + Disable teleporter
 + Enable Pointer
 + Get position of player
 + Get position of Start
 + Wait for input
 + Get direction of ray
 + Get Angle
 */

public class Rays : MonoBehaviour
{
    public GameObject player;

    public GameObject teleportLine;
    public GameObject pointerLine;
    public GameObject raticle;
    public GameObject fog;

    public UnityEngine.XR.Interaction.Toolkit.TeleportationArea ta;

    private bool pointerEnabled;

    private Vector3 playerPos;
    private Vector3 originPos;
    private Vector3 pointPos;

    private Vector3 Test;
    // Start is called before the first frame update
    void Start()
    {
        teleportLine.SetActive(true);
        pointerLine.SetActive(false);
        ta.customReticle = raticle;
        pointerEnabled = false;
    }

    public void InitRays()
    {
        fog.SetActive(false);
        playerPos = player.transform.position;
        originPos = new Vector3(0,0,0);
        Ray r = new Ray(playerPos, originPos);
        Test = r.direction;
        teleportLine.SetActive(false);
        pointerLine.SetActive(true);
        ta.customReticle = null;
        pointerEnabled = true;

    }

    public void ResetRays()
    {
        teleportLine.SetActive(true);
        pointerLine.SetActive(false);
        ta.customReticle = raticle;
        pointerEnabled = false;
    }

    public void SetPointValue(Vector3 p)
    {
        pointPos = p;
    }

    public float CalculateAngle()
    {
        Vector2 firstPlayer = new Vector2(Test[0], Test[2]);
        Vector2 secondPlayer = new Vector2(pointPos[0], pointPos[2]);
        Debug.Log(pointPos);
        float AngleRad = Mathf.Atan2(firstPlayer.y - secondPlayer.y, firstPlayer.x - secondPlayer.x);
        float AngleDeg = (180 / Mathf.PI) * AngleRad;
        return AngleDeg;
    }

    public bool isPointer()
    {
        return pointerEnabled;
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }
}
